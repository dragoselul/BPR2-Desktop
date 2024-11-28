using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using BPR2_Desktop.Model.Enums;

namespace BPR2_Desktop.ViewModels.MicroManagement;

public partial class ShelfDesignerViewModel: ViewModel
{
    [ObservableProperty] private List<ShelfTypes> _shelfs;
    [ObservableProperty] private List<Line> _items;
    [ObservableProperty] private ShelfTypes _selectedItem = ShelfTypes.DoubleSided;
    [ObservableProperty] private int _numberOfShelves = 0;
    [ObservableProperty] private double _distanceBetweenShelves = 0;
    [ObservableProperty] private double _heightOfShelf = 0;
    [ObservableProperty] private double _widthOfShelf = 0;
    [ObservableProperty] private double _shelveThickness = 0;
    

    public ShelfDesignerViewModel()
    {
        InitializeViewModel();
    }
    
    private void InitializeViewModel()
    {
        Shelfs = GetShelfs();
    }
    
    private List<ShelfTypes> GetShelfs()
    {

        return Enum.GetValues(typeof(ShelfTypes)).Cast<ShelfTypes>().ToList();
    }
    
    [RelayCommand]
    private void GenerateShelfLines()
    {
        Items = new();

        for (int i = 0; i < NumberOfShelves; i++)
        {
            double yPosition = i * (DistanceBetweenShelves + ShelveThickness);

            Line shelfLine = new Line
            {
                X1 = 0,
                Y1 = yPosition,
                X2 = WidthOfShelf,
                Y2 = yPosition,
                Stroke = Brushes.Black,
                StrokeThickness = ShelveThickness
            };

            Items.Add(shelfLine);
        }
    }
}