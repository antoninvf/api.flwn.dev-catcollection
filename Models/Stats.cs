namespace CatCollectionAPI;

public class Stats
{
    public int FileCount { get; set; } = 0;
    public int ImageCount { get; set; } = 0;
    public int VideoCount { get; set; } = 0;
    public int CategoryCount { get; set; } = 0;
    public List<string> Categories { get; set; } = new List<string>();
}