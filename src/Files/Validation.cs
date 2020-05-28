using Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Files
{
    /// <summary>
    /// 文件验证
    /// </summary>
    public class Validation
    {
        private static readonly object lockObj = new object();

        private static HashSet<string> _allowFileExtension;

        private static readonly char[] FileSeparatorChars = new char[1] { ',' };

        /// <summary>
        /// validate file extension is validation
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static bool ValidateExtension(string fileName)
        {
            if (_allowFileExtension is null)
            {
                lock (lockObj)
                {
                    if (_allowFileExtension is null)
                    {
                        string[] allowFileExtension = Config.GetValue("File:AllowFileExtension")
                                                            .Split(FileSeparatorChars, StringSplitOptions.RemoveEmptyEntries);
                        _allowFileExtension = new HashSet<string>(allowFileExtension);
                    }
                }
            }

            return _allowFileExtension.Contains(Path.GetExtension(fileName));
        }

        private static HashSet<string> _allowAvatarExtension;
        public static bool ValidateAvatarExtension(string fileName)
        {
            if (_allowAvatarExtension is null)
            {
                lock (lockObj)
                {
                    if (_allowAvatarExtension is null)
                    {
                        string[] allowAvatarExtension = Config.GetValue("File:AllowAvatarExtension")
                                                              .Split(FileSeparatorChars, StringSplitOptions.RemoveEmptyEntries);
                        _allowAvatarExtension = new HashSet<string>(allowAvatarExtension);
                    }
                }
            }

            return _allowAvatarExtension.Contains(Path.GetExtension(fileName));
        }

        /// <summary>
        /// 验证文件签名是否合法
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static bool SignatureValidation(FileStream file)
        {
            if (file is null)
                throw new FileNotFoundException();

            using var reader = new BinaryReader(file);
            var signtures = FileSigntures.Signtures[Path.GetExtension(file.Name).ToLower()];
            var headerBytes = reader.ReadBytes(signtures.Max(m => m.Length));

            return signtures.Any(s => headerBytes.Take(s.Length).SequenceEqual(s));
        }
    }
}
