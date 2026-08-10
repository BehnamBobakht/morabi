using DataLayer.Base;
using DataLayer.Entities;
using DataLayer.Enumarables;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace DataLayer.Repositories
{
    public class Entity_AlexaRank : BaseRepository<AlexaRank>
    {
        private DatabaseEntities _context;
        public Entity_AlexaRank(DatabaseEntities context) : base(context)
        {
            _context = context;
        }

        public void CheckAlexaRank(UnitOfWork _context)
        {
            List<Website> websiteList = _context.Website.GetAllByType(
                Enum_Code.SYSTEM_TYPE_CMS, 
                Enum_Code.SYSTEM_TYPE_SHOP, 
                Enum_Code.SYSTEM_TYPE_WEBSITE);
            foreach (Website item in websiteList)
            {
                foreach (WebsiteDomain domain in item.WebsiteDomain.Where(p => 
                    p.Domain.Contains("localhost") == false && 
                    p.Domain.Contains("*") == false))
                {
                    try
                    {
                        var client = new RestClient("http://data.alexa.com/");
                        var request = new RestRequest("data", Method.GET);
                        request.Parameters.Clear();
                        request.AddQueryParameter("cli", "100");
                        request.AddQueryParameter("dat", "snbamz");
                        request.AddQueryParameter("url", domain.Domain);
                        IRestResponse response = client.Execute(request);

                        XmlDocument xml = new XmlDocument();
                        xml.LoadXml(response.Content);
                        
                        XmlNode popularityNode = xml.SelectSingleNode("/ALEXA/SD/POPULARITY");
                        XmlNode reachNode = xml.SelectSingleNode("/ALEXA/SD/REACH");
                        XmlNode rankNode = xml.SelectSingleNode("/ALEXA/SD/RANK");
                        XmlNode countryNode = xml.SelectSingleNode("/ALEXA/SD/COUNTRY");

                        string countryName = countryNode != null ? countryNode.Attributes["NAME"].Value : null;
                        int? countryRank = countryNode != null ? countryNode.Attributes["RANK"].Value.GetInteger() : default(int?);
                        int? globalRank = popularityNode != null ? popularityNode.Attributes["TEXT"].Value.GetInteger() : default(int?);
                        int? reachDelta = rankNode != null ? rankNode.Attributes["DELTA"].Value.GetInteger() : default(int?);
                        int? reachRank = reachNode != null ? reachNode.Attributes["RANK"].Value.GetInteger() : default(int?);

                        AlexaRank lastRank = domain.AlexaRank.OrderByDescending(p => p.ID).FirstOrDefault();
                        AlexaRank rank = new AlexaRank()
                        {
                            CountryName = countryName,
                            CountryRank = countryRank,
                            Datetime = DateTime.Now,
                            DomainId = domain.ID,
                            GlobalRank = globalRank,
                            ReachDelta = reachDelta,
                            ReachRank = reachRank
                        };

                        if (lastRank != null)
                        {
                            if (lastRank.CountryRank != rank.CountryRank ||
                                lastRank.GlobalRank != rank.GlobalRank ||
                                lastRank.ReachDelta != rank.ReachDelta ||
                                lastRank.ReachRank != rank.ReachRank)
                            {
                                _context.AlexaRank.Insert(rank);
                                _context.Save();
                            }
                        }
                        else
                        {
                            _context.AlexaRank.Insert(rank);
                            _context.Save();
                        }
                    }
                    catch (Exception) { }
                }
            }
        }
    }
}
