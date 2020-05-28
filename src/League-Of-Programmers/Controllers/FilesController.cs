using League_Of_Programmers.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace League_Of_Programmers.Controllers
{
    public class FilesController : LOPController
    {
        private readonly ILogger logger;
        private readonly IConfiguration configuration;
        public FilesController(ILogger<FilesController> logger, IConfiguration configuration)
        {
            this.logger = logger;
            this.configuration = configuration;
        }

        /*
         *  上传单个小文件
         *  文件大小限制 64 KB
         * 
         *  return:
         *      400:    fail
         *      201:    created
         *      202:    accepted file but create fail
         */
        [HttpPost]
        public async Task<IActionResult> UploadFileAsync(IFormFile file)
        {
            if (file is null)
                return BadRequest("没有上传文件");

            if (!int.TryParse(configuration.GetSection("File:AvatarFileSizeLimet").Value, out int smallFileMaxLength))
                smallFileMaxLength = 64;

            if (file.Length > smallFileMaxLength)
                return BadRequest($"上传的文件最大只能限制在{smallFileMaxLength}字节");

            var trustedFileNameForDisplay = WebUtility.HtmlEncode(file.FileName);
            var trustedFileNameForFileStorage = Path.GetRandomFileName();
            string extension = Path.GetExtension(trustedFileNameForDisplay);
            string fileName = Path.GetFileNameWithoutExtension(trustedFileNameForFileStorage);
            trustedFileNameForFileStorage = fileName + extension;

            if (!Files.Validation.ValidateExtension(trustedFileNameForFileStorage))
            {
                return BadRequest("不允许的文件扩展名");
            }

            string saveWebPath = configuration.GetSection("File:SaveWebPath").Value;
            string saveFullPath = Path.Combine(Directory.GetCurrentDirectory(), saveWebPath, trustedFileNameForFileStorage);

            await using var fileStream = System.IO.File.Create(saveFullPath);
            await file.CopyToAsync(fileStream);

            (bool isSuccess, int id) = await Files.File.SaveToDBAsync(trustedFileNameForDisplay, trustedFileNameForFileStorage, file.Length);
            if (isSuccess)
                return Created(saveWebPath, id);

            //  delete the save file if save to DB fault
            System.IO.File.Delete(saveFullPath);
            return Accepted();
        }

        /*
         *  流式上传单个大文件
         *  return:
         *      400:    fail
         *      201:    created
         *      202:    accepted file but create fail
         */
        [HttpPost("stream"), Filters.DisableFormValueModelBinding]
        public async Task<IActionResult> StreamUploadFileAsync()
        {
            if (!MultipartRequestHelper.IsMultipartContentType(Request.ContentType))
            {
                logger.LogWarning("ContentType mistake: {0}", Request.ContentType);
                return BadRequest($"文件媒体类型不允许: {Request.ContentType}");
            }

            var boundary = MultipartRequestHelper.GetBoundary(
                MediaTypeHeaderValue.Parse(Request.ContentType), 256);
            var reader = new MultipartReader(boundary, HttpContext.Request.Body);
            var section = await reader.ReadNextSectionAsync();


            if (section != null)
            {
                var hasContentDispositionHeader =
                    ContentDispositionHeaderValue.TryParse(section.ContentDisposition, out var contentDisposition);

                var trustedFileNameForDisplay = WebUtility.HtmlEncode(contentDisposition.FileName.Value);
                var trustedFileNameForFileStorage = Path.GetRandomFileName();
                string extension = Path.GetExtension(trustedFileNameForDisplay);
                string fileName = Path.GetFileNameWithoutExtension(trustedFileNameForFileStorage);
                trustedFileNameForFileStorage = fileName + extension;

                if (!Files.Validation.ValidateExtension(trustedFileNameForFileStorage))
                {
                    return BadRequest("不允许的文件扩展名");
                }

                if (hasContentDispositionHeader)
                {
                    if (!MultipartRequestHelper.HasFileContentDisposition(contentDisposition))
                    {
                        logger.LogWarning("have no content disposition header");
                        return BadRequest("have no content disposition header");
                    }
                    else
                    {
                        if (!int.TryParse(configuration.GetSection("File:FileSizeLimit").Value, out int FileMaxLength))
                            FileMaxLength = 9 << 10 << 10;

                        if (section.Body.Length > FileMaxLength)
                            return BadRequest($"上传的文件最大只能限制在{FileMaxLength}字节");

                        string saveWebPath = configuration.GetSection("File:SaveWebPath").Value;
                        string saveFullPath = Path.Combine(Directory.GetCurrentDirectory(), saveWebPath, trustedFileNameForFileStorage);

                        await using var fileStream = System.IO.File.Create(saveFullPath);
                        await section.Body.CopyToAsync(fileStream);

                        (bool isSuccess, int id) = await Files.File.SaveToDBAsync(trustedFileNameForDisplay, trustedFileNameForFileStorage, fileStream.Length);
                        if (isSuccess)
                            return Created(saveWebPath, id);
                    }
                }
            }

            return Accepted();
        }
    }
}
