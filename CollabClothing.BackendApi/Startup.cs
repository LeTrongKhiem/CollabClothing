using CollabClothing.Application.Catalog.Banners;
using CollabClothing.Application.Catalog.Brands;
using CollabClothing.Application.Catalog.Cart;
using CollabClothing.Application.Catalog.Categories;
using CollabClothing.Application.Catalog.Color;
using CollabClothing.Application.Catalog.Products;
using CollabClothing.Application.Catalog.Promotions;
using CollabClothing.Application.Catalog.Sizes;
using CollabClothing.Application.Catalog.Statistic;
using CollabClothing.Application.Common;
using CollabClothing.Application.System.Mail;
using CollabClothing.Application.System.Roles;
using CollabClothing.Application.System.Users;
using CollabClothing.BackendApi.Controllers;
using CollabClothing.Data.EF;
using CollabClothing.Data.Entities;
using CollabClothing.Utilities.Constants;
using CollabClothing.ViewModels.System.Mail;
using CollabClothing.ViewModels.System.Users;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;

namespace CollabClothing.BackendApi
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

            services.AddDbContext<CollabClothingDBContext>(options => options.UseSqlServer(Configuration.GetConnectionString(SystemConstans.MainConnection)));

            var emailConfig = Configuration.GetSection("MailSettings").Get<MailSettings>();
            services.AddSingleton(emailConfig);

            services.AddScoped<IEmailSender, MailService>();
            services.AddIdentity<AppUser, AppRole>()
            //.AddDefaultUI()
            .AddEntityFrameworkStores<CollabClothingDBContext>()
            .AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(options =>
            {
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(2);  // Khóa 2 phút
                options.Lockout.MaxFailedAccessAttempts = 3;                        // Thất bại 3 lần thì khóa
            });

            services.AddTransient<IPublicProductService, PublicProductService>();
            services.AddTransient<IManageProductService, ManageProductService>();
            services.AddTransient<IStorageService, FileStorageService>();
            services.AddTransient<ICategoryService, CategoryService>();
            services.AddTransient<IBrandService, BrandService>();
            services.AddTransient<IBannerService, BannerSevice>();
            services.AddTransient<IUtilities, UtilitiesHelp>();
            services.AddTransient<ISizeService, SizeService>();
            services.AddTransient<IColorService, ColorService>();
            services.AddTransient<ICartService, CartService>();
            services.AddTransient<IPromotionService, PromotionService>();
            services.AddTransient<IBenefitService, BenefitService>();
            services.AddTransient<IDemo111, demo111>();


            services.AddTransient<UserManager<AppUser>, UserManager<AppUser>>();
            services.AddTransient<SignInManager<AppUser>, SignInManager<AppUser>>();
            services.AddTransient<RoleManager<AppRole>, RoleManager<AppRole>>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IRoleService, RoleService>();
            // Đăng ký dịch vụ Mail
            // services.AddTransient<IValidator<LoginRequest>, LoginRequestValidator>();
            // services.AddTransient<IValidator<RegisterRequest>, RegisterRequestValidator>(); vi khai bao fv => fv.RegisterValidatorsFromAssemblyContaining

            string issuer = Configuration.GetValue<string>("Token:Issuer");
            string signingKey = Configuration.GetValue<string>("Token:Key");
            byte[] signingKeyBytes = System.Text.Encoding.UTF8.GetBytes(signingKey);
            services.AddAuthentication(auth =>
            {
                auth.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                auth.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                auth.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddCookie()
                .AddGoogle(googleOptions =>
                {
                    // Đọc thông tin Authentication:Google từ appsettings.json
                    IConfigurationSection googleAuthNSection = Configuration.GetSection("Authentication:Google");
                    googleOptions.CallbackPath = "/loginwithgoogle";

                    // Thiết lập ClientID và ClientSecret để truy cập API google
                    googleOptions.ClientId = googleAuthNSection["ClientId"];
                    googleOptions.ClientSecret = googleAuthNSection["ClientSecret"];
                    // Cấu hình Url callback lại từ Google (không thiết lập thì mặc định là /signin-google)
                    googleOptions.SignInScheme = IdentityConstants.ExternalScheme;

                })
                .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = issuer,
                    ValidateAudience = true,
                    ValidAudience = issuer,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ClockSkew = System.TimeSpan.Zero,
                    IssuerSigningKey = new SymmetricSecurityKey(signingKeyBytes)
                };
            });
            //services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie()
            //    .AddGoogle(googleOptions =>
            //    {
            //        // Đọc thông tin Authentication:Google từ appsettings.json
            //        IConfigurationSection googleAuthNSection = Configuration.GetSection("Authentication:Google");
            //        googleOptions.CallbackPath = "/loginwithgoogle";

            //        // Thiết lập ClientID và ClientSecret để truy cập API google
            //        googleOptions.ClientId = googleAuthNSection["ClientId"];
            //        googleOptions.ClientSecret = googleAuthNSection["ClientSecret"];
            //        // Cấu hình Url callback lại từ Google (không thiết lập thì mặc định là /signin-google)
            //        //googleOptions.SignInScheme = IdentityConstants.ApplicationScheme;

            //    });
            services.AddHttpContextAccessor();
            // services.AddControllersWithViews();
            services.AddControllers().AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<LoginRequestValidator>());

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "CollabClothing.BackendApi", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = @"JWT Authorization header using the Bearer scheme. \r\n\r\n 
                                  Enter 'Bearer' [space] and then your token in the text input below.
                                  \r\n\r\nExample: 'Bearer 12345abcdef'",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                  {
                    {
                      new OpenApiSecurityScheme
                      {
                        Reference = new OpenApiReference
                          {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                          },
                          Scheme = "oauth2",
                          Name = "Bearer",
                          In = ParameterLocation.Header,

                        },
                        new List<string>()
                      }
                    });
            });
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
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            //app.UseIdentity();
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseAuthentication();
            app.UseRouting();
            app.UseAuthorization();
            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Swagger CollabClothing V1");
                c.RoutePrefix = string.Empty;
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
