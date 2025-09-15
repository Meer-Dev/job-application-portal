using System.Threading.Tasks;
using Abp.Application.Services;
using Microsoft.AspNetCore.Http;

namespace solvefy.task.FileUpload
{
    public interface IFileUploadAppService : IApplicationService
    {
        Task<string> UploadResumeAsync(IFormFile file);
    }
}