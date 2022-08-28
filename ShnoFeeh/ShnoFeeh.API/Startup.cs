using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using ShnoFeeh.API.Core.Interfaces;
using ShnoFeeh.API.Core.Services;
using ShnoFeeh.API.Infrastructure.Data;
using ShnoFeeh.API.Infrastructure.Logging;
using ShnoFeeh.API.Infrastructure.Services;
using ShnoFeeh.API.Middleware;
using System;
using System.Text;

namespace ShnoFeeh.API
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
            services.AddControllers();
            var key = Encoding.ASCII.GetBytes(Configuration.GetValue<string>("AppSettings:Secret"));
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddTransient(typeof(IDataAccessHelper), typeof(DataAccessHelper));
            services.AddTransient(typeof(IAppLogger), typeof(LoggerAdapter));
            services.AddTransient(typeof(IHttpClientHelper), typeof(HttpClientHelper));
            services.AddSingleton<ICommon, Common>();
            services.AddScoped<ICountryService, CountryService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IContactUsService, ContactUsService>();
            services.AddTransient<IEmailSender, EmailSender>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IAdsService, AdsService>();
            services.AddScoped<ICampaginService, CampaginService>();
            services.AddScoped<IPaymentRefService, PaymentRefService>();
            services.AddScoped<IAdsPricesService, AdsPricesService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<ICityService, CityService>();
            services.AddScoped<IExceptionService, ExceptionService>();
            services.AddScoped<IPaymentService, PaymentService>();
            services.AddScoped<IAdvertismentService, AdvertismentService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseMiddleware(typeof(ExceptionMiddleware));
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
