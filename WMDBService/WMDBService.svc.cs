using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace WMDBService
{
    public class Service1 : IWMDBService
    {
        public List<Site> GetAllSites()
        {
            List<Site> sites = new List<Site>();
            for (int i = 0; i < 10; i++)           //здесь выборка из бд должна быть
            {
                sites.Add(new Site()
                {
                    Id = i.ToString(),
                    Empty = false,
                    Capacity = 200
                });
            }
            return sites;
        }

        public List<Site> GetSites(bool empty, bool full)  //where empty or full
        {
            List<Site> sites = new List<Site>();
            for (int i = 0; i < 10; i++)           //здесь выборка из бд должна быть
            {
                sites.Add(new Site()
                {
                    Id = i.ToString(),
                    Empty = empty,
                    Capacity = 200
                });
            }
            return sites;
        }

        public bool SiteExists(string id)
        {
            return true;//существует запись из site с id
        }

        public List<Hangar> GetAllHangars(string siteId)
        {
            List<Hangar> hangars = new List<Hangar>();
            for (int i = 0; i < 10; i++)           //здесь выборка из бд должна быть
            {
                hangars.Add(new Hangar()
                {
                    Id = i.ToString(),
                    SiteId = siteId,
                    Capacity = 20,
                    Fullness = 10
                });
            }
            return hangars;
        }

        public List<Hangar> GetHangars(string siteId, bool empty, bool full)  //where empty or full
        {
            List<Hangar> hangars = new List<Hangar>();
            for (int i = 0; i < 10; i++)           //здесь выборка из бд должна быть
            {
                hangars.Add(new Hangar()
                {
                    Id = i.ToString(),
                    SiteId = siteId,
                    Capacity = 20,
                    Fullness = empty ? 0 : 10
                });
            }
            return hangars;
        }

        public int GetFreePlacesCount()
        {
            return 2000;   //возврат суммы capacity у записей из site
        }

        public bool ModifyHangar(string id, int N)
        {
            throw new NotImplementedException();
        }
    }
}
