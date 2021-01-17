using API.Models.Service;
using System.Threading.Tasks;



namespace API.Data
{
    public interface IFileRepository
    {
        Task<ServiceResponse<string>> Upload(FileDto user);
        Task<ServiceResponse<FileDto>> Download(string fileName);
        Task<ServiceResponse<string>> DeleteUploadedFile(string fileName);
        Task<bool> FileExist(string fileName);
    }
}
