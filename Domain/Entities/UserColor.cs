using System;

namespace AucX.Domain.Entities;

public class UserColor
{
    public int Id { get; set; }
    
    // Название цвета, например, "Red" или "Pastel Pink"
    public string Name { get; set; } = null!;
    
    // Код цвета, например, "#FF0000"
    public string ColorCode { get; set; } = null!;
    
    // Цена, по которой цвет был приобретён
    public int Price { get; set; }
    
    // Категория цвета: Default, Vibrant, Pastel, Neutrals и т.д.
    public string Category { get; set; } = null!;

    // Внешний ключ для AppUser
    public string UserId { get; set; } = null!;
    public virtual AppUser User { get; set; } = null!;
}
