using System;

namespace AucX.Domain.Entities;

public class GameItem
{
    public int Id { get; set; }
    public required string Name { get; set; }           // Название
    public required string Description { get; set; }    // Описание
    public required string ImageUrl { get; set; }       // Ссылка на изображение
}