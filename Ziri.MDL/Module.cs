using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ziri.MDL
{
    [Table("ModuleInfo")]
    public class ModuleInfo
    {
        [Key]
        public long ID { get; set; }
        [StringLength(32)]
        public string IconFont { get; set; }
        [Required, StringLength(16)]
        public string Name { get; set; }
        [Required, StringLength(16)]
        public string Title { get; set; }
        public bool Enabled { get; set; }
    }

    [Table("ActionInfo")]
    public class ActionInfo
    {
        [Key]
        public long ID { get; set; }
        [StringLength(32)]
        public string IconFont { get; set; }
        [Required, StringLength(16)]
        public string Name { get; set; }
        [Required, StringLength(16)]
        public string Title { get; set; }
        public bool Enabled { get; set; }
    }

    [Table("ModuleAction")]
    public class ModuleAction
    {
        [Key]
        public long ID { get; set; }
        public long ModuleID { get; set; }
        public long ActionID { get; set; }
    }

    public enum ModuleType
    {
        Login = 1,
        Workbench = 2,
        User = 3,
        Notifications = 4,
        Document = 5
    }
}
