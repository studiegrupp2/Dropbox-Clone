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

        builder.Services.AddAuthentication().AddBearerToken(IdentityConstants.BearerScheme);
        builder.Services.AddIdentityCore<User>().AddEntityFrameworkStores<DatabaseContext>().AddApiEndpoints();
        
        var app = builder.Build();


        // // Configure the HTTP request pipeline.
        // if (app.Environment.IsDevelopment())
        // {
        //     app.UseSwagger();
        //     app.UseSwaggerUI();
        // }

        // app.UseHttpsRedirection();
        app.MapIdentityApi<User>();
        app.MapControllers();
        app.UseAuthentication();
        app.UseAuthorization();
        
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

public class AppFileDto
{
    public int Id { get; set; }
    public string FileName { get; set; }
    public byte[] Content { get; set; }

    public AppFileDto(AppFile appFile)
    {

        this.FileName = appFile.FileName;
        this.Content = appFile.Content;
        this.Id = appFile.Id;
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

    [HttpPost("UploadFile")]
    [Authorize]
    public IActionResult UploadFile([FromForm] IFormFile file)
    {
        var user = context.Users.Find(User.FindFirstValue(ClaimTypes.NameIdentifier));
        if (user == null)
        {
            return NotFound();
        }
        
        using (var memoryStream = new MemoryStream())
        {
            file.CopyToAsync(memoryStream);
            byte[] bytearr = memoryStream.ToArray();
            AppFile _file = new AppFile(file.FileName, bytearr);

            context.AppFiles.Add(_file);
            context.SaveChanges();

            AppFileDto output = new AppFileDto(_file);
            //return Ok(output);
            return CreatedAtAction(nameof(UploadFile), output);
        }
    }

    [HttpGet("GetAllFiles")]
    public List<AppFileDto> GetAllFiles()
    {
        var list = context.AppFiles.ToList().Select(appFile => new AppFileDto(appFile)).ToList();
        return list;
    }

    [HttpGet("DownloadFile/{id}")]
    public IActionResult Download(int id)
    {
        var file = context.AppFiles.FirstOrDefault(files => files.Id == id);

        if (file == null)
        {
            return NotFound();
        }
        return File(file.Content, "application/octet-stream", file.FileName);
    }
}