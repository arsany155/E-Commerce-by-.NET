
using E_Commerce_API_Angular_Project.Interfaces;
using E_Commerce_API_Angular_Project.IRepository;
using E_Commerce_API_Angular_Project.Models;
using E_Commerce_API_Angular_Project.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics;
using System.Text;

namespace E_Commerce_API_Angular_Project
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
        
            //------------
            builder.Services.AddDbContext<EcommContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("cs"));
            });
            //builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));

            //---------------------------------------------------------------

            builder.Services.AddIdentity<appUser, IdentityRole<int>>()
               .AddEntityFrameworkStores<EcommContext>();

            builder.Services.AddAuthentication(options => {
                //Check JWT Token Header
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                //[authrize]
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;//return unauth
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>//verified key
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidIssuer = builder.Configuration["JWT:IssuerIP"],
                    ValidateAudience = true,
                    ValidAudience = builder.Configuration["JWT:AudienceIP"],
                    IssuerSigningKey =
                        new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(builder.Configuration["JWT:SecritKey"]))

                };
            });


            builder.Services.AddScoped<IAppUserRepo,AppUserRepo>();


            #region peterKameel
            builder.Services.AddScoped<IProductRepo, ProductRepo>();
            builder.Services.AddScoped<ICategoryRepo, CategoryRepo>();
            builder.Services.AddScoped<IBrandRepo, BrandRepo>();
            builder.Services.AddScoped<IReviewRepo, ReviewRepo>();

            #endregion




            builder.Services.AddScoped<IFavListRepo, FavListRepo>();
            builder.Services.AddScoped<IFavListItemsRepo, FavListItemsRepo>();
            builder.Services.AddScoped<IMailRepo, MailRepo>();
            builder.Services.AddScoped<IUserOtpRepo, UserOtpRepo>();
            builder.Services.AddScoped<IUserRoleRepo,UserRoleRepo>();
            builder.Services.AddScoped<ITestImg, TestImg>();
            builder.Services.AddScoped<IimageAsStringRepo, ImageAsStringRepo>();


            #region shaimaa
            builder.Services.AddScoped<IOrderRepository, OrderRepository>();
            builder.Services.AddScoped<IOrderItemRepository, OrderItemRepository>();
            #endregion

            #region Nourhan
            builder.Services.AddScoped<ICartRepo, CartRepo>();
            builder.Services.AddScoped<ICartItemRepo, CartItemRepo>();

            #endregion

            //-------------------------------------------------
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("MyPolicy", policy => {
                    policy.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();

                });
            });




            //---------------------------------------------------------------

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            //-----------------------
            

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseStaticFiles();
            app.UseCors("MyPolicy");

          

            app.UseAuthorization();

            app.MapControllers();
       
            app.Run();
        }
    }
}
