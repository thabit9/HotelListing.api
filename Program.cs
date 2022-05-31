//using System.Configuration;
using HotelListing.api;
using HotelListing.api.Configurations;
using HotelListing.api.Data.EFContext;
using HotelListing.api.IRepository;
using HotelListing.api.Repository;
using HotelListing.api.Sevices;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Serialization;
using Serilog;
using Serilog.Events;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
//Register the Database context
builder.Services.AddDbContext<HotelContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("HotelConn")));

#region Serilog method 1
var logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext() 
    .CreateLogger();
builder.Logging.ClearProviders();
builder.Logging.AddSerilog(logger); 
#endregion
#region Serilog method 2
/*Log.Logger = new LoggerConfiguration()
    .WriteTo.File(
        path: "../logs/webapi-.log",
        outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} {CorrelationId} [{Level: u3}] {Username} {Message:lj} {NewLine} {Exception}",
        rollingInterval: RollingInterval.Day,
        restrictedToMinimumLevel: LogEventLevel.Information
    ).CreateLogger();*/
#endregion

#region  Newtonsoft Json Config Method 1
/*builder.Services.AddControllers().AddNewtonsoftJson(s => {
    s.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
});*/
#endregion
#region  Newtonsoft Json Config Method 2
builder.Services.AddControllers().AddNewtonsoftJson(s => {
    s.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
});
#endregion

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme {
        Description = @"JWT Authorization header using the Bearer scheme.
        Enter  'Bearer' [space] and then your token in the text input belo.
        Example: 'Bearer 12345abcdef '",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement(){
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference{
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "0auth2",
                Name = "Bearer",
                In = ParameterLocation.Header
            },
            new List<string>()
        }
    });
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Hotel Listing Api",
        Version = "v1",
        Description = "Service to support the Hotel Listing sample App/Site",
        TermsOfService = new Uri("https://example.com/terms"),
        License = new OpenApiLicense
        {
            Name = "Freeware",
            //Url= "https://en.wikipedia.org/wiki/Freeware"
            Url = new Uri ("http://localhost:5032/LICENSE.txt")
        }
    });
    /*var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);*/
});

Configurationx config = new Configurationx();
//Microsoft Identity
builder.Services.AddAuthentication();
builder.Services.ConfigureIdentity();
builder.Services.ConfigureJWT(config.Configuration!);

// http://docs.asp.net/en/latest/security/cors.html
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowAnyOrigin();
        //.AllowCredentials();
    });
});
// Add Automapper service
builder.Services.AddAutoMapper(typeof(MapperInitializer));
builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IAuthManager, AuthManager>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger(c => {
        c.RouteTemplate = "HotelListing Api v1/swagger/{documentName}/swagger.json";
    });
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/HotelListing Api v1/swagger/v1/swagger.json", "HotelListing Api v1");
    });
}

app.ConfigureExceptionHandler();
app.UseHttpsRedirection();

app.UseRouting();
app.UseStaticFiles();
app.UseCors("AllowAll");      
app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    /*endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");*/
    endpoints.MapControllers();
});
//app.MapControllers();

app.Run();
