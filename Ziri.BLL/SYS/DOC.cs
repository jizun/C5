using System;
using System.Web;
using System.Linq;
using System.Drawing;
using System.Transactions;
using Ziri.MDL;
using Ziri.DAL;
using System.IO;

namespace Ziri.BLL.SYS
{
    public class DOC
    {
        public static FileUploadInfo GetFileUploadInfo(long FileID)
        {
            using (var EF = new EF())
            {
                var fileInfo = EF.FileInfos.Where(i => i.ID == FileID).FirstOrDefault();
                if (fileInfo == null) { return null; }
                var fileUploadInfo = new FileUploadInfo
                {
                    FileInfo = fileInfo,
                    FileExtName = EF.FileExtName.Where(i => i.ID == fileInfo.ExtNameID).FirstOrDefault(),
                    FileMIME = EF.FileMIME.Where(i => i.ID == fileInfo.MIMEID).FirstOrDefault(),
                    UploadInfo = EF.UploadInfos.Where(i => i.FileID == fileInfo.ID).FirstOrDefault()
                };
                return fileUploadInfo;
            }
        }
        public static FileUploadInfo GetFileUploadInfo(string FileName, long FileLength)
        {
            using (var EF = new EF())
            {
                var fileInfo = EF.FileInfos.Where(i => i.Name == FileName && i.Length == FileLength).FirstOrDefault();
                if (fileInfo == null) { return null; }
                return GetFileUploadInfo(fileInfo.ID);
            }
        }

        public static FileUploadInfo FileUploadInfoSave(FileUploadInfo FileUploadInfo, out string Message)
        {
            using (var EF = new EF())
            {
                if (FileUploadInfo.UploadInfo.ID == 0)
                {
                    //新增
                    EF.FileInfos.Add(FileUploadInfo.FileInfo);
                    EF.SaveChanges();

                    FileUploadInfo.UploadInfo.FileID = FileUploadInfo.FileInfo.ID;
                    EF.UploadInfos.Add(FileUploadInfo.UploadInfo);
                    EF.SaveChanges();
                    Message = null;
                    return FileUploadInfo;
                }
                //修改
                using (TransactionScope TS = new TransactionScope())
                {
                    var fileInfo = EF.FileInfos.Where(i => i.ID == FileUploadInfo.FileInfo.ID).FirstOrDefault();
                    if (fileInfo == null)
                    {
                        Message = "文件信息编号[" + FileUploadInfo.FileInfo.ID + "]不存在";
                        return null;
                    }
                    fileInfo.GUID = FileUploadInfo.FileInfo.GUID;
                    fileInfo.MIMEID = FileUploadInfo.FileInfo.MIMEID;
                    fileInfo.Name = FileUploadInfo.FileInfo.Name;
                    fileInfo.ExtNameID = FileUploadInfo.FileInfo.ExtNameID;
                    fileInfo.Length = FileUploadInfo.FileInfo.Length;

                    var uploadInfo = EF.UploadInfos.Where(i => i.ID == FileUploadInfo.UploadInfo.ID).FirstOrDefault();
                    if (uploadInfo == null)
                    {
                        Message = "上传记录编号[" + FileUploadInfo.UploadInfo.ID + "]不存在";
                        return null;
                    }
                    uploadInfo.FileID = fileInfo.ID;
                    uploadInfo.UploadTime = FileUploadInfo.UploadInfo.UploadTime;
                    EF.SaveChanges();

                    TS.Complete();
                    Message = null;
                    return new FileUploadInfo
                    {
                        FileInfo = fileInfo,
                        UploadInfo = uploadInfo
                    };
                }
            }
        }

        public static FileUploadInfo Upload(HttpPostedFile httpFile, string SavePath, out string Message)
        {
            Message = null;

            //记录文件信息
            var FileUploadInfo = GetFileUploadInfo(httpFile.FileName, httpFile.ContentLength);
            if (FileUploadInfo != null) { return FileUploadInfo; }

            string FileGUID = Guid.NewGuid().ToString();
            string FileExtName = Path.GetExtension(httpFile.FileName).ToLower();
            string FileMIMEName = Utils.GetMime(FileExtName);
            FileUploadInfo = FileUploadInfoSave(new FileUploadInfo
            {
                FileInfo = new MDL.FileInfo
                {
                    GUID = FileGUID,
                    MIMEID = GetFileMIME(FileMIMEName).ID,
                    Name = httpFile.FileName,
                    ExtNameID = GetFileExtName(FileExtName).ID,
                    Length = httpFile.ContentLength,
                },
                UploadInfo = new UploadInfo
                {
                    UploadTime = DateTime.Now
                }
            }, out Message);
            if (Message != null) { return null; }

            //保存文件
            try { httpFile.SaveAs(SavePath + FileGUID + FileExtName); }
            catch (Exception e) { Message = e.Message; return null; }

            //创建预览
            FileThumbCreate(httpFile, FileGUID, FileMIMEName, FileExtName, SavePath);

            return FileUploadInfo;
        }

        public static void FileThumbCreate(HttpPostedFile httpFile, string FileGUID, string FileMIMEName, string FileExtName, string SavePath)
        {
            if (FileMIMEName.Substring(0, 5) != "image") { return; }

            Image SimpleImage = Image.FromStream(httpFile.InputStream, true);
            double maxWidth = 100;
            double maxHeight = 100;
            double theWidth = SimpleImage.Width;
            double theHeight = SimpleImage.Height;
            if (theWidth > theHeight)
            {
                if (theWidth > maxWidth)
                {
                    theWidth = maxWidth;
                    theHeight = SimpleImage.Height * (theWidth / SimpleImage.Width);
                }
            }
            else
            {
                if (theHeight > maxHeight)
                {
                    theHeight = maxHeight;
                    theWidth = SimpleImage.Width * (theHeight / SimpleImage.Height);
                }
            }
            Image Bitmap = new Bitmap((int)theWidth, (int)theHeight);

            //加文字水印
            Graphics graphics = Graphics.FromImage(Bitmap);
            graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
            graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            graphics.Clear(Color.Transparent); //Color.White
            graphics.DrawImage(SimpleImage, new Rectangle(0, 0, Bitmap.Width, Bitmap.Height), new Rectangle(0, 0, SimpleImage.Width, SimpleImage.Height), GraphicsUnit.Pixel);
            // LogoTextCode
            string tLogoText = "ZIRI";
            Graphics textGraphics = Graphics.FromImage(Bitmap);
            Font font = new Font("Baskerville Old Face", 10);
            textGraphics.DrawString(tLogoText, font, Brushes.White, (float)theWidth - 39, (float)theHeight - 19);
            textGraphics.DrawString(tLogoText, font, Brushes.White, (float)theWidth - 39, (float)theHeight - 21);
            textGraphics.DrawString(tLogoText, font, Brushes.White, (float)theWidth - 41, (float)theHeight - 19);
            textGraphics.DrawString(tLogoText, font, Brushes.White, (float)theWidth - 41, (float)theHeight - 21);
            textGraphics.DrawString(tLogoText, font, Brushes.Black, (float)theWidth - 40, (float)theHeight - 20);
            textGraphics.Dispose();

            //加图片水印
            //Image copyImage = Image.FromFile(HttpContext.Current.Server.MapPath("../Basic/Pictures/ZIRI.png"));
            //Graphics a = Graphics.FromImage(Bitmap);
            //a.DrawImage(copyImage, new Rectangle(Bitmap.Width - copyImage.Width - 5, Bitmap.Height - copyImage.Height - 5, copyImage.Width, copyImage.Height), 0, 0, copyImage.Width, copyImage.Height, GraphicsUnit.Pixel);

            //保存缩略图
            Bitmap.Save(SavePath + "thumb/" + FileGUID + ".png", System.Drawing.Imaging.ImageFormat.Png);
        }

        public static FileMIME GetFileMIME(string MIME)
        {
            using (var EF = new EF())
            {
                FileMIME fm = EF.FileMIME.Where(i => i.Name == MIME).FirstOrDefault();
                if (fm == null)
                {
                    EF.FileMIME.Add(fm = new FileMIME { Name = MIME });
                    EF.SaveChanges();
                }
                return fm;
            }
        }

        public static FileExtName GetFileExtName(long ExtNameID)
        {
            using (var EF = new EF())
            {
                return EF.FileExtName.Where(i => i.ID == ExtNameID).FirstOrDefault();
            }
        }

        public static FileExtName GetFileExtName(string ExtName)
        {
            using (var EF = new EF())
            {
                FileExtName en = EF.FileExtName.Where(i => i.Name == ExtName).FirstOrDefault();
                if (en == null)
                {
                    EF.FileExtName.Add(en = new FileExtName { Name = ExtName });
                    EF.SaveChanges();
                }
                return en;
            }
        }
    }
}
