namespace Data.Models
{
    using System.Data.Entity;

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
