using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;

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
        // builder.Services.AddAuthentication().AddBearerToken().
        // // Configure the HTTP request pipeline.
        // if (app.Environment.IsDevelopment())
        // {
        //     app.UseSwagger();
        //     app.UseSwaggerUI();
        // }

        // app.UseHttpsRedirection();

        app.MapControllers();

        app.Run();
    }
}

public class User : IdentityUser
{
    public List<AppFile> AppFiles = new List<AppFile>();
}
public class AppFile
{
    public int Id { get; set; }
    public string FileName { get; set; }
    public byte[] Content { get; set; }
    public User User { get; set; } = null;

    public AppFile() { }

    public AppFile(string filename, byte[] content)
    {
        this.FileName = filename;
        this.Content = content;
    }
}

public class DatabaseContext : IdentityDbContext<User>
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
    //private static List<IFormFile> _files = new List<IFormFile>(); //Denna ska bytas ut till en databas

    // [HttpPost("UploadFile")]
    // public IActionResult UploadFile([FromForm] IFormFile file)
    // {
    //     // IFormFile formFile = file; //new FormFile();

    //     AppFile _file = new AppFile(file.FileName)
    //     // context.AppFiles.Add(formFile); //AppFiles<Appfile> 
    //     context.AppFiles.Add(file);
    //     context.SaveChanges();
    //     return Ok(file.Length);
    // }


    [HttpPost("UploadFile")]
    public IActionResult UploadFile([FromForm] IFormFile file)
    {
        using (var memoryStream = new MemoryStream())
        {
            file.CopyToAsync(memoryStream);

            byte[] bytearr = memoryStream.ToArray();

            AppFile _file = new AppFile(file.FileName, bytearr);
            //IFormFile formFile = file;

            // IFormFile _file = new AppFile(file.FileName, file.Length);
            context.AppFiles.Add(_file);
            context.SaveChanges();
            //new FormFile();
            //_files.Add(formFile);
            return Ok(new AppFile(_file.FileName, _file.Content));
        }
    }



    // [HttpPost("UploadFiles")]
    // public IActionResult UploadFiles(List<IFormFile> files)
    // {
    //     List<IFormFile> formFile = files; //new FormFile();
    //     foreach (var File in files)
    //     {
    //         _files.Add(File);
    //     }
    //     return Ok(files.Sum(file => file.Length));
    // }
    // //=> Ok(files.Sum(file => file.Length));


    [HttpGet("GetAllFiles")]
    public List<AppFile> GetAllFiles()
    {
        var list = context.AppFiles.ToList();
        return list;
    }

    // [HttpGet("DownloadFile/{id}")]
    // public IActionResult DownloadFile(int id)
    // {
    //     var file = context.AppFiles.FirstOrDefault(f => f.Id == id);

    //     if (file == null)
    //     {
    //         return NotFound();
    //     }
    //     return File(file.Content, "application/octet-stream");
    // }
    [HttpGet("DownloadFile/{id}")]
    public IActionResult Download(int id)
    {
        // byte[] bytes;
        // string fileName, contentType;

        var file = context.AppFiles.FirstOrDefault(files => files.Id == id);

        if (file == null)
        {
            //fileName = file.fileName;
            // bytes = file.Content;

            return NotFound();
        }

        return File(file.Content, "application/octet-stream", file.FileName);
    }
}