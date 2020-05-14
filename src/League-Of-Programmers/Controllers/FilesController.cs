using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using League_Of_Programmers.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

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
         * 上传单个小文件
         */
        [HttpPost]
        public async Task<IActionResult> UploadFileAsync(IFormFile file)
        {
            if (file is null)
                return BadRequest(ModelState.AddMessageError("没有上传文件"));

            var trustedFileNameForDisplay = WebUtility.HtmlEncode(file.FileName);
            var trustedFileNameForFileStorage = Path.GetRandomFileName();
            string extension = Path.GetExtension(trustedFileNameForDisplay);
            string fileName = Path.GetFileNameWithoutExtension(trustedFileNameForFileStorage);
            trustedFileNameForFileStorage = fileName + extension;

            if (!Domain.Files.Validation.ValidateExtension(trustedFileNameForFileStorage))
            {
                return BadRequest(ModelState.AddMessageError("不允许的文件扩展名"));
            }

            string saveWebPath = configuration.GetSection("File:SaveWebPath").Value;
            string saveFullPath = Path.Combine(Directory.GetCurrentDirectory(), saveWebPath, trustedFileNameForFileStorage);

            await using var fileStream = System.IO.File.Create(saveFullPath);
            await file.CopyToAsync(fileStream);

            (bool isSuccess, int id) = await Domain.Files.File.SaveToDBAsync(trustedFileNameForDisplay, trustedFileNameForFileStorage, file.Length);
            if (isSuccess)
                return Created(saveWebPath, id);

            //  delete the save file if save to DB fault
            System.IO.File.Delete(saveFullPath);
            return Accepted();
        }

        /*
         * 流式上传单个大文件
         */
        [HttpPost("stream"),
            Filters.DisableFormValueModelBinding,
            //Filters.GenerateAntiforgeryTokenCookie, 
            //ValidateAntiForgeryToken]
            ]
        public async Task<IActionResult> StreamUploadFileAsync()
        {
            if (!MultipartRequestHelper.IsMultipartContentType(Request.ContentType))
            {
                logger.LogWarning("ContentType mistake: {0}", Request.ContentType);
                return BadRequest(ModelState.AddMessageError($"文件媒体类型不允许: {Request.ContentType}"));
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

                if (!Domain.Files.Validation.ValidateExtension(trustedFileNameForFileStorage))
                {
                    return BadRequest(ModelState.AddMessageError("不允许的文件扩展名"));
                }

                if (hasContentDispositionHeader)
                {
                    if (!MultipartRequestHelper.HasFileContentDisposition(contentDisposition))
                    {
                        logger.LogWarning("have no content disposition header");

                        return BadRequest(ModelState.AddMessageError("have no content disposition header"));
                    }
                    else
                    {
                        string saveWebPath = configuration.GetSection("File:SaveWebPath").Value;
                        string saveFullPath = Path.Combine(Directory.GetCurrentDirectory(), saveWebPath, trustedFileNameForFileStorage);

                        await using var fileStream = System.IO.File.Create(saveFullPath);
                        await section.Body.CopyToAsync(fileStream);

                        //  validation file safe
                        //bool isSafe = Domain.Files.Validation.SignatureValidation(fileStream);
                        //if (!isSafe)
                        //{
                        //    System.IO.File.Delete(saveFullPath);
                        //    ModelState.AddModelError("message", "文件签名有误");
                        //    return BadRequest(ModelState);
                        //}

                        return Created(saveWebPath, null);
                    }
                }
            }

            return BadRequest();
        }
    }
}
