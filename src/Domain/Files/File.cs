using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using DB;

namespace Domain.Files
{

    public class File
    {
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
    }
}
