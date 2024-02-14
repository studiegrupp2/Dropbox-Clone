// using Microsoft.AspNetCore.Mvc;

// namespace Backend;

// public class CreateDropDto
// {
//     public string Name { get; set; } = "";
//     public string Description { get; set; } = "";
// }

// [ApiController]
// [Route("api/course")]
// public class CourseController : ControllerBase
// {
//     private static List<Files> files = new List<Files>();

//     [HttpPost("UploadFile")]
 //      public DropResult UploadFile(IFormFile file)
 //     => Ok(file.Length);

//     [HttpGet]
//     public List<Files> GetAllFiles()
//     {
//         return files;
//     }
// }