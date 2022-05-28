using HotelListing.api.Configurations;
using HotelListing.api.Data.EFContext;
using HotelListing.api.IRepository;
using HotelListing.api.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Serialization;
using Serilog;
//using Serilog.Events;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
//Register the Database context
builder.Services.AddDbContext<HotelContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("HotelConn")));

//serilog method 1
var logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext() 
    .CreateLogger();
builder.Logging.ClearProviders();
builder.Logging.AddSerilog(logger); 
//Serilog method 2
/*Log.Logger = new LoggerConfiguration()
    .WriteTo.File(
        path: "../logs/webapi-.log",
        outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} {CorrelationId} [{Level: u3}] {Username} {Message:lj} {NewLine} {Exception}",
        rollingInterval: RollingInterval.Day,
        restrictedToMinimumLevel: LogEventLevel.Information
    ).CreateLogger();*/

//method 1
/*builder.Services.AddControllers().AddNewtonsoftJson(s => {
    s.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
});*/
//method 2
builder.Services.AddControllers().AddNewtonsoftJson(s => {
    s.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
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
