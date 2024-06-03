namespace CatCollectionAPI;

public class CatFile
{
    public string FileName { get; set; }
    public string Link { get; set; }
    public string Category { get; set; }
    
    public CatFile()
    {
        FileName = "";
        Link = "";
        Category = "";
    }
}