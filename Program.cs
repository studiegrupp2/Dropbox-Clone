using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backend_2;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddDbContext<DatabaseContext>(options =>
        {
            options.UseNpgsql(
                "Host=localhost;Database=dropbox;Username=postgres;Password=password"
            );
        });
 
        builder.Services.AddControllers();

        // // Add services to the container.
        // builder.Services.AddAuthorization();

        // // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        // builder.Services.AddEndpointsApiExplorer();
        // builder.Services.AddSwaggerGen();

        var app = builder.Build();

        // // Configure the HTTP request pipeline.
        // if (app.Environment.IsDevelopment())
        // {
        //     app.UseSwagger();
        //     app.UseSwaggerUI();
        // }

        // app.UseHttpsRedirection();

        app.MapControllers();

        //app.Run();
    }
}

public class AppFile
{
    public int Id { get; set; }
    public byte[] Content { get; set; }

    public AppFile() { }

    public AppFile(byte[] content)
    {
        this.Content = content;
    }
}

public class DatabaseContext : DbContext
{
    public DbSet<AppFile> AppFiles { get; set; }

    public DatabaseContext(DbContextOptions<DatabaseContext> options)
        : base(options) { }

}

[ApiController]

[Route("api/")]
public class ValuesController : ControllerBase
{
    DatabaseContext context;

    public ValuesController(DatabaseContext context)
    {
        this.context = context;
    }
    private static List<IFormFile> _files = new List<IFormFile>(); //Denna ska bytas ut till en databas
    [HttpPost("UploadFile")]
    public IActionResult UploadFile([FromForm] IFormFile file)
    {
        IFormFile formFile = file; //new FormFile();
        _files.Add(formFile);
        return Ok(file.Length);
    }

    [HttpPost("UploadFiles")]
    public IActionResult UploadFiles(List<IFormFile> files)
    {
        List<IFormFile> formFile = files; //new FormFile();
        foreach (var File in files)
        {
            _files.Add(File);
        }
        return Ok(files.Sum(file => file.Length));
    }
    //=> Ok(files.Sum(file => file.Length));


    [HttpGet("GetAllFiles")]
    public List<IFormFile> GetAllFiles()
    {
        return _files;
    }
}