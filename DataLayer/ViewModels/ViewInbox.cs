using DataLayer.Entities;
using DataLayer.Enumarables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.ViewModels
{
    public class ViewInbox
    {
        public int Inbox { get; set; }
        public int Important { get; set; }
        public int Sent { get; set; }
        public int Draft { get; set; }
        public int Trash { get; set; }

        public ViewInbox(SiteUser user)
        {
            List<EmailInbox> list = user.EmailInbox.Where(p => p.IsDelete == false).ToList();
            this.Inbox = list.Count(p => p.Code.Label == Enum_Code.EMAIL_STATUS_INBOX.ToString() && p.IsRead == false);
            this.Important = list.Count(p => p.Code.Label == Enum_Code.EMAIL_STATUS_IMPORTANT.ToString() && p.IsRead == false);
            this.Sent = list.Count(p => p.Code.Label == Enum_Code.EMAIL_STATUS_SENT.ToString() && p.IsRead == false);
            this.Draft = list.Count(p => p.Code.Label == Enum_Code.EMAIL_STATUS_DRAFT.ToString() && p.IsRead == false);
            this.Trash = list.Count(p => p.Code.Label == Enum_Code.EMAIL_STATUS_TRASH.ToString() && p.IsRead == false);
        }
    }
}
