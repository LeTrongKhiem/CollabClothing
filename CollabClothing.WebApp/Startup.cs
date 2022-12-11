using CollabClothing.ApiShared;
using CollabClothing.ViewModels.System.Users;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace CollabClothing.WebApp
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
            services.AddHttpClient();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
            });
            //services.AddDbContext<ApplicationDbContext>(options =>
            //    options.UseSqlServer(
            //        Configuration.GetConnectionString("DefaultConnection")));
            //services.AddDatabaseDeveloperPageExceptionFilter();

            //services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
            //    .AddEntityFrameworkStores<ApplicationDbContext>();
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(option =>
            {
                option.LoginPath = "/Account/Login";
                option.AccessDeniedPath = "/User/Forbidden";
            }
               );

            services.AddAuthentication().AddGoogle(googleOptions =>
            {
                // Đọc thông tin Authentication:Google từ appsettings.json
                IConfigurationSection googleAuthNSection = Configuration.GetSection("Authentication:Google");

                // Thiết lập ClientID và ClientSecret để truy cập API google
                googleOptions.ClientId = googleAuthNSection["ClientId"];
                googleOptions.ClientSecret = googleAuthNSection["ClientSecret"];
                // Cấu hình Url callback lại từ Google (không thiết lập thì mặc định là /signin-google)
                googleOptions.CallbackPath = "/loginwithgoogle";
                googleOptions.SignInScheme = IdentityConstants.ExternalScheme;

            });


            services.AddControllersWithViews().AddRazorRuntimeCompilation();
            services.AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<LoginRequestValidator>());
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            IMvcBuilder builder = services.AddRazorPages();
#if DEBUG
            if (environment == Environments.Development)
            {
                builder.AddRazorRuntimeCompilation();
            }
#endif

            services.AddTransient<IBannerApiClient, BannerApiClient>();
            services.AddTransient<IBrandApiClient, BrandApiClient>();
            services.AddTransient<IProductApiClient, ProductApiClient>();
            services.AddTransient<ICategoryApiClient, CategoryApiClient>();
            services.AddTransient<IBrandApiClient, BrandApiClient>();
            services.AddTransient<IUserApiClient, UserApiClient>();
            services.AddTransient<IOrderApiClient, OrderApiClient>();
            services.AddTransient<IPromotionApiClient, PromotionApiClient>();
            services.AddTransient<ISizeApiClient, SizeApiClient>();
            services.AddTransient<IColorApiClient, ColorApiClient>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseAuthentication();
            app.UseRouting();
            app.UseAuthorization();
            app.UseSession();

            app.UseEndpoints(endpoints =>
            {
                //endpoints.MapControllerRoute(
                //  name: "Product Category",
                //  pattern: "/danh-muc/{id}", new
                //  {
                //      controller = "Product",
                //      action = "Category"

                //  });

                //endpoints.MapControllerRoute(
                //  name: "Product Category",
                //  pattern: "/danh-muc/{id}", new
                //  {
                //      controller = "Product",
                //      action = "Category"

                //  });
                endpoints.MapControllerRoute(
                  name: "Product Brand",
                  pattern: "/danh-muc/thuong-hieu/{brandId}", new
                  {
                      controller = "Product",
                      action = "Category"
                  });
                endpoints.MapControllerRoute(
                   name: "Product LoadMore",
                   pattern: "/danh-muc/thuong-hieu/load/{brandId}", new
                   {
                       controller = "LoadMore",
                       action = "Brand"

                   });
                endpoints.MapControllerRoute(
                  name: "Product Category",
                  pattern: "/danh-muc/{slug}", new
                  {
                      controller = "Product",
                      action = "Category"

                  });
                endpoints.MapControllerRoute(
                 name: "Product LoadMore",
                 pattern: "/danh-muc/loadmore/{slug}", new
                 {
                     controller = "LoadMore",
                     action = "Index",
                     //controller = "Product",
                     //action = "Category",
                 });
                //endpoints.MapControllerRoute(
                //  name: "Product Category",
                //  pattern: "/danh-muc/{slug}/{priceOrder}", new
                //  {
                //      controller = "Product",
                //      action = "Category"

                //  });

                //endpoints.MapControllerRoute(
                //  name: "Product LoadMore",
                //  pattern: "/danh-muc/{slug}/loadmore/{price}", new
                //  {
                //      controller = "LoadMore",
                //      action = "Index",
                //  });
                //endpoints.MapControllerRoute(
                //  name: "Product LoadMore",
                //  pattern: "/danh-muc/load/{slug}", new
                //  {
                //      controller = "LoadMore",
                //      action = "Index"

                //  });


                endpoints.MapControllerRoute(
                    name: "Product Details",
                    pattern: "{controller=Product}/{action=Detail}/{id?}");
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");

                //endpoints.MapRazorPages();
            });
        }
    }
}
