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
            file.Link = $"https://cat.basil.florist/browse/{Uri.EscapeDataString(file.Category)}/{Uri.EscapeDataString(file.FileName)}";
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
        // all files except Thumbs.db
        var files = getFiles().Where(x => !x.FileName.Equals("Thumbs.db")).ToList();
        var categoryCount = files.Select(x => x.Category).Distinct().Count();
        var random = new Random();

        while (true)
        {
            var randomFile = files[random.Next(files.Count)];

            if (randomFile.Category == ".secret")
            {
                if (random.Next(0, categoryCount) == 0)
                {
                    return randomFile;
                }
            }
            else
            {
                return randomFile;
            }
        }
    }

    [HttpGet("visitrandom")]
    public ActionResult GetVisitRandom()
    {
        // all files except Thumbs.db
        var files = getFiles().Where(x => !x.FileName.Equals("Thumbs.db")).ToList();
        var categoryCount = files.Select(x => x.Category).Distinct().Count();
        var random = new Random();

        while (true)
        {
            var randomFile = files[random.Next(files.Count)];

            if (randomFile.Category == ".secret")
            {
                if (random.Next(0, categoryCount) == 0)
                {
                    return Redirect(randomFile.Link);
                }
            }
            else
            {
                return Redirect(randomFile.Link);
            }
        }
    }

    [HttpGet("cats/{category}")]
    public ActionResult<IEnumerable<CatFile>> GetVersions(string category)
    {
        return getFiles().Where(x => Uri.EscapeDataString(x.Category.ToLower()).Equals(Uri.EscapeDataString(category.ToLower()))).ToList();
    }

    // get it like cats short for categories
    [HttpGet("cats")]
    public ActionResult<IEnumerable<string>> GetModpacks()
    {
        if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development") path = "Y:/cat";

        // get all folders in the directory
        var folders = Directory.GetDirectories(path).Select(Path.GetFileName).ToList();

        return folders!;
    }

    // stats endpoints
    [HttpGet("stats")]
    public ActionResult<Stats> GetStats()
    {
        var files = getFiles();
        var stats = new Stats();
        stats.FileCount = files.Count;

        // Image extensions
        stats.ImageCount = files.Count(x => x.FileName.EndsWith(".png"));
        stats.ImageCount += files.Count(x => x.FileName.EndsWith(".jpg"));
        stats.ImageCount += files.Count(x => x.FileName.EndsWith(".jpeg"));
        stats.ImageCount += files.Count(x => x.FileName.EndsWith(".gif"));
        stats.ImageCount += files.Count(x => x.FileName.EndsWith(".webp"));

        // Video extensions
        stats.VideoCount = files.Count(x => x.FileName.EndsWith(".mp4"));
        stats.VideoCount += files.Count(x => x.FileName.EndsWith(".webm"));
        stats.VideoCount += files.Count(x => x.FileName.EndsWith(".mov"));
        stats.CategoryCount = files.Select(x => x.Category).Distinct().Count();
        stats.Categories = files.Select(x => x.Category).Distinct().ToList();
        return stats;
    }

    [HttpGet("stats/{category}")]
    public ActionResult<Stats> GetStats(string category)
    {
        var files = getFiles().Where(x => Uri.EscapeDataString(x.Category.ToLower()).Equals(Uri.EscapeDataString(category.ToLower()))).ToList();
        var stats = new Stats();
        stats.FileCount = files.Count;

        // Image extensions
        stats.ImageCount = files.Count(x => x.FileName.EndsWith(".png"));
        stats.ImageCount += files.Count(x => x.FileName.EndsWith(".jpg"));
        stats.ImageCount += files.Count(x => x.FileName.EndsWith(".jpeg"));
        stats.ImageCount += files.Count(x => x.FileName.EndsWith(".gif"));
        stats.ImageCount += files.Count(x => x.FileName.EndsWith(".webp"));

        // Video extensions
        stats.VideoCount = files.Count(x => x.FileName.EndsWith(".mp4"));
        stats.VideoCount += files.Count(x => x.FileName.EndsWith(".webm"));
        stats.VideoCount += files.Count(x => x.FileName.EndsWith(".mov"));
        return stats;
    }
}