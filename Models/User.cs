namespace CSFinal.Models;

public class User
{
    public int Id { get; set; }
    public string Name { get; set; }
    public ICollection<Painting> Paintings { get; set; }
    public DateTime CreatedAt { get; set; }
}