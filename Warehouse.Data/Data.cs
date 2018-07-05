using System;
using System.Collections.Generic;
using System.Linq;
using Data.Models;

namespace Warehouse.Data
{
    public class DataFunctions
    {
        public delegate void LogExceptionFunction(string type, string errorText, string callStack, string time);

        public static List<Site> GetSites(bool empty, bool full, LogExceptionFunction LogException)
        {
            try
            {
                using (WarehouseContext db = new WarehouseContext())
                {
                    return db.sites.Where(e => (empty || !e.empty) && (full || e.capacity != 0)).ToList();
                }
            }
            catch (Exception e)
            {
                LogException(e.GetType().ToString(), e.Message, e.StackTrace, DateTime.Now.ToString());
                return new List<Site>();
            }
        }

        public static Hangar GetHangar(string id, LogExceptionFunction LogException)
        {
            try
            {
                using (WarehouseContext db = new WarehouseContext())
                {
                    return db.hangars.Where(e => e.id == id).FirstOrDefault();
                }
            }
            catch (Exception e)
            {
                LogException(e.GetType().ToString(), e.Message, e.StackTrace, DateTime.Now.ToString());
                return null;
            }
        }

        public static List<Hangar> GetHangars(string siteId, bool empty, bool full, LogExceptionFunction LogException)
        {
            try
            {
                using (WarehouseContext db = new WarehouseContext())
                {
                    return db.hangars.Where(e => e.site_id == siteId && (empty || e.fullness != 0) && (full || e.capacity != e.fullness)).ToList();
                }
            }
            catch (Exception e)
            {
                LogException(e.GetType().ToString(), e.Message, e.StackTrace, DateTime.Now.ToString());
                return new List<Hangar>();
            }
        }

        public static bool ModifyHangar(string id, int N, LogExceptionFunction LogException)
        {
            try
            {
                using (WarehouseContext db = new WarehouseContext())
                {
                    Hangar hangar = GetHangar(id, LogException);
                    if (hangar == null)
                    {
                        return false;
                    }
                    int newFullness = hangar.fullness + N;
                    if (newFullness >= 0 && newFullness <= hangar.capacity)
                    {
                        db.hangars.Where(e => e.id == id).FirstOrDefault().fullness = newFullness;
                        db.sites.FirstOrDefault(e => e.id == hangar.site_id).capacity -= N;
                        if (newFullness == 0)
                        {
                            if (!db.hangars.Any(e => e.site_id == id && e.fullness > 0))
                            {
                                db.sites.FirstOrDefault(e => e.id == hangar.site_id).empty = true;
                            }
                        }
                        else if (db.sites.FirstOrDefault(e => e.id == hangar.site_id).empty)
                        {
                            db.sites.FirstOrDefault(e => e.id == hangar.site_id).empty = false;
                        }
                        db.SaveChanges();
                        return true;
                    }
                    return false;
                }
            }
            catch (Exception e)
            {
                LogException(e.GetType().ToString(), e.Message, e.StackTrace, DateTime.Now.ToString());
                return false;
            }
        }
    }
}
