using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ziri.MDL
{
    [Table("KindInfo")]
    public class KindInfo
    {
        [Key]
        public long ID { get; set; }
        public long ParentID { get; set; }
        public long LogoFileID { get; set; }
        [Required, StringLength(32)]
        public string Name { get; set; }
        [Required, StringLength(32)]
        public string Title { get; set; }
        public int OrderBy { get; set; }
        public bool Enabled { get; set; }
    }
}
