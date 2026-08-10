using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.ViewModels
{
    public class StoreShowVm
    {
        public int? ID { get; set; }

        public Nullable<int> AccountId { get; set; }
        public string Name { get; set; }
        public string AddressValue { get; set; }
        public string Phone1 { get; set; }
        public string Phone2 { get; set; }
        public string Fax { get; set; }
        public string Website { get; set; }
        public string Description { get; set; }    
        public  PictureVm Picture { get; set; }
        public List<CompetitiveFeatureVm> CompetitiveFeature { get; set; }

    }
}
