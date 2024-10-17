namespace HC_7572.Domain.Objects;

public sealed class User
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public ICollection<Location> Locations { get; set; } = new List<Location>();
}