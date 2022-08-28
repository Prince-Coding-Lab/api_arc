using ShnoFeeh.API.Core.Dto;
using ShnoFeeh.BusinessService.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ShnoFeeh.BusinessService.Interfaces
{
    public interface IManagementService
    {
        Task<ResponseDto<List<CategoryDto>>> GetAllCategoriesAsync(int? cityId);
        Task<ResponseDto<CategoryDto>> AddCategoryAsync(AddCategoryDto model);
        Task<ResponseDto<CategoryDto>> DeleteCategoryAsync(int categoryId);
        Task<UploadImageResponse> UploadCategoryImage(Image model);
        Task<ResponseDto<AdvertismentDto>> UpdateAdvertisementImage(UpdateAdvertismentDto model);
        Task<ResponseDto<List<AdvertismentDto>>> GetAllAdvertisementsAsync(int cityId);
        Task<ResponseDto<AdvertismentDto>> DeleteAdvertisementAsync(int advertisementId);
    }
}
