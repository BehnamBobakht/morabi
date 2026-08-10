using DataLayer.Entities;
using DataLayer.Enumarables;
using OpenPop.Mime;
using OpenPop.Pop3;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Schedules
{
    public class Sync_EmailInbox : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            UnitOfWork _context = new UnitOfWork();
            SyncLog log = new SyncLog() {
                Name = "Email Inbox",
                StartDatetime = DateTime.Now
            };
            try
            {
                List<EmailAddress> listEmails = _context.EmailAddress.GetEmailsForSync();
                foreach (EmailAddress item in listEmails)
                {
                    Pop3Client client = new Pop3Client();
                    client.Connect(item.EmailHost.Pop3, item.EmailHost.Pop3Port, item.EmailHost.Ssl);
                    client.Authenticate(item.Email, item.Password);

                    int CurrentCount = _context.EmailInbox.GetEmailInboxCountByEmailAddress(item);
                    int EmailCount = client.GetMessageCount();
                    
                    if (EmailCount > CurrentCount)
                    {
                        int FetchCount = 0;
                        for (int i = CurrentCount + 1; i < CurrentCount + 10 && i < EmailCount + 1; i++)
                        {
                            Message msg = client.GetMessage(i);
                            EmailInbox inbox = new EmailInbox();
                            inbox.EmailId = item.ID;
                            inbox.UserId = item.UserId;
                            inbox.StatusId = _context.Code.GetByLabel(Enum_Code.EMAIL_STATUS_INBOX).ID;
                            inbox.CreateDatetime = DateTime.Now;
                            inbox.EmailPopNumber = i;
                            inbox.EmailUniqueId = msg.Headers.MessageId;
                            inbox.IsRead = false;
                            inbox.IsDelete = false;
                            inbox.Subject = msg.Headers.Subject;
                            inbox.FromEmail = msg.Headers.From.Address;
                            inbox.FromDisplayName = msg.Headers.From.DisplayName;
                            inbox.EmailDatetime = msg.Headers.DateSent;
                            if (msg.MessagePart.MessageParts[0].Body != null)
                                inbox.BodyText = Encoding.UTF8.GetString(msg.MessagePart.MessageParts[0].Body);
                            if (msg.MessagePart.MessageParts[1].Body != null)
                                inbox.BodyHtml = Encoding.UTF8.GetString(msg.MessagePart.MessageParts[1].Body);
                            _context.EmailInbox.Insert(inbox);
                            FetchCount++;
                        }
                        log.Description = "6 New Email For " + item.Email;
                    }
                    else
                    {
                        log.Description = "No New Email For " + item.Email;
                    }
                    client.Disconnect();
                    client.Dispose();
                    log.Status = "OK";
                }
            }
            catch (Exception ex)
            {
                log.Status = "ERROR";
                log.Description = ex.Message;
            }
            log.EndDatetime = DateTime.Now;
            //_context.SyncLog.Insert(log);
            //_context.Save();
        }
    }
}
