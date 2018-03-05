using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Pages.areas._207.Data;

namespace Pages.areas._207
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
            services
                .AddSingleton<IPageApplicationModelProvider, FullNamePageApplicationModelProvider>()
                .AddSingleton<NoFallbackPageFilter>()
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                .Configure<CookiePolicyOptions>(options => options.CheckConsentNeeded = context => true);

            services.AddDbContext<ApplicationDbContext>(
                options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            services.AddIdentity<IdentityUser, IdentityRole>(options => options.Stores.MaxLengthForKeys = 128)
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultUI()
                .AddDefaultTokenProviders();

            services
                .AddMvc(options => options.Filters.AddService<NoFallbackPageFilter>())
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_0)
                .AddRazorPagesOptions(options =>
                {
                    options.AllowAreas = false;
                    options.Conventions
                        .AddAreaPageRoute(areaName: "MyArea", pageName: "/Contact", route: "/MyContact")
                        .AuthorizeAreaPage(areaName: "MyArea", pageName: "/Contact");
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseAuthentication();

            app.UseMvc();
        }
    }
}
