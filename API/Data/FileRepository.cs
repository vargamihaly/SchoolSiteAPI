using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Text;
using System.Threading.Tasks;
using API.Models.Entities;
using API.Models.Service;
using API.Models.DBContext;
using API.Models;

namespace API.Data
{
    public class FileRepository : IFileRepository
    {
        private readonly APIDBContext _context;
        private readonly IConfiguration _configuration;

        public FileRepository(APIDBContext context, IConfiguration configuration)
        {
            _configuration = configuration;
            _context = context;
        }

        public async Task<ServiceResponse<FileDto>> Download(string fileName)
        {
            ServiceResponse<FileDto> response = new ServiceResponse<FileDto>();

            if (!await FileExist(fileName))
            {
                response.Success = false;
                response.Message = "The file with this name does not exist.";
                return response;
            }

            UploadedFile uploadedFile = await _context.UploadedFiles.SingleAsync(_ => _.FileName == fileName);

            FileDto downloadableFile = new FileDto()
            {
                FileName = uploadedFile.FileName,
                Category = uploadedFile.Category.ToString(),
                FileAsString = ByteArrayToString((uploadedFile.FileAsByteArray)),
            };

            response.Data = downloadableFile;
            return response;
        }

        public async Task<ServiceResponse<string>> Upload(FileDto file)
        {
            ServiceResponse<string> response = new ServiceResponse<string>();

            if (await FileExist(file.FileName))
            {
                response.Success = false;
                response.Message = "File with the same name already exists.";
                return response;
            }

            if (!FileCategoryExist(file.Category))
            {
                response.Success = false;
                response.Message = "Category does not exist.";
                return response;
            }

            UploadedFile uploadableFile = new UploadedFile()
            {
                Category = (FileCategory)Enum.Parse(typeof(FileCategory), file.Category, true),
                Date = DateTime.Now,
                //FileAsByteArray = UnescapeText(file.FileAsByteArray),
                FileAsByteArray = StringToByteArray(file.FileAsString),
                FileName = file.FileName.ToLower()
            };

            await _context.UploadedFiles.AddAsync(uploadableFile);
            await _context.SaveChangesAsync();

            response.Data = file.FileName;
            return response;
        }

        public async Task<ServiceResponse<string>> DeleteUploadedFile(string fileName)
        {
            ServiceResponse<string> response = new ServiceResponse<string>();

            if (!await FileExist(fileName))
            {
                response.Success = false;
                response.Message = "File with the same name does not exist";
                return response;
            }

            UploadedFile uploadedFile = await _context.UploadedFiles.FirstAsync(_ => _.FileName.ToLower() == fileName.ToLower());

            _context.UploadedFiles.Remove(uploadedFile);
            await _context.SaveChangesAsync();

            response.Message = "File deleted successfully";
            return response;
        }

        public async Task<bool> FileExist(string fileName)
        {
            if (await _context.UploadedFiles.AnyAsync(x => x.FileName.ToLower() == fileName.ToLower()))
            {
                return true;
            }
            return false;
        }

        private bool FileCategoryExist(string category)
        {
            if (Enum.IsDefined(typeof(FileCategory), category))
            {
                return true;
            }
            return false;
        }

        private byte[] UnescapeText(byte[] decodableString)
        {
            Chilkat.StringBuilder sb = new Chilkat.StringBuilder();
            sb.Append(ByteArrayToString(decodableString));
            sb.Decode("json", "utf-8");
            
            byte[] unescapedString = StringToByteArray(sb.GetAsString());
            return unescapedString;
        }

        private byte[] EscapeText(byte[] encodableString)
        {
            Chilkat.StringBuilder sb = new Chilkat.StringBuilder();
            sb.Append(ByteArrayToString(encodableString));
            sb.Encode("json", "utf-8");

            byte[] escapedString = StringToByteArray(sb.GetAsString());
            return escapedString;
        }

        private byte[] StringToByteArray(string text)
        {
            byte[] bytes = Encoding.ASCII.GetBytes(text);
            return bytes;
        }

        private string ByteArrayToString(byte[] text)
        {
            string str = Encoding.ASCII.GetString(text);
            return str;
        }
    }
}
