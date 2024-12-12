using System.Windows.Media.Imaging;
using BPR2_Desktop.Database;
using BPR2_Desktop.Model;
using BPR2_Desktop.Model.Helpers;

namespace BPR2_Desktop.ViewModels.UserControls;

public partial class ImageDisplayPanelViewModel: ViewModel
{
    private readonly ProductContext _context;
    [ObservableProperty]
    private BitmapImage _image;
    [ObservableProperty]
    private double _maxHeight = 400;
    [ObservableProperty]
    private double _maxWidth = 400;
    public ImageDisplayPanelViewModel(ProductContext context)
    {
        _context = context;
        InitializeViewModel();
    }

    private void InitializeViewModel()
    {
        ClearImage();
    }
    
    public void ClearImage()
    {
        Image = new BitmapImage();
    }
    
    public async Task LoadImage(string mainEan)
    {
        try
        {
            // Fetch image asynchronously - this will already run on a background thread
            ProductImage? image = await _context.GetImageByEAN(mainEan);
            
            // Use Dispatcher to update UI-bound collection on the main thread
            Application.Current.Dispatcher.Invoke(() =>
            {
                Image = image != null 
                    ? ImageConverter.ConvertBlobToImage(image.Picture_Blob)
                    : ImageConverter.LoadDefaultPicture();
            });
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }
}