using System;
using System.ComponentModel.DataAnnotations;

namespace AucX.Application.DTOs;

public class UserDto
{
    public int Id { get; set; }
    public required string UserName { get; set; }
    public required string Email { get; set; }
    // Дополнительно можно добавить флаг подтверждения почты или другие поля
}
