using Microsoft.EntityFrameworkCore;
using ViventiumTest;
using ViventiumTest.Models;


WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

/*
builder.Services.AddDbContext<ViventiumTestDbContext>(opt =>
    opt.UseInMemoryDatabase("ViventiumTestDb"));
*/
builder.Services.AddDbContext<ViventiumTestDbContext>(opt =>
    opt.UseSqlite("Data Source=ViventiumTest.db"));


builder.Services.AddControllers(o=>o.InputFormatters.Insert(0, new TextMediaTypeFormatter()))  //Add support to "text/plain"
    .AddJsonOptions(opts => opts.JsonSerializerOptions.PropertyNamingPolicy = null);           //preserver capitalization, preventing camelCase


WebApplication app = builder.Build();


app.UseHttpsRedirection();
app.MapControllers();

app.Run();
