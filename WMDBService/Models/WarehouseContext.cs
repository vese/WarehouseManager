namespace DataService.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    [DbConfigurationType(typeof(MySql.Data.Entity.MySqlEFConfiguration))]
    public partial class WarehouseContext : DbContext
    {
        public WarehouseContext()
            : base("name=WarehouseContext")
        {
        }

        public virtual DbSet<Hangar> hangars { get; set; }
        public virtual DbSet<Site> sites { get; set; }
    }
}
