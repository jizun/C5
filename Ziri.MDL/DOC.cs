using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ziri.MDL
{
    [Table("FileInfo")]
    public class FileInfo
    {
        [Key]
        public long ID { get; set; }
        public string GUID { get; set; }
        public int MIMEID { get; set; }
        public string Name { get; set; }
        public int ExtNameID { get; set; }
        public long Length { get; set; }
    }

    [Table("UploadInfo")]
    public class UploadInfo
    {
        [Key]
        public long ID { get; set; }
        public long FileID { get; set; }
        public DateTime UploadTime { get; set; }
    }

    public class FileUploadInfo
    {
        public FileInfo FileInfo { get; set; }
        public FileExtName FileExtName { get; set; }
        public FileMIME FileMIME { get; set; }
        public UploadInfo UploadInfo { get; set; }
    }

    public class FileUploadError
    {
        public string FileName { get; set; }
        public string ErrorMessage { get; set; }
    }
}