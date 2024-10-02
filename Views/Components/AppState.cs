namespace BPR2_Desktop.Views.Components;

public class AppState
{
    private static AppState _instance;

    public static AppState Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new AppState();
            }
            return _instance;
        }
    }

    public string CurrentDesignFile { get; set; }

    // Private constructor to prevent instantiation from outside
    private AppState() { }
}
