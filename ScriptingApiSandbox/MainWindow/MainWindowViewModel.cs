using CommunityToolkit.Mvvm.ComponentModel;

namespace ScriptingApiSandbox.MainWindow;

public sealed partial class MainWindowViewModel : ObservableObject
{
    [ObservableProperty]
    public partial string Greeting { get; set; } = "Welcome to Avalonia!";
}