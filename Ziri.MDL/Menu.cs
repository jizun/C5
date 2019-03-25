using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ziri.MDL
{
    [Table("MenuGroup")]
    public class MenuGroup
    {
        [Key]
        public long ID { get; set; }
        public int GroupID { get; set; }
        public long MenuID { get; set; }
    }

    public enum MenuGroupType
    {
        AsideMenu = 1,
        HeadMenu = 2
    }

    public class MenuGroupInfo
    {
        public int GroupID { get; set; }
        public string GroupName { get; set; }
        public string GroupTitle { get; set; }
    }

    [Table("MenuInfo")]
    public class MenuInfo
    {
        [Key]
        public long ID { get; set; }
        public long ParentID { get; set; }
        [StringLength(32)]
        public string IconFont { get; set; }
        [Required, StringLength(16)]
        public string Name { get; set; }
        [Required, StringLength(16)]
        public string Title { get; set; }
        [StringLength(256)]
        public string URL { get; set; }
        public bool Enabled { get; set; }
        public int OrderBy { get; set; }
    }
}
