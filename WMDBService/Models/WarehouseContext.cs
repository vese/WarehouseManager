namespace WMDBService.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class WarehouseContext : DbContext
    {
        public WarehouseContext()
            : base("name=WarehouseContext")
        {
        }

        public virtual DbSet<hangar> hangars { get; set; }
        public virtual DbSet<site> sites { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<hangar>()
                .Property(e => e.id)
                .IsUnicode(false);

            modelBuilder.Entity<hangar>()
                .Property(e => e.site_id)
                .IsUnicode(false);

            modelBuilder.Entity<site>()
                .Property(e => e.id)
                .IsUnicode(false);

            modelBuilder.Entity<site>()
                .HasMany(e => e.hangars)
                .WithRequired(e => e.site)
                .HasForeignKey(e => e.site_id)
                .WillCascadeOnDelete(false);
        }
    }
}
