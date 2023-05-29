using Microsoft.EntityFrameworkCore;
using Project.Data;
using Project.Validation;
using AutoMapper;
using FluentValidation;
using MediatR;
using System.Reflection;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Project.UseCases;
using Project.UseCases.Tokens;
using Project.RSA;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpContextAccessor();
builder.Services.AddTransient<IUserAccessor, UserAccessor>();
builder.Services.AddTransient<IGeneralRepository, GeneralRepository>();
builder.Services.AddTransient<ITokenRepository, TokenRepository>();
// Add services to the container.
builder.Services.AddMediatR(Assembly.GetExecutingAssembly());
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();
builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());
//builder.Services.AddMediatR(typeof(Program).GetTypeInfo().Assembly);
//builder.Services.AddMediatR(typeof(MyHandler));
builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
builder.Services.AddTransient<ExceptionHandlingMiddleware>();
//builder.Services.AddCustomRequestValidation();
builder.Services.AddScoped<IRsaService, RsaService>();
// builder.Services.AddEndpointsApiExplorer();
// builder.Services.AddSwaggerGen();

builder.Services.AddAuthentication("Bearer")
        .AddJwtBearer("Bearer", options =>
        {
            //options.RequireHttpsMetadata = Configuration.GetValue<bool>("IDENTITY_REQUIREHTTPSMETADATA");
            //options.Authority = Configuration.GetValue<string>("IDENTITY_AUTHORITY");
            //options.Audience = Configuration.GetValue<string>("IDENTITY_AUDIENCE");
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = builder.Configuration["Jwt:Issuer"],
                ValidAudience = builder.Configuration["Jwt:Issuer"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
            };
        });

builder.Services.AddCors(o => o.AddPolicy("AllowAnyCorsPolicy", builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            }));

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddDbContext<DataContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("IU_BT_DB")));
}

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    // app.UseDeveloperExceptionPage();
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseCors("AllowAnyCorsPolicy");
app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseMiddleware<TokenMiddleware>();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// app.UseSwagger();
// app.UseSwaggerUI();

app.Run();
