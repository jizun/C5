using System;
using System.IO;
using System.Web;
using System.Linq;
using System.Drawing;
using System.Configuration;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Runtime.Remoting.Messaging;
using Ziri.DAL;
using Ziri.MDL;

namespace Ziri.BLL
{
    public class DocManager
    {
        public string DocRead(long PathID)
        {
            using (var EF = new EF())
            {
                var DocPath = EF.DocPath.Where(i => i.ID == PathID).FirstOrDefault();
                return FolderRead(DocPath.ID, DocPath.RealityPath, null);
            }
        }


        public List<Doc> GetDocList()
        {
            using (var EF = new EF())
            {
                return EF.Doc.ToList();
            }
        }

        public Doc GetDoc(long DocID)
        {
            using (var EF = new EF())
            {
                return EF.Doc.Where(i => i.ID == DocID).FirstOrDefault();
            }
        }

        public List<DocPathList> GetDocPathList(long DocID)
        {
            using (var EF = new EF())
            {
                return (from docPath in EF.DocPath
                        join fileCount in (
                            from docFile in EF.DocFile
                            group docFile by docFile.DocPathID into g
                            select new { PathID = g.Key, FileCount = g.Count() }
                        ) on docPath.ID equals fileCount.PathID into t1
                        from fileCount in t1.DefaultIfEmpty()
                        where docPath.DocID == DocID
                        select new DocPathList
                        {
                            PathID = docPath.ID,
                            VirtualPath = docPath.VirtualPath,
                            FileCount = fileCount.FileCount
                        }
                           ).ToList();
            }
        }

        public DocFileInfo GetDocFile(long FileID, string ReadAction = null, List<string> FilterExtNames = null, string OrderByLastAccessTime = null)
        {
            using (var EF = new EF())
            {
                var docFiles = (from docFile in EF.DocFile
                                join docFolder in EF.DocFolder on docFile.DocFolderID equals docFolder.ID into t1
                                from docFolder in t1.DefaultIfEmpty()
                                join docPath in EF.DocPath on docFile.DocPathID equals docPath.ID into t2
                                from docPath in t2.DefaultIfEmpty()
                                join fileExtName in EF.FileExtName on docFile.ExtNameID equals fileExtName.ID into t3
                                from fileExtName in t3.DefaultIfEmpty()
                                select new DocFileInfo
                                {
                                    FileID = docFile.ID,
                                    RealityPath = docPath.RealityPath,
                                    VirtualPath = docPath.VirtualPath,
                                    FolderName = docFolder.Name,
                                    FileName = docFile.Name,
                                    FileExtName = fileExtName.Name,
                                    LastAccessTime = docFile.LastAccessTime
                                });

                //当前项
                if (!new string[] { "Previous", "Next" }.Contains(ReadAction)) { return docFiles.Where(i => i.FileID == FileID).FirstOrDefault(); }
                //筛选
                if (FilterExtNames != null && FilterExtNames.Count > 0)
                {
                    docFiles = docFiles.Where(i => FilterExtNames.Contains(i.FileExtName));
                }
                //排序
                if (OrderByLastAccessTime == "asc") { docFiles = docFiles.OrderBy(i => i.FileID).OrderBy(i => i.LastAccessTime); }
                else if (OrderByLastAccessTime == "desc") { docFiles = docFiles.OrderBy(i => i.FileID).OrderByDescending(i => i.LastAccessTime); }
                //前后项
                var list = docFiles.ToList();
                DocFileInfo current = list.Where(i => i.FileID == FileID).FirstOrDefault();
                if (ReadAction == "Next") { return list.Where(i => i.OrderByID > current.OrderByID).FirstOrDefault(); }
                else { return list.Where(i => i.OrderByID < current.OrderByID).OrderByDescending(i => i.OrderByID).FirstOrDefault(); }
            }
        }

        public long GetFileCount(long DocID)
        {
            using (var EF = new EF())
            {
                return (from docFile in EF.DocFile
                        join docPath in EF.DocPath on docFile.DocPathID equals docPath.ID
                        join doc in EF.Doc on docPath.DocID equals doc.ID
                        where doc.ID == DocID
                        select docFile).Count();
            }
        }

        public object GetFileExtNameList(long DocID)
        {
            using (var EF = new EF())
            {
                var a = (from docFile in EF.DocFile
                         join docPath in EF.DocPath on docFile.DocPathID equals docPath.ID into t1
                         from docPath in t1.DefaultIfEmpty()
                         join fileExtName in EF.FileExtName on docFile.ExtNameID equals fileExtName.ID into t2
                         from fileExtName in t2.DefaultIfEmpty()
                         where docPath.DocID == DocID
                         group new { fileExtName.ID, fileExtName.Name } by new { fileExtName.ID, fileExtName.Name } into g
                         select new
                         {
                             g.Key.ID,
                             g.Key.Name
                         }
                        ).ToList();
                return a;
            }
        }

        /// <summary>
        /// 文档列表
        /// </summary>
        /// <param name="DocID">文档编号</param>
        /// <param name="FilterField">筛选字段</param>
        /// <param name="OrderField">排序字段</param>
        /// <param name="PageSize">页面大小</param>
        /// <param name="PageIndex">页面起始项</param>
        /// <param name="TotalRowCount">总行数</param>
        /// <param name="PageCount">页数</param>
        /// <param name="pageIndex">页面起始项校正</param>
        /// <returns></returns>
        public List<DocFileList> GetFileList(long DocID, List<ListFilterField> FilterField, List<ListOrderField> OrderField
            , int PageSize, int PageIndex, out long TotalRowCount, out int PageCount, out int pageIndex)
        {
            using (var EF = new EF())
            {
                //列表
                var DataList = from docPath in EF.DocPath
                               join docFile in EF.DocFile on docPath.ID equals docFile.DocPathID
                               join docFolder in EF.DocFolder on docFile.DocFolderID equals docFolder.ID into t1
                               from docFolder in t1.DefaultIfEmpty()
                               join fileExtName in EF.FileExtName on docFile.ExtNameID equals fileExtName.ID into t2
                               from fileExtName in t2.DefaultIfEmpty()
                               where docPath.DocID == DocID
                               select new DocFileList
                               {
                                   FileID = docFile.ID,
                                   FilePath = docPath.VirtualPath + docFolder.Name + docFile.Name,
                                   FileName = docFile.Name,
                                   FileExtName = fileExtName.Name,
                                   LastAccessTime = docFile.LastAccessTime
                               };

                //筛选
                foreach (var item in FilterField)
                {
                    if (item.Name == "FileExtName" && item.Value.Count > 0) { DataList = DataList.Where(i => item.Value.Contains(i.FileExtName)); }
                }

                //排序
                if (OrderField.Count == 0) { DataList = DataList.OrderBy(i => i.FileID); }
                else
                {
                    foreach (var item in OrderField)
                    {
                        if (item.Name == "LastAccessTime")
                        {
                            if (item.Mode == OrderByMode.Asc) { DataList = DataList.OrderBy(i => i.FileID).OrderBy(i => i.LastAccessTime); }
                            else { DataList = DataList.OrderBy(i => i.FileID).OrderByDescending(i => i.LastAccessTime); }

                        }
                    }
                }

                //分页
                TotalRowCount = DataList.Count();
                PageCount = (int)Math.Ceiling((double)TotalRowCount / PageSize);
                if (PageIndex > PageCount) { PageIndex = PageCount; }
                pageIndex = PageIndex;
                if (TotalRowCount == 0) { return new List<DocFileList>(); }
                return DataList.Skip((PageIndex - 1) * PageSize).Take(PageSize).ToList();
            }
        }

        private string FolderRead(long DocPathID, string RealityPath, string FolderName)
        {
            var DirInfo = new DirectoryInfo(RealityPath);
            var EF = new EF();

            //获取当前目录名记录
            DocFolder DocFolder = null;
            if (FolderName == null)
            {
                //根目录
                DocFolder = new DocFolder { ID = 0 };
            }
            else
            {
                DocFolder = EF.DocFolder.Where(i => i.Name == FolderName).FirstOrDefault();
                if (DocFolder == null)
                {
                    EF.DocFolder.Add(DocFolder = new DocFolder
                    {
                        Name = FolderName,
                        CreationTime = DirInfo.CreationTime,
                        LastAccessTime = DirInfo.LastAccessTime
                    });
                    EF.SaveChanges();
                }
                else if (DocFolder.LastAccessTime == DirInfo.LastAccessTime)
                {
                    //记录已经存在并且未变更，无需处理
                    return null;
                }
            }

            //历遍子目录
            foreach (DirectoryInfo Folder in DirInfo.GetDirectories())
            {
                FolderRead(DocPathID, Folder.FullName, FolderName + Folder.Name + "/");
            }

            //历遍文件
            foreach (var iFile in DirInfo.GetFiles())
            {
                var DocFile = EF.DocFile.Where(i => i.DocFolderID == DocFolder.ID && i.Name == iFile.Name).FirstOrDefault();
                if (DocFile == null)
                {
                    EF.DocFile.Add(DocFile = new DocFile
                    {
                        DocPathID = DocPathID,
                        DocFolderID = DocFolder.ID,
                        Name = iFile.Name,
                        ExtNameID = SYS.DOC.GetFileExtName(iFile.Extension).ID,
                        Length = iFile.Length,
                        MIMEID = SYS.DOC.GetFileMIME(Utils.GetMime(Path.GetExtension(iFile.FullName).ToLower())).ID,
                        CreationTime = iFile.CreationTime,
                        LastAccessTime = iFile.LastWriteTime
                    });
                    try { EF.SaveChanges(); }
                    catch (DbEntityValidationException ex)
                    {
                        string error = "";
                        foreach (var item in ex.EntityValidationErrors)
                        {
                            foreach (var item2 in item.ValidationErrors)
                            { error += string.Format("{0}:{1}\r\n", item2.PropertyName, item2.ErrorMessage); }
                        }
                        throw new Exception(error);
                    }
                    catch (Exception) { throw; }

                }
                else if (DocFile.LastAccessTime == iFile.LastAccessTime)
                {
                    //记录已经存在并且未变更，无需处理
                    break;
                }

                //异步创建预览
                FileSimpleHandler tFileSimpleHandler = new FileSimpleHandler(FileSimpleCreate);
                IAsyncResult asyncResult = tFileSimpleHandler.BeginInvoke(DocFile.ID, new AsyncCallback(FileSimpleCallback), DocFile);
                new LogManager().WriteLog("文件[" + DocFile.ID + "]创建预览开始。");
            }

            EF.Dispose();
            return null;
        }

        #region 创建文件预览
        private delegate string FileSimpleHandler(long FileID);
        private static void FileSimpleCallback(IAsyncResult asyncResult)
        {
            DocFile DocFile = new DocFile();
            try
            {
                FileSimpleHandler tFileSimpleHandler = (FileSimpleHandler)((AsyncResult)asyncResult).AsyncDelegate;
                string tFileSimpleCreateResult = tFileSimpleHandler.EndInvoke(asyncResult);
                DocFile = (DocFile)asyncResult.AsyncState;
                new LogManager().WriteLog(tFileSimpleCreateResult);
            }
            catch (Exception e) { new LogManager().WriteLog("文件[" + DocFile.ID + "]回调错误：" + e.Message); }

        }
        public string FileSimpleCreate(long FileID)
        {
            DocFileInfo docFileInfo = GetDocFile(FileID);
            string FileRealityName = docFileInfo.RealityPath + docFileInfo.FolderName + docFileInfo.FileName;
            string FilePreviewName = ConfigurationManager.AppSettings["MediaPath"] + docFileInfo.VirtualPath + docFileInfo.FolderName;
            Utils.CreateDirectory(FilePreviewName);
            FilePreviewName += docFileInfo.FileName + ".jpg";

            //创建预览
            double SimpleWidthMax = 198;
            double SimpleHeightMax = 198;
            double SimpleWidth;
            double SimpleHeight;
            if (".FLV.HEVC.MP4.AVI.MPEG.RMVB.WMV".Contains(docFileInfo.FileExtName.ToUpper()))
            {
                string ffmpeg = HttpContext.Current.Server.MapPath("/Bin/ffmpeg.exe");
                string FlvImgSize = SimpleWidthMax + "*" + SimpleHeightMax;
                System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo(ffmpeg)
                {
                    WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden,
                    Arguments = " -i " + FileRealityName + "  -y -f image2 -t 0.1 -vf scale=" + SimpleWidthMax + ":" + SimpleWidthMax + "/a " + FilePreviewName
                };
                System.Diagnostics.Process.Start(startInfo);
                
            }
            else if (".BMP.GIF.JPG.JPEG.EXIF.PNG.TIFF".Contains(docFileInfo.FileExtName.ToUpper()))
            {
                Image SourceImage = Image.FromFile(FileRealityName, true);
                if (SourceImage.Width > SourceImage.Height)
                {
                    SimpleWidth = SimpleWidthMax;
                    SimpleHeight = SourceImage.Height * (SimpleWidth / SourceImage.Width);
                }
                else
                {
                    SimpleHeight = SimpleHeightMax;
                    SimpleWidth = SourceImage.Width * (SimpleHeight / SourceImage.Height);
                }
                if (SimpleWidth < 50 || SimpleHeight < 50)
                {
                    SimpleWidth += 50;
                    SimpleHeight += 50;
                }
                using (Image SimpleImage = new Bitmap((int)SimpleWidth, (int)SimpleHeight))
                using (Graphics SimpleGraphics = Graphics.FromImage(SimpleImage))
                {
                    SimpleGraphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
                    SimpleGraphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                    SimpleGraphics.DrawImage(SourceImage
                        , new Rectangle(0, 0, SimpleImage.Width, SimpleImage.Height)
                        , new Rectangle(0, 0, SourceImage.Width, SourceImage.Height)
                        , GraphicsUnit.Pixel);
                    SimpleImage.Save(FilePreviewName, System.Drawing.Imaging.ImageFormat.Jpeg);
                }
                SourceImage.Dispose();
            }
            return "文件[" + FileID + "]创建预览回调完成。";
        }
        #endregion

    }
}
