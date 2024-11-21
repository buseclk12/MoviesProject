namespace BLL.Models;

public class MovieModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public DateTime? ReleaseDate { get; set; }
    public decimal TotalRevenue { get; set; }
    public int DirectorId { get; set; }
    public List<int> GenreIds { get; set; } 
}
