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
    public class OrderController : ControllerBase
    {
        #region Fields
        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;
        IConfiguration _iconfiguration;
        #endregion

        #region Constructors
        public OrderController(IOrderService orderService,
            IMapper mapper, IConfiguration configuration)
        {
            _orderService = orderService;
            _mapper = mapper;
            _iconfiguration = configuration;
        }
        #endregion

        #region Action Methods
        /// <summary>
        /// Create a new order
        /// </summary>
        /// <param name="order">Order Api</param>
        /// <returns>Api Response</returns>
        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] AddOrderDto order)
        {
            if (!ModelState.IsValid)
            {
                return Ok(ApiResponse.ValidationErrorResponse(ModelState));
            }
            var addToOrder = _mapper.Map<Orders>(order);

            DatabaseResponse response = await _orderService.AddOrderAsync(addToOrder);

            if (response.ResponseCode == (int)DbReturnValue.CreateSuccess)
            {
                return Ok(ApiResponse.OkResult(true, response.Results, DbReturnValue.CreateSuccess));
            }
            else
            {
                return Ok(ApiResponse.OkResult(true, response.Results, DbReturnValue.RecordExists));
            }

        }
        /// <summary>
        /// Update Order
        /// </summary>
        /// <param name="order">Updated objects</param>
        /// <returns>Api Response</returns>
        [HttpPut]
        public async Task<IActionResult> PutAsync([FromBody] UpdateOrderDto order)
        {
            if (!ModelState.IsValid)
            {
                return Ok(ApiResponse.ValidationErrorResponse(ModelState));
            }
            var updateToOrder = _mapper.Map<Orders>(order);

            DatabaseResponse response = await _orderService.UpdateOrderAsync(updateToOrder);

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
        /// Get all orders
        /// </summary>
        /// <returns>Api Response</returns>
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAsync()
        {
            if (!ModelState.IsValid)
            {
                return Ok(ApiResponse.ValidationErrorResponse(ModelState));
            }
            DatabaseResponse response = await _orderService.GetOrdersAsync();

            if (response.ResponseCode == (int)DbReturnValue.RecordExists)
            {
                return Ok(ApiResponse.OkResult(true, response.Results, DbReturnValue.RecordExists));
            }
            else
            {
                return Ok(ApiResponse.OkResult(true, response.Results, DbReturnValue.NotExists));
            }

        }
        #endregion
    }
}