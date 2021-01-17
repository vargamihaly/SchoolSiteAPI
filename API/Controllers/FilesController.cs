using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using API.Data;
using API.Models.Service;
using Microsoft.AspNetCore.Cors;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("CorsPolicy")]
    public class FilesController : ControllerBase
    {
        private readonly IFileRepository _fileRepository;

        public FilesController(IFileRepository fileRepository)
        {
            _fileRepository = fileRepository;
        }



        //// GET: api/UploadedFiles
        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<UploadedFile>>> GetUploadedFiles()
        //{
        //    return await _context.UploadedFiles.ToListAsync();
        //}

        // GET: api/UploadedFiles/5

        //[HttpGet("{id}")]
        //public async Task<ActionResult<UploadedFile>> GetUploadedFile(int id)
        //{
        //    var uploadedFile = await _context.UploadedFiles.FindAsync(id);

        //    if (uploadedFile == null)
        //    {
        //        return NotFound();
        //    }

        //    return uploadedFile;
        //}


        [HttpPost]
        public async Task<IActionResult> Upload(FileDto uploadedFile)
        {
            ServiceResponse<string> response = await _fileRepository.Upload(uploadedFile);

            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> Download(string fileName)
        {
            ServiceResponse<FileDto> response = await _fileRepository.Download(fileName);

            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        // DELETE: api/UploadedFiles/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUploadedFile(string fileName)
        {
            ServiceResponse<string> response = await _fileRepository.DeleteUploadedFile(fileName);

            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        //private bool UploadedFileExists(int id)
        //{
        //    return _context.UploadedFiles.Any(e => e.FileId == id);
        //}
    }
}
