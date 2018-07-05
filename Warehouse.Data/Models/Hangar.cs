namespace Data.Models
{
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("hangar")]
    public partial class Hangar
    {
        public string id { get; set; }

        public string site_id { get; set; }

        public int capacity { get; set; }

        public int fullness { get; set; }
    }
}
