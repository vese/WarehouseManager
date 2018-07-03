﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using WMDBService.Models;

namespace WMDBService
{

    public class New_table
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    [DbConfigurationType(typeof(MySql.Data.Entity.MySqlEFConfiguration))]
    public class ntContext : DbContext
    {
        public ntContext() : base("conn")
        { }

        public DbSet<New_table> new_tables { get; set; }
    }

    public class Service1 : IWMDBService
    {
        WarehouseContext db = new WarehouseContext();

        public List<Site> GetAllSites()
        {
            try
            {
                ntContext nt = new ntContext();
                nt.new_tables.ToList().ForEach(e => { });
            }
            catch (Exception e)
            {
                var t = "t";
            }

            List<Site> sites = new List<Site>();
            try
            {
                db.sites.ToList().ForEach(e => sites.Add(new Site()
                {
                    Id = e.id,
                    Capacity = e.capacity,
                    Empty = e.empty
                }));
            }
            catch (Exception e)
            {
                var t = "t";
            }
            return sites;
        }

        public List<Site> GetSites(bool empty, bool full)
        {
            List<Site> sites = new List<Site>();
            db.sites.Where(e => (empty || !e.empty) && (full || e.capacity != 0)).ToList().ForEach(e => sites.Add(new Site()
            {
                Id = e.id,
                Capacity = e.capacity,
                Empty = e.empty
            }));
            return sites;
        }

        public bool HangarExists(string id)
        {
            foreach (var hangar in db.hangars)
            {
                if (hangar.id == id)
                {
                    return true;
                }
            }
            return false;
        }

        public Hangar GetHangar(string hangarId)
        {
            Hangar hangar = new Hangar();
            db.hangars.Where(e => e.id == hangarId).ToList().ForEach(e =>
            {
                hangar.Id = e.id;
                hangar.SiteId = e.site_id;
                hangar.Capacity = e.capacity;
                hangar.Fullness = e.fullness;
            });
            return hangar;
        }

        public List<Hangar> GetAllHangars(string siteId)
        {
            List<Hangar> hangars = new List<Hangar>();
            db.hangars.Where(e => e.site_id == siteId).ToList().ForEach(e => hangars.Add(new Hangar()
            {
                Id = e.id,
                SiteId = e.site_id,
                Capacity = e.capacity,
                Fullness = e.fullness
            }));
            return hangars;
        }

        public List<Hangar> GetHangars(string siteId, bool empty, bool full)
        {
            List<Hangar> hangars = new List<Hangar>();
            db.hangars.Where(e => e.site_id == siteId && (empty || e.fullness != 0) && (full || e.capacity != e.fullness)).ToList().ForEach(e => hangars.Add(new Hangar()
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
            int places = 0;
            db.sites.ToList().ForEach(e => places += e.capacity);
            return places;
        }

        public bool ModifyHangar(string id, int N)
        {
            if (N != 0)
            {
                foreach (var hangar in db.hangars.ToList())
                {
                    if (hangar.id == id)
                    {
                        int newFullness = hangar.fullness + N;
                        if (newFullness >= 0 && newFullness <= hangar.capacity)
                        {
                            hangar.fullness = newFullness;
                            db.sites.Where(e => e.id == hangar.site_id).ToList().FirstOrDefault().capacity -= N;
                            if (newFullness == 0)
                            {
                                bool empty = true;
                                foreach (var h in db.hangars.ToList())
                                {
                                    if (h.site_id == hangar.site_id && h.fullness > 0)
                                    {
                                        empty = false;
                                        break;
                                    }
                                }
                                if (empty)
                                {
                                    db.sites.Where(e => e.id == hangar.site_id).FirstOrDefault().empty = true;
                                }
                            }
                            else if (db.sites.Where(e => e.id == hangar.site_id).FirstOrDefault().empty)
                            {
                                db.sites.Where(e => e.id == hangar.site_id).FirstOrDefault().empty = false;
                            }
                            db.SaveChanges();
                            return true;
                        }
                        else
                        {
                            break;
                        }
                    }
                }
            }
            return false;
        }
    }
}
