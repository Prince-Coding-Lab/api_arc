using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using ShnoFeeh.API.Core.Dto;
using ShnoFeeh.API.Core.Entities;
using ShnoFeeh.API.Core.Enums;
using ShnoFeeh.API.Core.Interfaces;
using ShnoFeeh.API.Model;

namespace ShnoFeeh.API.Controllers
{
    [Authorize(Roles = "admin,company")]
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        #region Fields
        private readonly ICategoryService _categoryService;
        private readonly IMapper _mapper;
        IConfiguration _iconfiguration;
        #endregion

        #region Constructors 
        public CategoryController(ICategoryService categoryService, IMapper mapper, IConfiguration configuration)
        {
            _categoryService = categoryService;
            _mapper = mapper;
            _iconfiguration = configuration;
        }
        #endregion

        #region Action Methods
        /// <summary>
        /// Add new category
        /// </summary>
        /// <param name="category">category object</param>
        /// <returns>Api Response</returns>
        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] AddCategoryDto category)
        {
            if (!ModelState.IsValid)
            {
                return Ok(ApiResponse.ValidationErrorResponse(ModelState));
            }
            var addToCategories = _mapper.Map<Categories>(category);

            DatabaseResponse response = await _categoryService.AddCategoryAsync(addToCategories);

            if (response.ResponseCode == (int)DbReturnValue.CreateSuccess)
            {
                return Ok(ApiResponse.OkResult(true, response.Results, DbReturnValue.CreateSuccess));
            }
            return Ok(ApiResponse.OkResult(true, response.Results, DbReturnValue.RecordExists));

        }
        /// <summary>
        /// Update the category information.
        /// </summary>
        /// <param name="category">update category object</param>
        /// <returns>Api Response</returns>
        [HttpPut]
        public async Task<IActionResult> PutAsync([FromBody] UpdateCategoryDto category)
        {
            if (!ModelState.IsValid)
            {
                return Ok(ApiResponse.ValidationErrorResponse(ModelState));
            }
            var updateCategory = _mapper.Map<Categories>(category);

            DatabaseResponse response = await _categoryService.UpdateCategoryAsync(updateCategory);

            if (response.ResponseCode == (int)DbReturnValue.UpdateSuccess)
            {
                return Ok(ApiResponse.OkResult(true, response.Results, DbReturnValue.UpdateSuccess));
            }
            else if (response.ResponseCode == (int)DbReturnValue.RecordExists)
            {
                return Ok(ApiResponse.OkResult(true, response.Results, DbReturnValue.RecordExists));
            }
            return Ok(ApiResponse.OkResult(true, response.Results, DbReturnValue.NotExists));
        }
        /// <summary>
        /// Get all categories by country Id or all
        /// </summary>
        /// <param name="cityId">Optional Param CityId</param>
        /// <returns>Api Response</returns>
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetAsync(int? cityId, string lang)
        {
            DatabaseResponse response = await _categoryService.GetCategoriesAsync(cityId,lang);

            if (response.ResponseCode == (int)DbReturnValue.RecordExists)
            {
                return Ok(ApiResponse.OkResult(true, response.Results, DbReturnValue.RecordExists));
            }
            return Ok(ApiResponse.OkResult(true, response.Results, DbReturnValue.NotExists));
        }

        /// <summary>
        /// Delete a user
        /// </summary>
        /// <param name="userId">User Id</param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IActionResult> DeleteAsync(int categoryId)
        {
            DatabaseResponse response = await _categoryService.DeleteCategoryAsync(categoryId);

            if (response.ResponseCode == (int)DbReturnValue.DeleteSuccess)
            {
                return Ok(ApiResponse.OkResult(true, response.Results, DbReturnValue.DeleteSuccess));
            }
            else if (response.ResponseCode == (int)DbReturnValue.ActiveTryDelete)
            {
                return Ok(ApiResponse.OkResult(true, response.Results, DbReturnValue.ActiveTryDelete));
            }
            return Ok(ApiResponse.OkResult(true, response.Results, DbReturnValue.NotExists));
        }




        #endregion
    }
}