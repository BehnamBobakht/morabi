using DataLayer.Base;
using DataLayer.Entities;
using DataLayer.Enumarables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;

namespace DataLayer.Repositories
{
    public class Entity_Email : BaseRepository<Email>
    {
        private DatabaseEntities _context;
        public Entity_Email(DatabaseEntities context) : base(context)
        {
            _context = context;
        }

        public void SaveNewEmail(string receiver, Enum_EmailType type, string body, string title)
        {
            try
            {
                if (string.IsNullOrEmpty(receiver) == false && BaseSecurity.IsValidInput(receiver, Enum_Validation.EMAIL))
                {
                    string typeString = type.ToString().ToUpper();
                    EmailType emailType = _context.EmailType.FirstOrDefault(p => p.Label == typeString);
                    if (emailType == null)
                        return;
                    EmailAddress address = _context.EmailAddress.FirstOrDefault(p => p.EmailSetting.Any(q => q.EmailTypeId == emailType.ID));
                    if (address == null)
                        return;
                    int? emailTypeId = emailType == null ? default(int?) : emailType.ID;

                    string bodyTemplate = address.EmailHost.Body;
                    // && (bodyTemplate.Contains(body) == true))//.Contains("${BODY}") == true))
                    if ((bodyTemplate != null) && (bodyTemplate.Contains("${BODY}") == true))
                    {
                        body = bodyTemplate
                            .Replace("${TITLE}", title)
                            .Replace("${BODY}", body);
                    }

                    Email message = new Email()
                    {
                        Body = body,
                        TypeId = emailTypeId,
                        CreateDatetime = DateTime.Now,
                        LastSendDatetime = DateTime.Now,
                        Receive = receiver,
                        Sender = address.Email,
                        IsSend = false,
                        Title = title,
                        AddressId = address.ID
                    };
                    Insert(message);
                    Save();
                }
            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {
                throw;
            }
        }

        public void SaveNewEmailByAttach(string receiver, Enum_EmailType type, string body, string title, ref int id)
        {
            try
            {
                if (string.IsNullOrEmpty(receiver) == false && BaseSecurity.IsValidInput(receiver, Enum_Validation.EMAIL))
                {
                    string typeString = type.ToString().ToUpper();
                    EmailType emailType = _context.EmailType.FirstOrDefault(p => p.Label == typeString);
                    if (emailType == null)
                        return;
                    EmailAddress address = _context.EmailAddress.FirstOrDefault(p => p.EmailSetting.Any(q => q.EmailTypeId == emailType.ID));
                    if (address == null)
                        return;
                    int? emailTypeId = emailType == null ? default(int?) : emailType.ID;

                    string bodyTemplate = address.EmailHost.Body;
                    // && (bodyTemplate.Contains(body) == true))//.Contains("${BODY}") == true))
                    if ((bodyTemplate != null) && (bodyTemplate.Contains("${BODY}") == true))
                    {
                        body = bodyTemplate
                            .Replace("${TITLE}", title)
                            .Replace("${BODY}", body);
                    }

                    Email message = new Email()
                    {
                        Body = body,
                        TypeId = emailTypeId,
                        CreateDatetime = DateTime.Now,
                        LastSendDatetime = DateTime.Now,
                        Receive = receiver,
                        Sender = address.Email,
                        IsSend = false,
                        Title = title,
                        AddressId = address.ID
                    };
                    Insert(message);
                    Save();
                    id = message.ID;

                }
            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {
                throw;
            }
        }

        public void SendCenter()
        {
            List<Email> list = _context.Email
                .Where(p => p.IsSend == false)
                .Take(100)
                .ToList();
            if (list.Count > 0)
            {
                foreach (Email item in list)
                {
                    item.IsSend = true;
                }
                _context.SaveChanges();
                var smsTypes = list.GroupBy(p => p.TypeId);
                foreach (var item in smsTypes)
                {
                    try
                    {
                        EmailSetting setting = _context.EmailSetting.FirstOrDefault(p => p.EmailTypeId == item.Key);
                        if (setting != null)
                        {
                            List<Email> listEmailType = list.Where(p => p.TypeId == item.Key).ToList();
                            EmailHost provider = setting.EmailAddress.EmailHost;
                            EmailAddress sender = setting.EmailAddress;
                            EmailType type = setting.EmailType;
                            foreach (Email email in listEmailType)
                            {
                                try
                                {
                                    SmtpClient smtpClient = new SmtpClient();
                                    NetworkCredential basicCredential = new NetworkCredential(email.EmailAddress.Email, email.EmailAddress.Password);
                                    MailMessage message = new MailMessage();
                                    MailAddress fromAddress = new MailAddress(email.EmailAddress.Email);
                                    smtpClient.Host = email.EmailAddress.EmailHost.Smtp;
                                    smtpClient.UseDefaultCredentials = false;
                                    smtpClient.Credentials = basicCredential;
                                    message.From = fromAddress;
                                    message.Subject = email.Title;
                                    message.IsBodyHtml = true;
                                    message.Body = email.Body;

                                    List<EmailAttachment> listAttach = _context.EmailAttachment.Where(x => x.EmailId == email.ID).ToList();
                                    if (listAttach != null)
                                    {
                                        foreach (var att in listAttach)
                                        {
                                            try
                                            {
                                                string address = att.Url;
                                                message.Attachments.Add(new Attachment(address));
                                            }
                                            catch
                                            {

                                            }

                                            //System.Net.Mail.Attachment attachment;
                                            //attachment = new System.Net.Mail.Attachment(address);
                                            //message.Attachments.Add(attachment);
                                        }
                                    }

                                    message.To.Add(email.Receive);
                                    smtpClient.Send(message);

                                    //SmtpClient smtpClient = new SmtpClient();
                                    //NetworkCredential basicCredential = new NetworkCredential("test@behpardazan.com", "123@123aAmM");
                                    //MailMessage message = new MailMessage();
                                    //MailAddress fromAddress = new MailAddress("test@behpardazan.com");
                                    //smtpClient.Host = "mail.behpardazan.com";
                                    //smtpClient.UseDefaultCredentials = false;
                                    //smtpClient.Credentials = basicCredential;
                                    //message.From = fromAddress;
                                    //message.Subject = "this is subject on server";
                                    //message.IsBodyHtml = true;
                                    //message.Body = "inam body";
                                    //message.To.Add("torabi6@gmail.com");
                                    //smtpClient.Send(message);
                                }
                                catch (Exception ex)
                                {
                                    email.Body = ex.Message + " " + ex.InnerException != null ? ex.InnerException.Message : "";
                                    _context.SaveChanges();
                                }
                            }
                        }
                        _context.SaveChanges();
                    }
                    catch (Exception) { }
                }
            }
        }
    }
}
