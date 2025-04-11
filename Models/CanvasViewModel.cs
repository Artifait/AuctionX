using System;
using System.ComponentModel.DataAnnotations;

namespace AucX.WebUI.Models;

public class CanvasViewModel
{
    [Required(ErrorMessage = "Введите название холста")]
    [StringLength(100, ErrorMessage = "Максимум 100 символов")]
    public string Name { get; set; }

    [Required(ErrorMessage = "Укажите ширину")]
    [Range(1, 256, ErrorMessage = "Ширина 1-64")]
    public int Width { get; set; } = 1;

    [Required(ErrorMessage = "Укажите высоту")]
    [Range(1, 256, ErrorMessage = "Высота 1-64")]
    public int Height { get; set; } = 1;
    public string PixelData;
    public int MaxWidth { get; set; } = 64;
    public int MaxHeight { get; set; } = 64;
    public List<string> AvailableColors { get; set; } = new();
    public string? ErrorMessage { get; set; }
}