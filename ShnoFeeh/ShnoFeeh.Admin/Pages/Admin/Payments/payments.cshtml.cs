using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ShnoFeeh.API.Core.DomainModels;
using ShnoFeeh.API.Core.Dto;
using ShnoFeeh.BusinessService.Common.Constant;
using ShnoFeeh.BusinessService.Common.Manager;
using ShnoFeeh.BusinessService.Interfaces;

namespace ShnoFeeh.Admin.Pages
{
    [Authorize(Roles = "Admin,Company")]
    public class paymentsModel : PageModel
    {
        public readonly ISessionManager _sessionManager;
        private readonly IMarketingService _marketingService;

        #region PageProperties
        [ViewData]
        public string SuccessMessage { get; set; }
        [ViewData]
        public string ErrorMessage { get; set; }
        [BindProperty]
        public List<UserDto> UserDto { get; set; }
        [BindProperty]
        public List<OrderDto> OrderDtos { get; set; }
        [BindProperty]
        public decimal Amount { get; set; }
        #endregion
        public paymentsModel(ISessionManager sessionManager, IMarketingService marketingService)
        {
            _sessionManager = sessionManager;
            _marketingService = marketingService;
        }
        public async Task OnGetAsync()
        {
            if (_sessionManager.GetObject<UserDto>("userInfo") != null)
            {
                await GetOrders();
            }
            else
            {
                RedirectToPage("/Admin/Account/login");
            }
        }
        public IActionResult OnPostAdDetails(int adId)
        {
            _sessionManager.SetInt("adId", adId);
            return Redirect("/Admin/Ads/AdDetails");
        }

        public async Task GetOrders()
        {
            var response = await _marketingService.GetAllOrdersAsync();
            if (response.HasSucceeded)
            {
                if (_sessionManager.GetString(SessionItems.RoleName) == "Admin")
                {
                    OrderDtos = response.ReturnedObject;
                    for (int i = 0; i < OrderDtos.Count(); i++)
                    {
                        var resp = await _marketingService.GetAdByIdAsync(OrderDtos[i].OrderAds[0].AdId);
                        {
                            if (resp.HasSucceeded && resp.ReturnedObject.Count() > 0)
                            {
                                if (resp.ReturnedObject[0].CampaginName == null)
                                {
                                    OrderDtos.RemoveAt(i);
                                }
                            }
                        }
                    }
                    Amount = OrderDtos.Sum(x => x.TotalPrice) ?? 0.00m;
                }
                else
                {
                    OrderDtos = response.ReturnedObject.Where(x => x.UserId == _sessionManager.GetInt(SessionItems.Id)).ToList();
                    for (int i = 0; i < OrderDtos.Count(); i++)
                    {
                        var resp = await _marketingService.GetAdByIdAsync(OrderDtos[i].OrderAds[0].AdId);
                        {
                            if (resp.HasSucceeded && resp.ReturnedObject.Count() > 0)
                            {
                                if (resp.ReturnedObject[0].CampaginName == null)
                                {
                                    OrderDtos.RemoveAt(i);
                                }
                            }
                        }
                    }
                    Amount = OrderDtos.Sum(x => x.TotalPrice) ?? 0.00m;
                }
            }

        }
        public async Task<IActionResult> OnGetAmountAsync(int month)
        {
            await GetOrders();
            if (month == 0)
            {
                Amount = OrderDtos.Sum(x => x.TotalPrice) ?? 0.00m;
            }
            else if (month == 1)
            {
                DateTime oneMonth = DateTime.Now.AddMonths(-1);
                Amount = OrderDtos.Where(x => x.CreatedDate <= DateTime.Now && x.CreatedDate >= oneMonth).Sum(x => x.TotalPrice) ?? 0.00m;

            }
            else if (month == 3)
            {
                DateTime oneMonth = DateTime.Now.AddMonths(-3);
                Amount = OrderDtos.Where(x => x.CreatedDate <= DateTime.Now && x.CreatedDate >= oneMonth).Sum(x => x.TotalPrice) ?? 0.00m;
            }
            else if (month == 6)
            {
                DateTime oneMonth = DateTime.Now.AddMonths(-6);
                Amount = OrderDtos.Where(x => x.CreatedDate <= DateTime.Now && x.CreatedDate >= oneMonth).Sum(x => x.TotalPrice) ?? 0.00m;
            }
            else if (month == 12)
            {
                DateTime oneMonth = DateTime.Now.AddMonths(-12);
                Amount = OrderDtos.Where(x => x.CreatedDate <= DateTime.Now && x.CreatedDate >= oneMonth).Sum(x => x.TotalPrice) ?? 0.00m;
            }

            return new JsonResult(Amount);
        }

        public ActionResult OnPostGetInvoiceAsync(int invoiceId)
        {

            PaymentStatusRequest req = new PaymentStatusRequest()
            {
                Key = invoiceId.ToString(),
                KeyType = "InvoiceId"
            };
            var response = _marketingService.GetPaymentStatusAsync(req);
            if (response.Result.HasSucceeded)
            {
                return new JsonResult(response.Result.ReturnedObject);
            }

            return new JsonResult("");
        }

    }
}
