namespace WMDBService.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("hangar")]
    public partial class Hangar
    {
        public string id { get; set; }

        public string site_id { get; set; }

        public int capacity { get; set; }

        public int fullness { get; set; }
    }
}
