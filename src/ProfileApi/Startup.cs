using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using ProfileApi.Middleware;
using ProfileApi.Models;
using ProfileApi.Services;
using ProfileApi.Services.QueryService;

namespace ProfileApi
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
            services.AddScoped<IDataContext<Profile>, ProfileContext>();
            services.AddTransient<IRepository<Profile>, Repository<Profile>>();
            services.AddScoped<LoginService>();
            services.AddTransient<IQueryBuilder<Profile>, QueryBuilder<Profile>>();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            //Maps settings file to Settings Class at runtime
            services.Configure<Settings>(options =>
            {
                options.ConnectionString = Configuration.GetSection("MongoDb:ConnectionString").Value;
                options.Database = Configuration.GetSection("MongoDb:Database").Value;
                options.SecretToken = Configuration.GetSection("JWT:SecretToken")
                .Value;
                options.TokenIssuer = Configuration.GetSection("JWT:Issuer").Value;
            });


            var appSettingsSection = Configuration.GetSection("Api");

            var secretToken = Encoding.ASCII.GetBytes(Configuration.GetSection("JWT:SecretToken").Value);

            services.AddAuthentication(a =>
            {
                a.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                a.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(secretToken),
                    ValidateIssuer = false,// validate the server that generates the token
                    ValidateAudience = false//validate the user who generates token is authorized
                };
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseMiddleware(typeof(CustomExceptionMiddleware));
            app.UseAuthentication();
            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
