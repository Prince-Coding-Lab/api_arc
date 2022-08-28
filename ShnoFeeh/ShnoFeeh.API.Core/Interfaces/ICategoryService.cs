using ShnoFeeh.API.Core.Dto;
using ShnoFeeh.API.Core.Entities;
using System.Threading.Tasks;

namespace ShnoFeeh.API.Core.Interfaces
{
    public interface ICategoryService
    {
        Task<DatabaseResponse> AddCategoryAsync(Categories category);
        Task<DatabaseResponse> UpdateCategoryAsync(Categories category);
        Task<DatabaseResponse> GetCategoriesAsync(int? cityId,string lang);
        Task<DatabaseResponse> DeleteCategoryAsync(int categoryId);
    }
}
