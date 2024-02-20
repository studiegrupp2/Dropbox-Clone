using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace backend_2;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

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

        app.Run();
    }
}

[ApiController]
[Route("api/test")]
public class MyController : ControllerBase
{
    [HttpGet()]
    public string SayHello()
    {
        return "Test";
    } 
}

[Route("api/")]
public class ValuesController : ControllerBase
{
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
        foreach(var File in files) {
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