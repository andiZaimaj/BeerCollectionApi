using System.ComponentModel.DataAnnotations;

public class Beer
{
    public int Id { get; set; }

    [Required]
    public string Name { get; set; }

    [Required]
    public string Type { get; set; }

    public List<double> Ratings { get; set; } = new List<double>();

    public double AverageRating => Ratings.Count == 0 ? 0 : Ratings.Average();
}
