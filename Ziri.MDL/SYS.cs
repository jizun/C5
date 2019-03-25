using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ziri.MDL
{
    public class AlertMessage
    {
        public AlertType Type { get; set; }
        public string Message { get; set; }
    }

    public enum AlertType
    {
        warning,
        error,
        success,
        info,
        question
    }

    public class ListDict
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
    }

    public class PagerCount
    {
        public long ItemCount { get; set; }
        public int PageSize { get; set; }
        public int PageCount { get; set; }
        public int PageIndex { get; set; }
    }

    [Table("Notifications")]
    public class Notifications
    {
        [Key]
        public long ID { get; set; }
        public int ModuleTypeID { get; set; }
        [StringLength(256)]
        public string Description { get; set; }
        [StringLength(256)]
        public string ProcessURL { get; set; }
        public DateTime UpdateTime { get; set; }
        public DateTime? HaveReadTime { get; set; }
    }

    public class WorkbenchNotifications
    {
        public long ID { get; set; }
        public int ModuleTypeID { get; set; }
        public string ModuleTypeIcon { get; set; }
        public string Description { get; set; }
        public string ProcessURL { get; set; }
        public DateTime UpdateTime { get; set; }
        public DateTime? HaveReadTime { get; set; }
    }
}
