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
         * 上传单个文件
         */
        [HttpPost]
        public async Task<IActionResult> UploadFileAsync(IFormFile file)
        {
            bool t = Domain.Files.Validation.ValidateExtension("a.png");
            t = Domain.Files.Validation.ValidateExtension("a.png");

            throw new NotImplementedException();
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
                ModelState.AddModelError("message",
                    $"The request couldn't be processed (Error 1).");
                logger.LogWarning("ContentType mistake: {0}", Request.ContentType);

                return BadRequest(ModelState);
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
                    ModelState.AddModelError("message", "不允许的文件扩展名");
                    return BadRequest(ModelState);
                }

                if (hasContentDispositionHeader)
                {
                    if (!MultipartRequestHelper
                        .HasFileContentDisposition(contentDisposition))
                    {
                        ModelState.AddModelError("message",
                            $"The request couldn't be processed (Error 2).");
                        logger.LogWarning("have no content disposition header");

                        return BadRequest(ModelState);
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
                //  section = await reader.ReadNextSectionAsync();
            }

            return BadRequest();
        }
    }
}
