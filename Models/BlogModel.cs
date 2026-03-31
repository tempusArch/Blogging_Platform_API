namespace BlogAPI.Models;

public class BlogModel {
    public int id {get; set;}
    public string Title {get; set;} = string.Empty;
    public string Content {get; set;} = string.Empty;
    public string? Category {get; set;} = string.Empty;
    public List<string>? Tags {get; set;} = new List<string>();
    public DateTime? CreatedAt {get; set;} = DateTime.UtcNow;
    public DateTime? UpdatedAt {get; set;} = DateTime.UtcNow;

}