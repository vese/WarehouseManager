namespace DataService.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("site")]
    public partial class Site
    {
        public string id { get; set; }

        public bool empty { get; set; }

        public int capacity { get; set; }
    }
}
