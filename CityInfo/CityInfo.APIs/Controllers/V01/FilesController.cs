using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;

namespace CityInfo.APIs.Controllers.V01
{
    [ApiController]
    //[Authorize]
    [ApiVersion(0.1, Deprecated = true)]
    [Route("api/v{version:apiVersion}/files")]
    public class FilesController : ControllerBase
    {
        #region [ Fields ]
        private readonly FileExtensionContentTypeProvider _fileExtensionContentTypeProvider;
        #endregion

        #region [ Constructure ]
        public FilesController(
            FileExtensionContentTypeProvider fileExtensionContentTypeProvider)
        {
            _fileExtensionContentTypeProvider = fileExtensionContentTypeProvider
                ?? throw new ArgumentNullException(
                    nameof(fileExtensionContentTypeProvider));
        }
        #endregion

        #region [ GET Methods ]
        [HttpGet("{fileId}")]
        public ActionResult GetFile(string fileId)
        {
            var pathToFile = "CourseModule.pdf";
            
            if (!System.IO.File.Exists(pathToFile))
                return NotFound("File not found!");

            if (!_fileExtensionContentTypeProvider.TryGetContentType(pathToFile, out var contentType))
                contentType = "application/octet-stream";

            var bytes = System.IO.File.ReadAllBytes(pathToFile);

            return File(bytes, contentType, Path.GetFileName(pathToFile));
        }
        #endregion
    }
}
