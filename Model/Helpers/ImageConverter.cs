using System.IO;
using System.Windows.Media.Imaging;

namespace BPR2_Desktop.Model.Helpers;

public static class ImageConverter
{
    private static BitmapImage? _defaultImage = null;
    
    public static string GetImageType(byte[] blob)
    {
        if (blob.Length >= 8 &&
            blob[0] == 0x89 && blob[1] == 0x50 && blob[2] == 0x4E &&
            blob[3] == 0x47 && blob[4] == 0x0D && blob[5] == 0x0A &&
            blob[6] == 0x1A && blob[7] == 0x0A)
        {
            return "PNG";
        }
        if (blob.Length >= 2 &&
            blob[0] == 0xFF && blob[1] == 0xD8 &&
            blob[^2] == 0xFF && blob[^1] == 0xD9)
        {
            return "JPEG";
        }
        return "Unknown";
    }
    
    public static BitmapImage ConvertBlobToImage(byte[]? blob)
    {
        using (var stream = new MemoryStream(blob))
        {
            var image = new BitmapImage();
            image.BeginInit();
            image.CacheOption = BitmapCacheOption.OnLoad; // Load the image immediately
            image.StreamSource = stream;
            image.EndInit();
            image.Freeze(); // Freeze to make it thread-safe for WPF
            return image;
        }
    }
    
    public static BitmapImage LoadDefaultPicture()
    {
        try
        {
            if (_defaultImage != null)
            {
                return _defaultImage;
            }
            var defaultPicturePath = Path.Combine(AppContext.BaseDirectory, "../../../", "Pictures/default-image.jpg");

            if (!File.Exists(defaultPicturePath))
            {
                throw new FileNotFoundException($"Default image not found at path: {defaultPicturePath}");
            }

            var bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri(defaultPicturePath, UriKind.RelativeOrAbsolute);
            bitmap.CacheOption = BitmapCacheOption.OnLoad; // Ensure it loads immediately
            bitmap.EndInit();
            bitmap.Freeze(); // Make the BitmapImage thread-safe
            _defaultImage = bitmap;
            return bitmap;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading default image: {ex.Message}");
            return new BitmapImage();
        }
    }
}