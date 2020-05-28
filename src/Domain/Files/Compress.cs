using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Files
{
    /// <summary>
    /// 文件压缩，缩略图
    /// </summary>
    class Compress
    {
        /// <summary>
        /// 缩略图最宽
        /// </summary>
        internal const double THUMBNAIL_WIDTH = 200;
        /// <summary>
        /// 缩略图最高
        /// </summary>
        internal const double THUMBNAIL_HEIGHT = 150;
        /// <summary>
        /// 缩略图压缩大小，字节
        /// </summary>
        internal const double THUMBNAIL_SIZE = 64;

        /// <summary>
        /// 获取缩略图
        /// </summary>
        /// <param name="sourcePath">源文件</param>
        /// <param name="fileSavePath">缩略图保存路径</param>
        /// <param name="thumbnailWidth">缩略图宽</param>
        /// <param name="thumbnailHeight">缩略图高</param>
        /// <returns>缩略图名</returns>
        internal string MakeThumbnail(string sourcePath, double thumbnailWidth = THUMBNAIL_WIDTH, double thumbnailHeight = THUMBNAIL_HEIGHT)
        {
            /*
             *  如果是 GIF ，则截取封面做缩略图
             */

            //从文件取得图片对象，并使用流中嵌入的颜色管理信息
            using Image sourceImage = Image.FromFile(sourcePath, true);
            //缩略图宽、高
            double newWidth = sourceImage.Width, newHeight = sourceImage.Height;
            //宽大于模版的横图
            if (sourceImage.Width > sourceImage.Height || sourceImage.Width == sourceImage.Height)
            {
                if (sourceImage.Width > thumbnailWidth)
                {
                    //宽按模版，高按比例缩放
                    newWidth = thumbnailWidth;
                    newHeight = sourceImage.Height * (newWidth / sourceImage.Width);
                }
            }
            //高大于模版的竖图
            else
            {
                if (sourceImage.Height > thumbnailHeight)
                {
                    //高按模版，宽按比例缩放
                    newHeight = thumbnailHeight;
                    newWidth = sourceImage.Width * (newHeight / sourceImage.Height);
                }
            }
            //取得图片大小
            Size mySize = new Size((int)newWidth, (int)newHeight);
            //新建一个bmp图片
            using Image bitmap = new Bitmap(mySize.Width, mySize.Height);
            //新建一个画板
            using Graphics g = Graphics.FromImage(bitmap);
            //设置高质量插值法
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
            //设置高质量,低速度呈现平滑程度
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            //清空一下画布
            g.Clear(Color.White);
            //在指定位置画图
            g.DrawImage(sourceImage, new Rectangle(0, 0, bitmap.Width, bitmap.Height),
            new Rectangle(0, 0, sourceImage.Width, sourceImage.Height),
            GraphicsUnit.Pixel);

            //保存缩略图
            var thumbnailName = Path.GetFileNameWithoutExtension(Path.GetTempFileName()) + "-thumbnail" + Path.GetExtension(sourcePath);
            var thumbnailPath = Path.Combine(File.SaveThumbnailAbsoluteDirectory, thumbnailName);
            bitmap.Save(thumbnailPath);

            //  如果压缩后的图片比原图大，就将原图作为缩略图保存
            FileInfo sourceInfo = new FileInfo(sourcePath);
            FileInfo thumbnailInfo = new FileInfo(thumbnailPath);
            if (thumbnailInfo.Length > sourceInfo.Length)
            {
                System.IO.File.Delete(thumbnailPath);
                sourceInfo.CopyTo(thumbnailPath);
            }

            return thumbnailName;
        }

        /// <summary>
        /// 获取 GIF 文件的缩略图
        /// </summary>
        /// <param name="sourceFile"></param>
        /// <returns>缩略图名</returns>
        internal string GetGIFThumbnail(string sourceFile)
        {
            if (Path.GetExtension(sourceFile).ToLower() != ".gif")
                throw new FormatException("不是 GIF 文件");

            //  截取封面，保存到临时文件
            var tempName = Path.GetFileNameWithoutExtension(Path.GetTempFileName()) + ".gif";
            var templatePath = Path.Combine(File.SaveTemplateAbsoluteDirectory, tempName);
            string thumbnailName;
            try
            {
                GetGIFCover(sourceFile, templatePath);
                //  保存封面缩略图
                thumbnailName = MakeThumbnail(templatePath);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                //  删除临时文件
                if (System.IO.File.Exists(templatePath))
                    System.IO.File.Delete(templatePath);
            }
            return thumbnailName;
        }

        /// <summary>
        /// 获取 GIF 图的封面
        /// </summary>
        /// <param name="sourceFileFullPath"></param>
        /// <param name="saveFullPath"></param>
        internal void GetGIFCover(string sourceFile, string saveFullPath)
        {
            using Image gif = Image.FromFile(sourceFile, true);
            FrameDimension ImgFrmDim = new FrameDimension(gif.FrameDimensionsList[0]);
            gif.SelectActiveFrame(ImgFrmDim, 0);
            gif.Save(saveFullPath, ImageFormat.Png);
        }
    }
}
