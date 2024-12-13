using System.ComponentModel;

namespace BPR2_Desktop.ViewModels;

public class InputDialogViewModel : INotifyPropertyChanged
{
    private string _responseText;

    public string Prompt { get; set; }

    public string ResponseText
    {
        get => _responseText;
        set
        {
            if (_responseText != value)
            {
                _responseText = value;
                OnPropertyChanged(nameof(ResponseText));
                OnPropertyChanged(nameof(IsInputValid)); // Update validation status
            }
        }
    }

    public bool IsInputValid => !string.IsNullOrWhiteSpace(ResponseText);

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}


