// DTO/CanvasDto.cs
namespace AucX.WebUI.DTO
{
    public class CanvasCreateDto
    {
        public string Name { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public string[] Colors { get; set; }
        public string[] PixelData { get; set; }
    }

    public class CanvasSettingsDto
    {
        public int InitialWidth { get; set; }
        public int InitialHeight { get; set; }
        public int MaxWidth { get; set; }
        public int MaxHeight { get; set; }
        public List<string> DefaultColors { get; set; }
    }

    public class AvailableColorsDto
    {
        public List<string> Purchased { get; set; }
        public List<string> Default { get; set; }
    }
}