using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using Warehouse.Data;

namespace DataService
{
    public class DataService : IDataService
    {
        static void LogException(string type, string errorText, string callStack, string time)
        {

        }
        DataFunctions.LogExceptionFunction log = new DataFunctions.LogExceptionFunction(LogException);

        public List<Site> GetAllSites()
        {
            return GetSites();
        }

        public List<Site> GetSites(bool empty = true, bool full = true)
        {
            List<Site> sites = new List<Site>();
            DataFunctions.GetSites(empty, full, log).ForEach(e => sites.Add(new Site()
            {
                Id = e.id,
                Capacity = e.capacity,
                Empty = e.empty
            }));
            return sites;
        }

        public bool HangarExists(string hangarId)
        {
            return DataFunctions.GetHangar(hangarId, log) != null;
        }

        public Hangar GetHangar(string hangarId)
        {
            var h = DataFunctions.GetHangar(hangarId, log);
            return h == null ? null : new Hangar()
            {
                Id = h.id,
                SiteId = h.site_id,
                Capacity = h.capacity,
                Fullness = h.fullness
            };
        }

        public List<Hangar> GetAllHangars(string siteId)
        {
            return GetHangars(siteId);
        }

        public List<Hangar> GetHangars(string siteId, bool empty = true, bool full = true)
        {
            List<Hangar> hangars = new List<Hangar>();
            DataFunctions.GetHangars(siteId, empty, full, log).ForEach(e => hangars.Add(new Hangar()
            {
                Id = e.id,
                SiteId = e.site_id,
                Capacity = e.capacity,
                Fullness = e.fullness
            }));
            return hangars;
        }

        public int GetFreePlacesCount()
        {
            return DataFunctions.GetSites(true, false, log).Sum(e => e.capacity);
        }

        public bool ModifyHangar(string id, int N)
        {
            if (N != 0)
            {
                return DataFunctions.ModifyHangar(id, N, log);
            }
            return false;
        }
    }
}
