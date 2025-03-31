﻿namespace AucX.Domain.Entities;

public class User
{
    public int Id { get; set; }
    public required string UserName { get; set; }
    public required string Email { get; set; }
    public required string PasswordHash { get; set; }
    public required DateTime RegisteredAt { get; set; }
}
