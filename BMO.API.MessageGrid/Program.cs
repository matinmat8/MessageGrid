using BMO.API.Core.Configuration;
using BMO.API.Infrastructure.Data;
using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using BMO.API.Infrastructure.IoC;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container using your custom DependencyContainer
builder.Services.AddServices(builder.Configuration);

// Register controllers and Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
});

// Register DbContext
// builder.Services.AddDbContext<ApplicationDbContext>(options =>
//     options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.Configure<OracleSetting>(builder.Configuration.GetSection("OracleSettings"));

var oracleSettings = builder.Configuration.GetSection("OracleSettings").Get<OracleSetting>();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
options.UseOracle(oracleSettings!.ConnectionString));


var app = builder.Build();

// Configure the HTTP request pipelinepipeline
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
    });
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.MapControllers();
app.Run();
