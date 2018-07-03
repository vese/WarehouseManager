namespace WMDBService.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("hangar")]
    public partial class hangar
    {
        [StringLength(10)]
        public string id { get; set; }

        [Required]
        [StringLength(10)]
        public string site_id { get; set; }

        public int capacity { get; set; }

        public int fullness { get; set; }

        public virtual site site { get; set; }
    }
}
