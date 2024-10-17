﻿namespace HC_7572.Domain.Objects;

public sealed class Location
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public ICollection<User> Users { get; set; } = new List<User>();
}