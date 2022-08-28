using System;
using System.Globalization;
using System.Linq;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using ShnoFeeh.BusinessService.Common.Manager;
using ShnoFeeh.BusinessService.Common.Methods;
using ShnoFeeh.BusinessService.Common.Middleware;
using ShnoFeeh.BusinessService.Factories;
using ShnoFeeh.BusinessService.Interfaces;
using ShnoFeeh.BusinessService.Services;

namespace ShnoFeeh.Admin
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            #region Strict Transport Security
            services.AddHsts(options =>
            {
                options.Preload = true;
                options.IncludeSubDomains = true;
                options.MaxAge = TimeSpan.FromDays(60);
                options.ExcludedHosts.Add("ShnoFeeh.com");
                options.ExcludedHosts.Add("www.ShnoFeeh.com");
            });
            #endregion

            services.AddSession(options =>
            {
                // Set a short timeout for easy testing.
                options.IdleTimeout = TimeSpan.FromSeconds(8000);
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                // Make the session cookie essential
                options.Cookie.IsEssential = true;
            });


            #region Authentication



            
            //services.ConfigureApplicationCookie(o => o.LoginPath = "/Admin/Account/login");
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
             options.LoginPath = "/Admin/Account/login"             );
            services.ConfigureApplicationCookie(o => o.LoginPath = "/");

            #endregion

            // custom services
            services.AddHttpContextAccessor();
            services.AddServerSideBlazor(); // for components
            services.AddSingleton<IShnoFeehHttpFactory, ShnoFeehHttpFactory>();
            services.AddSingleton<IAccountService, AccountService>();
            services.AddSingleton<ISessionManager, SessionManager>();
            services.AddSingleton<ICommonService, CommonService>();
            services.AddSingleton<IDemographicService, DemographicService>();
            services.AddSingleton<IManagementService, ManagementService>();
            services.AddSingleton<IMarketingService, MarketingService>();            
            services.AddLocalization(options => options.ResourcesPath = "Resources");


            services.AddHttpClient("IShnoFeehHttpFactory");
            services.AddMvc().AddRazorPagesOptions(options =>
            {
                options.Conventions.Clear();
                //options.Conventions.AddPageRoute("/Admin/Account/login", "");
                //options.Conventions.AddPageRoute("/Index", "");
            });
            services.AddDataProtection();


            //services.AddScoped<Requestloc>
            services.AddRazorPages().AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix).AddDataAnnotationsLocalization(options =>
            {
                options.DataAnnotationLocalizerProvider = (type, factory) =>
                    factory.Create(typeof(SharedResource));
            });

            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = Microsoft.AspNetCore.Http.SameSiteMode.None;


            });

            #region Localization
            services.Configure<RequestLocalizationOptions>(options =>
            {
                var supportedCultures = new[]
                 {
                    new CultureInfo("en-US"),
                    new CultureInfo("ar-SA")
                };
                options.DefaultRequestCulture = new RequestCulture("en-GB");
                options.SupportedUICultures = supportedCultures;
            });
            //services.AddMvc().AddViewLocalization();

            #endregion

            services.AddRazorPages().AddRazorRuntimeCompilation();
            //services.AddRazorPages().AddViewLocalization();
            services.AddAntiforgery(o => o.HeaderName = "XSRF-TOKEN");
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            var cookiePolicyOptions = new CookiePolicyOptions
            {
                MinimumSameSitePolicy = Microsoft.AspNetCore.Http.SameSiteMode.None,
                Secure = CookieSecurePolicy.Always,
                HttpOnly = Microsoft.AspNetCore.CookiePolicy.HttpOnlyPolicy.Always
            };
            app.UseStaticFiles(new StaticFileOptions
            {
                OnPrepareResponse = ctx =>
                {
                    const int durationInSeconds = 60 * 60 * 24;
                    ctx.Context.Response.Headers[HeaderNames.CacheControl] =
                        "public,max-age=" + durationInSeconds;
                }
            });

            app.UseCookiePolicy(cookiePolicyOptions);
            app.UseHttpsRedirection();
            app.UseSession();
            app.UseRouting();
            app.UseMiddleware(typeof(ExceptionMiddleware));
            app.UseAuthentication();
            app.UseAuthorization();


            var localizationOptions = app.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>().Value;
            app.UseRequestLocalization(localizationOptions);

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/");

                endpoints.MapRazorPages();
            });
        }
    }
}
