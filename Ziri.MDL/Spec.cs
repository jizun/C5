using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ziri.MDL
{
    [Table("SpecInfo")]
    public class SpecInfo
    {
        [Key]
        public long ID { get; set; }
        [StringLength(32)]
        public string IconFont { get; set; }
        [StringLength(32)]
        public string Name { get; set; }
        [StringLength(32)]
        public string Title { get; set; }
        public bool Enabled { get; set; }
    }

    [Table("SpecValue")]
    public class SpecValue
    {
        [Key]
        public long ID { get; set; }
        public long SpecID { get; set; }
        [StringLength(32)]
        public string Value { get; set; }
        public bool Enabled { get; set; }
    }

    public class SpecValues {
        public SpecInfo SpecInfo { get; set; }
        public List<SpecValue> Values { get; set; }
    }
}
