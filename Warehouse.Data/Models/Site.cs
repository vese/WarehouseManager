namespace Data.Models
{
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("site")]
    public partial class Site
    {
        public string id { get; set; }

        public bool empty { get; set; }

        public int capacity { get; set; }
    }
}
