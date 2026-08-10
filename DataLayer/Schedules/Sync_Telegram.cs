using DataLayer.Entities;
using DataLayer.Enumarables;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot.Types.InlineKeyboardButtons;
using Telegram.Bot.Types.ReplyMarkups;

namespace DataLayer.Schedules
{
    public class Sync_Telegram : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            if (Sync_Base.Is_Telegram_Syncing == false)
            {
                Sync_Base.Is_Telegram_Syncing = true;

                UnitOfWork _context = new UnitOfWork();
                int count = _context.TelegramBot.Count();
                if (count > 0)
                {
                    DoTelegramJobs();
                }
                _context.Dispose();

                Sync_Base.Is_Telegram_Syncing = false;
            }
        }

        private void DoTelegramJobs()
        {
            UnitOfWork _tempcontext = new UnitOfWork();
            List<TelegramBot> list = _tempcontext.TelegramBot.GetAll();
            _tempcontext.Dispose();
            while (true)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    foreach (TelegramBot telegramBot in list)
                    {
                        try
                        {
                            Telegram.Bot.TelegramBotClient bot = new Telegram.Bot.TelegramBotClient(telegramBot.Token);
                            Telegram.Bot.Types.Update[] update = bot.GetUpdatesAsync(telegramBot.Offset).Result;
                            if (update.Length > 0)
                            {
                                UnitOfWork _context = new UnitOfWork();
                                List<TelegramCommand> listCommand = _context.TelegramCommand.GetAllByBotId(telegramBot.ID);
                                foreach (var up in update)
                                {
                                    if (up.Message == null)
                                        continue;

                                    var text = up.Message.Text;
                                    var from = up.Message.From;
                                    var chatId = up.Message.Chat.Id;

                                    if (text != null)
                                    {
                                        text = text.Trim();
                                        TelegramCommand command = listCommand.FirstOrDefault(p => p.Command == text);
                                        if (command != null)
                                        {
                                            Telegram.Bot.Types.Enums.ParseMode parseMode = Telegram.Bot.Types.Enums.ParseMode.Default;
                                            if (command.ParseMode == "Markdown")
                                                parseMode = Telegram.Bot.Types.Enums.ParseMode.Markdown;
                                            else if (command.ParseMode == "Html")
                                                parseMode = Telegram.Bot.Types.Enums.ParseMode.Html;
                                            else
                                                parseMode = Telegram.Bot.Types.Enums.ParseMode.Default;

                                            bool webPagePreview = !command.DisableWebPagePreview;
                                            bool notification = !command.DisableNotification;
                                            string replyText = GetStandardReplyText(command, from);

                                            if (command.Code.Label == Enum_Code.TELEGRAM_COMMAND_TYPE_TEXT.ToString())
                                            {
                                                bot.SendTextMessageAsync(chatId, replyText, parseMode, webPagePreview, notification);
                                            }
                                            else if (command.Code.Label == Enum_Code.TELEGRAM_COMMAND_TYPE_PICTURE.ToString())
                                            {
                                                
                                            }
                                            else if (command.Code.Label == Enum_Code.TELEGRAM_COMMAND_TYPE_VOICE.ToString())
                                            {

                                            }
                                            else if (command.Code.Label == Enum_Code.TELEGRAM_COMMAND_TYPE_VIDEO.ToString())
                                            {

                                            }
                                            else if (command.Code.Label == Enum_Code.TELEGRAM_COMMAND_TYPE_KEYBOARD.ToString())
                                            {
                                                if (command.KeyboardId != null)
                                                {
                                                    IReplyMarkup iKeyboard = null;
                                                    List<TelegramKeyBoardItem> listItems = command.TelegramKeyBoard.TelegramKeyBoardItem.ToList();
                                                    var results = from p in listItems
                                                                  group p by p.Row into g
                                                                  select new { RowId = g.Key, Items = g.ToList() };
                                                    if (command.TelegramKeyBoard.Code.Label == Enum_Code.TELEGRAM_KEYBOARD_TYPE_BUTTON.ToString())
                                                    {
                                                        ReplyKeyboardMarkup keyboard = new ReplyKeyboardMarkup();
                                                        List<KeyboardButton[]> listRows = new List<KeyboardButton[]>();
                                                        foreach (var groupItem in results)
                                                        {
                                                            List<KeyboardButton> row = new List<KeyboardButton>();
                                                            foreach (TelegramKeyBoardItem item in groupItem.Items.OrderBy(p => p.ShowNumber))
                                                            {
                                                                row.Add(item.Name);
                                                            }
                                                            listRows.Add(row.ToArray());
                                                        }
                                                        keyboard.Keyboard = listRows.ToArray();
                                                        iKeyboard = keyboard;
                                                    }
                                                    else
                                                    {
                                                        InlineKeyboardMarkup keyboard = new InlineKeyboardMarkup();
                                                        List<InlineKeyboardUrlButton[]> listRows = new List<InlineKeyboardUrlButton[]>();
                                                        foreach (var groupItem in results)
                                                        {
                                                            List<InlineKeyboardUrlButton> row = new List<InlineKeyboardUrlButton>();
                                                            foreach (TelegramKeyBoardItem item in groupItem.Items.OrderBy(p => p.ShowNumber))
                                                            {
                                                                row.Add(new InlineKeyboardUrlButton(item.Name, item.Url));
                                                            }
                                                            listRows.Add(row.ToArray());
                                                        }
                                                        keyboard.InlineKeyboard = listRows.ToArray();
                                                        iKeyboard = keyboard;
                                                    }
                                                    bot.SendTextMessageAsync(chatId, replyText, parseMode, webPagePreview, notification, 0, iKeyboard);
                                                }
                                            }
                                            else if (command.Code.Label == Enum_Code.TELEGRAM_COMMAND_TYPE_SURVEY.ToString())
                                            {

                                            }
                                        }
                                    }

                                    telegramBot.Offset = up.Id + 1;
                                    _context.Save();

                                    DoTelegramAccount(_context, telegramBot, from, up.Message.Contact);
                                }
                                _context.Dispose();
                            }
                        }
                        catch (Exception ex) {
                            string msg = ex.Message;
                        }
                    }
                }
            }
        }

        private string GetStandardReplyText(TelegramCommand command, User user)
        {
            string text = command.TextMessage;
            text = text.Replace("%FIRSTNAME%", user.FirstName);
            text = text.Replace("%LASTNAME%", user.LastName);
            text = text.Replace("%FULLNAME%", user.FirstName + " " + user.LastName);
            return text;
        }

        private void DoTelegramAccount(UnitOfWork _context, TelegramBot bot, User telegramUser, Telegram.Bot.Types.Contact telegramContact)
        {
            TelegramAccount account = _context.TelegramAccount.GetByBotAndTelegramId(bot.ID, telegramUser.Id);
            bool IsInsert = account == null ? true : false;
            if (IsInsert == true)
            {
                account = new TelegramAccount() {
                    CreateDatetime = DateTime.Now
                };
            }

            account.BotId = bot.ID;
            account.ChatId = telegramUser.Id.ToString();
            account.UpdateDatetime = DateTime.Now;
            account.IsBot = telegramUser.IsBot;
            account.Username = telegramUser.Username;
            account.FirstName = telegramUser.FirstName;
            account.LastName = telegramUser.LastName;
            account.LanguageCode = telegramUser.LanguageCode;
            if (telegramContact != null)
            {
                string mobile = telegramContact.PhoneNumber;
                if (mobile.StartsWith("98"))
                {
                    Regex regex = new Regex(Regex.Escape("98"));
                    string newMobile = regex.Replace(mobile, "0", 1);
                    account.Mobile = newMobile;

                    Account tempAccount = _context.Account.GetByMobile(mobile);
                    if (tempAccount != null)
                        account.AccountId = tempAccount.ID;

                    SiteUser tempUser = _context.SiteUser.GetByMobile(newMobile, true);
                    if (tempUser != null)
                        account.UserId = tempUser.ID;
                }
            }
            
            if (IsInsert == true)
                _context.TelegramAccount.Insert(account);
            else
                _context.TelegramAccount.Update(account);

            _context.Save();
        }
    }
}
