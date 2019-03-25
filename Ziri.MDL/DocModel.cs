using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ziri.MDL
{
    [Table("Doc")]
    public class Doc
    {
        [Key]
        public long ID { get; set; }
        public string Name { get; set; }
    }

    [Table("DocPath")]
    public class DocPath
    {
        [Key]
        public long ID { get; set; }
        public long DocID { get; set; }
        public string VirtualPath { get; set; }
        public string RealityPath { get; set; }
    }

    [Table("DocFolder")]
    public class DocFolder
    {
        [Key]
        public long ID { get; set; }
        [Required]
        public string Name { get; set; }
        public DateTime CreationTime { get; set; }
        public DateTime LastAccessTime { get; set; }
    }

    [Table("DocFile")]
    public class DocFile
    {
        [Key]
        public long ID { get; set; }
        [Required]
        public long DocPathID { get; set; }
        [Required]
        public long DocFolderID { get; set; }
        [Required]
        public string Name { get; set; }
        public long ExtNameID { get; set; }
        [Required]
        public long Length { get; set; }
        [Required]
        public long MIMEID { get; set; }
        public DateTime CreationTime { get; set; }
        public DateTime LastAccessTime { get; set; }
    }

    [Table("FileExtName")]
    public class FileExtName
    {
        [Key]
        public int ID { get; set; }
        public string Name { get; set; }
    }

    [Table("FileMIME")]
    public class FileMIME
    {
        [Key]
        public int ID { get; set; }
        [Required]
        public string Name { get; set; }
    }

    public class DocFileInfo
    {
        public long OrderByID { set; get; }
        public long FileID { get; set; }
        public string RealityPath { get; set; }
        public string VirtualPath { get; set; }
        public string FolderName { get; set; }
        public string FileName { get; set; }
        public string FileExtName { get; set; }
        public DateTime LastAccessTime { get; set; }
        private static long idx { get; set; } = 0;
        public DocFileInfo()
        {
            OrderByID = idx++;
            FileID = FileID;
            RealityPath = RealityPath;
            VirtualPath = VirtualPath;
            FolderName = FolderName;
            FileName = FileName;
            FileExtName = FileExtName;
            LastAccessTime = LastAccessTime;
        }
    }

    public class DocFileList
    {
        public long FileID { get; set; }
        public string FilePath { get; set; }
        public string FileName { get; set; }
        public string FileExtName { get; set; }
        public DateTime LastAccessTime { get; set; }
    }

    public class DocPathList
    {
        public long PathID { get; set; }
        public string VirtualPath { get; set; }
        public int? FileCount { get; set; }
    }
}
