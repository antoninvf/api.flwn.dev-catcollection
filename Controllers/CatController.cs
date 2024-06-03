using System.Globalization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace CatCollectionAPI.Controllers;

[EnableCors("flwn")]
[ApiController]
[Route("")]
public class CatController : ControllerBase
{
    private readonly ILogger<CatController> _logger;

    public CatController(ILogger<CatController> logger)
    {
        _logger = logger;
    }
    
    private string path = "/mnt/dino/cat";

    private List<CatFile> getFiles()
    {
        if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development") path = "Y:/cat";

        // recursively get all files in the directory and subdirectories
        var files = Directory.GetFiles(path, "*", SearchOption.AllDirectories);
        var fileList = new List<CatFile>();
        foreach (var e in files)
        {
            var file = new CatFile();
            file.FileName = Path.GetFileName(e);
            file.Category = Path.GetFileName(Path.GetDirectoryName(e)) ?? "/";
            file.Link = $"https://cat.basil.florist/{Uri.EscapeDataString(file.Category)}/{Uri.EscapeDataString(file.FileName)}";
            fileList.Add(file);
        }
        
        return fileList;
    }

    [HttpGet]
    public ActionResult<IEnumerable<CatFile>> GetAll()
    {
        return getFiles();
    }
    
    [HttpGet("random")]
    public ActionResult<CatFile> GetRandom()
    {
        var files = getFiles();
        var random = new Random();
        return files[random.Next(files.Count)];
    }
    
    [HttpGet("visitrandom")]
    public ActionResult GetVisitRandom()
    {
        var files = getFiles();
        var random = new Random();
        return Redirect(files[random.Next(files.Count)].Link);
    }
    
    [HttpGet("cats/{category}")]
    public ActionResult<IEnumerable<CatFile>> GetVersions(string category)
    {
        return getFiles().Where(x => Uri.EscapeDataString(x.Category.ToLower()).Equals(Uri.EscapeDataString(category.ToLower()))).ToList();
    }

    [HttpGet("categories")]
    public ActionResult<IEnumerable<string>> GetModpacks()
    {
        if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development") path = "Y:/cat";

        // get all folders in the directory
        var folders = Directory.GetDirectories(path).Select(Path.GetFileName).ToList();

        return folders!;
    }
}