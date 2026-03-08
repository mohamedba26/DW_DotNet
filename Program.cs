using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SalesDW.API.Data;
using SalesDW.API.Services.AuthService;
using SalesDW.API.Services.AuthProductService;
using SalesDW.API.Services.DimCustomerService;
using SalesDW.API.Services.DimDateService;
using SalesDW.API.Services.DimProductService;
using SalesDW.API.Services.DimTerritoryService;
using SalesDW.API.Services.DimVendorService;
using SalesDW.API.Services.FactPurchasingService;
using SalesDW.API.Services.FactSaleService;
using SalesDW.API.Services.CommandService;
using SalesDW.API.Services.CommandLineService;
using SalesDW.API.Services.VwProductProfitService;
using SalesDW.API.Services.VwPurchasingBaseService;
using SalesDW.API.Services.VwPurchasingByVendorService;
using SalesDW.API.Services.VwSalesBaseService;
using SalesDW.API.Services.VwSalesByTerritoryService;
using SalesDW.API.Services.VwTopProductService;
using SalesDW.API.Services.VwTotalSalesByYearService;
using SalesDW.API.Services.KpiService;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

// Add CORS services
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
        policy =>
        {
            policy.WithOrigins("http://localhost:3000", // Replace with your client app's origin
                                "https://www.example.com")
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "SalesDW API",
        Version = "v1"
    });

    // 🔐 Add JWT Authentication to Swagger
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter: Bearer {your JWT token}"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

builder.Services.AddDbContext<DwSalesPurchasingContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddDbContext<AuthDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("AuthConnection")));

// Register IHttpContextAccessor so services can read the current HttpContext (e.g., user claims)
builder.Services.AddHttpContextAccessor();

// Register services
builder.Services.AddScoped<IDimCustomerService, DimCustomerService>();
builder.Services.AddScoped<IDimProductService, DimProductService>();
builder.Services.AddScoped<IDimDateService, DimDateService>();
builder.Services.AddScoped<IDimTerritoryService, DimTerritoryService>();
builder.Services.AddScoped<IDimVendorService, DimVendorService>();
builder.Services.AddScoped<IFactPurchasingService, FactPurchasingService>();
builder.Services.AddScoped<IFactSaleService, FactSaleService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IAuthProductService, AuthProductService>();
builder.Services.AddScoped<ICommandService, CommandService>();
builder.Services.AddScoped<ICommandLineService, CommandLineService>();

// Register generated view services
builder.Services.AddScoped<IVwProductProfitService, VwProductProfitService>();
builder.Services.AddScoped<IVwPurchasingBaseService, VwPurchasingBaseService>();
builder.Services.AddScoped<IVwPurchasingByVendorService, VwPurchasingByVendorService>();
builder.Services.AddScoped<IVwSalesBaseService, VwSalesBaseService>();
builder.Services.AddScoped<IVwSalesByTerritoryService, VwSalesByTerritoryService>();
builder.Services.AddScoped<IVwTopProductService, VwTopProductService>();
builder.Services.AddScoped<IVwTotalSalesByYearService, VwTotalSalesByYearService>();
builder.Services.AddScoped<IKpiService, KpiService>();

// Configure JWT
var jwtKey = builder.Configuration["Jwt:Key"];
var jwtIssuer = builder.Configuration["Jwt:Issuer"];
var jwtAudience = builder.Configuration["Jwt:Audience"];

var key = Encoding.UTF8.GetBytes(jwtKey!);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtIssuer,
        ValidAudience = jwtAudience,
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
});
builder.Services.AddCors(options => {
    options.AddPolicy("AllowAngular", policy => {
        policy.WithOrigins("http://localhost:4200").AllowAnyHeader().AllowAnyMethod();
    });
});
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}



app.UseCors("AllowAngular");
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();