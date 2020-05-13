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
                ModelState.AddModelError("File",
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

                var trustedFileNameForFileStorage = Path.GetRandomFileName();
                var trustedFileNameForDisplay = WebUtility.HtmlEncode(contentDisposition.FileName.Value);

                if (hasContentDispositionHeader)
                {
                    if (!MultipartRequestHelper
                        .HasFileContentDisposition(contentDisposition))
                    {
                        ModelState.AddModelError("File",
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

                        return Created(saveFullPath, null);
                    }
                }
                //  section = await reader.ReadNextSectionAsync();
            }

            return BadRequest();
        }
    }
}
