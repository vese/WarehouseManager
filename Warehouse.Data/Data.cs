using System;
using System.Collections.Generic;
using System.Linq;
using Data.Models;

namespace Warehouse.Data
{
    public class DataFunctions
    {
        public static List<Site> GetSites(bool empty, bool full, out bool success, out string exceptionString)
        {
            try
            {
                using (WarehouseContext db = new WarehouseContext())
                {
                    success = true;
                    exceptionString = null;
                    return db.sites.Where(e => (empty || !e.empty) && (full || e.capacity != 0)).ToList();
                }
            }
            catch (Exception e)
            {
                ExceptionLogger.Instance.LogException(e.ToString(), DateTime.UtcNow);
                success = false;
                exceptionString = e.ToString();
                return null;
            }
        }

        public static Hangar GetHangar(string id, out bool exist, out bool success, out string exceptionString)
        {
            try
            {
                using (WarehouseContext db = new WarehouseContext())
                {
                    exist = db.hangars.Any(e => e.id == id);
                    success = true;
                    exceptionString = null;
                    return db.hangars.FirstOrDefault(e => e.id == id);
                }
            }
            catch (Exception e)
            {
                ExceptionLogger.Instance.LogException(e.ToString(), DateTime.UtcNow);
                exist = success = false;
                exceptionString = e.ToString();
                return null;
            }
        }

        public static List<Hangar> GetHangars(string siteId, bool empty, bool full, out bool success, out string exceptionString)
        {
            try
            {
                using (WarehouseContext db = new WarehouseContext())
                {
                    success = true;
                    exceptionString = null;
                    return db.hangars.Where(e => e.site_id == siteId && (empty || e.fullness != 0) && (full || e.capacity != e.fullness)).ToList();
                }
            }
            catch (Exception e)
            {
                ExceptionLogger.Instance.LogException(e.ToString(), DateTime.UtcNow);
                success = false;
                exceptionString = e.ToString();
                return null;
            }
        }

        public static bool ModifyHangar(string id, int N, out bool success, out string exceptionString)
        {
            try
            {
                using (WarehouseContext db = new WarehouseContext())
                {
                    bool exist, hSuccess;
                    string hExcStr;
                    Hangar hangar = GetHangar(id, out exist, out hSuccess, out hExcStr);
                    if (!hSuccess)
                    {
                        success = false;
                        exceptionString = hExcStr;
                        return false;
                    }
                    success = true;
                    exceptionString = null;
                    if (exist)
                    {
                        int newFullness = hangar.fullness + N;
                        if (newFullness >= 0 && newFullness <= hangar.capacity)
                        {
                            db.hangars.FirstOrDefault(e => e.id == id).fullness = newFullness;
                            if (db.sites.Any(e => e.id == hangar.site_id))
                            {
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
                            }
                            db.SaveChanges();
                            return true;
                        }
                    }
                    return false;
                }
            }
            catch (Exception e)
            {
                ExceptionLogger.Instance.LogException(e.ToString(), DateTime.UtcNow);
                success = false;
                exceptionString = e.ToString();
                return false;
            }
        }
    }
}
