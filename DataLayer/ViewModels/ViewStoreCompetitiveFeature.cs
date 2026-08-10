using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.ViewModels
{
    public class ViewStoreCompetitiveFeature
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public bool Status { get; set; }

        public ViewStoreCompetitiveFeature(StoreCompetitiveFeature competitiveFeature)
        {
            this.Id = competitiveFeature.ID;
            this.Text = competitiveFeature.CompetitiveFeature.TexCompetitiveFeature;
        }

        public ViewStoreCompetitiveFeature()
        {

        }


    }
}
