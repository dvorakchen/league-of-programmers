using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Common;
using DB;

namespace Files
{

    public class File
    {
        /// <summary>
        /// 获取临时文件夹的绝对路径
        /// </summary>
        public static string SaveTemplateAbsoluteDirectory
        {
            get
            {
                var temp = Directory.GetCurrentDirectory() + Config.GetValue("File:Template");
                if (!Directory.Exists(temp))
                    Directory.CreateDirectory(temp);
                return temp;
            }
        }

        /// <summary>
        /// 获取缩略图文件夹的绝对路径
        /// </summary>
        public static string SaveThumbnailAbsoluteDirectory
        {
            get
            {
                var temp = Directory.GetCurrentDirectory() + Config.GetValue("File:SaveThumbnailWebPath");
                if (!Directory.Exists(temp))
                    Directory.CreateDirectory(temp);
                return temp;
            }
        }

        /// <summary>
        /// save file info to DB
        /// </summary>
        /// <param name="fileInfo">file info</param>
        /// <returns>id of file in DB</returns>
        public static async Task<(bool, int)> SaveToDBAsync(string fileName, string saveName, long size)
        {
            await using var db = new LOPDbContext();
            DB.Tables.File newFileModel = new DB.Tables.File
            {
                Name = fileName,
                SaveName = saveName,
                Extension = Path.GetExtension(fileName),
                Size = size
            };

            await db.AddAsync(newFileModel);
            bool success = await db.SaveChangesAsync() == 1;

            return (success, success ? newFileModel.Id : -1);
        }

        /// <summary>
        /// delete file
        /// </summary>
        /// <param name="saveFileName"></param>
        public static void Delete(string saveFileName)
        {
            string saveWebPath = Config.GetValue("File:SaveWebPath");
            var currentDirectory = Directory.GetCurrentDirectory();
            string fullFileName = Path.Combine(currentDirectory + saveWebPath, saveFileName);
            if (System.IO.File.Exists(fullFileName))
                System.IO.File.Delete(fullFileName);
        }

        /// <summary>
        /// 获取缩略图
        /// </summary>
        /// <returns>缩略图保存文件名</returns>
        public static string GetThumbnail(string filePath)
        {
            Compress fileCompress = new Compress();

            var extension = Path.GetExtension(filePath);
            string thumbnailName = extension.ToLower() switch
            {
                ".gif" => fileCompress.GetGIFThumbnail(filePath),
                _ => fileCompress.MakeThumbnail(filePath)
            };
            return thumbnailName;
        }
    }
}
