namespace CSFinal.Models;

public class Painting
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Pixels { get; set; }
    public int UserId { get; set; }
    public User? User { get; set; }
    public DateTime CreatedAt { get; set; }
}