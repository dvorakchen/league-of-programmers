using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Files
{
    /// <summary>
    /// use this to get the file signture
    /// </summary>
    internal class FileSigntures
    {
        public static readonly Dictionary<string, List<byte[]>> Signtures = new Dictionary<string, List<byte[]>>
        {
            {
                ".jpeg", new List<byte[]>
                {
                    new byte[] { 0xFF, 0xD8, 0xFF, 0xE0 },
                    new byte[] { 0xFF, 0xD8, 0xFF, 0xE2 },
                    new byte[] { 0xFF, 0xD8, 0xFF, 0xE3 },
                }
            },
            {
                ".jpg", new List<byte[]>
                {
                    new byte[] { 0xFF, 0xD8, 0xFF, 0xE0 },
                    new byte[] { 0xFF, 0xD8, 0xFF, 0xE1 },
                    new byte[] { 0xFF, 0xD8, 0xFF, 0xE8 },
                }
            },
            {
                ".png", new List<byte[]>
                {
                    new byte[] { 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A }
                }
            },
            {
                ".bmp", new List<byte[]>
                {
                    new byte[] { 0x42, 0x4D }
                }
            },
            {
                ".gif", new List<byte[]>
                {
                    new byte[] { 0x47, 0x49, 0x46, 0x38 }
                }
            },
            {
                ".doc", new List<byte[]>
                {
                    new byte[] { 0xD0, 0xCF, 0x11, 0xE0, 0xA1, 0xB1, 0x1A , 0xE1 },
                    new byte[] { 0x0D, 0x44, 0x4F , 0x43 },
                    new byte[] { 0xCF, 0x11, 0xE0, 0xA1, 0xB1, 0x1A, 0xE1 , 0x00 },
                    new byte[] { 0xDB, 0xA5, 0x2D , 0x00 },
                    new byte[] { 0xEC, 0xA5, 0xC1 , 0x00 },
                }
            },
            {
                ".docx", new List<byte[]>
                {
                    new byte[] { 0x50, 0x4B, 0x03, 0x04 },
                    new byte[] { 0x50, 0x4B, 0x03, 0x04, 0x14, 0x00, 0x06 , 0x00 },
                }
            },
            {
                ".xls", new List<byte[]>
                {
                    new byte[] { 0xD0, 0xCF, 0x11, 0xE0, 0xA1, 0xB1, 0x1A , 0xE1 },
                    new byte[] { 0x09, 0x08, 0x10, 0x00, 0x00, 0x06, 0x05 , 0x00 },
                    new byte[] { 0xFD, 0xFF, 0xFF, 0xFF , 0x10 },
                    new byte[] { 0xFD, 0xFF, 0xFF, 0xFF , 0x1F },
                    new byte[] { 0xFD, 0xFF, 0xFF, 0xFF , 0x22 },
                    new byte[] { 0xFD , 0xFF, 0xFF, 0xFF , 0x23 },
                    new byte[] { 0xFD , 0xFF , 0xFF, 0xFF , 0x28 },
                    new byte[] { 0xFD, 0xFF, 0xFF, 0xFF , 0x29 },
                }
            },
            {
                ".xlsx", new List<byte[]>
                {
                    new byte[] { 0x50, 0x4B, 0x03 ,0x04 },
                    new byte[] { 0x50, 0x4B, 0x03, 0x04, 0x14, 0x00, 0x06 , 0x00 },
                }
            },
            {
                ".ppt", new List<byte[]>
                {
                    new byte[] { 0xD0, 0xCF, 0x11, 0xE0, 0xA1, 0xB1, 0x1A , 0xE1 },
                    new byte[] { 0x00, 0x6E, 0x1E , 0xF0 },
                    new byte[] { 0x0F, 0x00, 0xE8 , 0x03 },
                    new byte[] { 0xA0, 0x46, 0x1D , 0xF0 },
                    new byte[] { 0xFD, 0xFF, 0xFF, 0xFF, 0x0E, 0x00, 0x00 , 0x00 },
                    new byte[] { 0xFD, 0xFF, 0xFF, 0xFF, 0x1C, 0x00, 0x00 , 0x00 },
                    new byte[] { 0xFD, 0xFF, 0xFF, 0xFF, 0x43, 0x00, 0x00 , 0x00 },
                }
            },
            {
                ".pptx", new List<byte[]>
                {
                    new byte[] { 0x50, 0x4B, 0x03 ,0x04 },
                    new byte[] { 0x50, 0x4B, 0x03, 0x04, 0x14, 0x00, 0x06 , 0x00 },
                }
            },
        };
    }
}
