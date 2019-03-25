using System.Web;
using System.Collections.Generic;
using Ziri.MDL;
using Newtonsoft.Json;
using System.Linq;

namespace DMS.MOD.SYS
{
    /// <summary>
    /// Upload 的摘要说明
    /// </summary>
    public class Upload : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            var MODName = context.Request["mod"];
            var ACTName = context.Request["act"];
            var FileUploadInfos = new List<FileUploadInfo>();
            var FileUploadErrors = new List<FileUploadError>();
            var urlPath = "/DOC/upload/";   // + MODName + "/"
            var savePath = context.Server.MapPath(urlPath);
            for (int i = 0; i < context.Request.Files.Count; i++)
            {
                var file = context.Request.Files[i];
                if (file.ContentLength == 0) { continue; }
                var FileUploadInfo = Ziri.BLL.SYS.DOC.Upload(file, savePath, out string Message);
                if (Message == null) { FileUploadInfos.Add(FileUploadInfo); }
                else
                {
                    FileUploadErrors.Add(new FileUploadError
                    {
                        FileName = file.FileName,
                        ErrorMessage = Message,
                    });
                }
            }
            context.Response.ContentType = "text/plain";
            switch (ACTName)
            {
                case "sumnmber": context.Response.Write(GetSumnmerData(urlPath, FileUploadInfos, FileUploadErrors)); break;
                case "dropzone": context.Response.Write(GetDropzoneData(urlPath, FileUploadInfos, FileUploadErrors)); break;
                default: break;
            }
        }

        //富文本上传
        private string GetSumnmerData(string urlPath, List<FileUploadInfo> fileUploadInfos, List<FileUploadError> fileUploadErrors)
        {
            var summer_list = new sumnmer_data();
            if (fileUploadErrors.Count == 0)
            {
                foreach (var item in fileUploadInfos)
                {
                    summer_list.state = "succes";
                    summer_list.img_name = item.FileInfo.Name;
                    summer_list.img_url = urlPath + item.FileInfo.GUID + Ziri.BLL.SYS.DOC.GetFileExtName(item.FileInfo.ExtNameID).Name;
                }
            }
            else
            {
                summer_list.state = "error";
            }
            return JsonConvert.SerializeObject(summer_list);
        }

        //拖动上传
        private string GetDropzoneData(string urlPath, List<FileUploadInfo> fileUploadInfos, List<FileUploadError> fileUploadErrors)
        {
            return JsonConvert.SerializeObject(new
            {
                Result = true,
                Count = fileUploadInfos.Count,
                Message = string.Join(",", fileUploadErrors.Select(i => i.ErrorMessage)),
                Files = fileUploadInfos,
            });
        }

        //释放
        public bool IsReusable { get { return false; } }
    }

    public class sumnmer_data
    {
        public string state { get; set; }
        public string img_name { get; set; }
        public string img_url { get; set; }
    }
}