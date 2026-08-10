using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Helpers.Notification
{
    public class OneSignalApp
    {
        public Guid id { get; set; }
        public string name { get; set; }
        public string gcm_key { get; set; }
        public string chrome_key { get; set; }
        public string chrome_web_key { get; set; }
        public string chrome_web_origin { get; set; }
        public string chrome_web_gcm_sender_id { get; set; }
        public string chrome_web_default_notification_icon { get; set; }
        public string chrome_web_sub_domain { get; set; }
        public string apns_env { get; set; }
        public string apns_certificates { get; set; }
        public string site_name { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
        public int players { get; set; }
        public int messageable_players { get; set; }
        public string basic_auth_key { get; set; }
    }
}
