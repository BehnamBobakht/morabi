using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.ViewModels
{
    public class ViewExameAccountAnswer
    {
        public int ID { get; set; }
        public string QuestionAnswer { get; set; }
        public int ? IDRandom { get; set; }
        public bool ? Open { get; set; }
    }
}
