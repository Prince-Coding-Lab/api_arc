using ShnoFeeh.API.Core.Dto;
using System.Threading.Tasks;

namespace ShnoFeeh.API.Core.Interfaces
{
    public interface IAppLogger
    {
        Task<int> LogInformationAsync(ExceptionLogDto exLog);
        void LogWarning(string message, params object[] args);
    }
}
