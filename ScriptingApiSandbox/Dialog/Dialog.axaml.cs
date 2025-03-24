using Avalonia.Controls;
using Avalonia.Interactivity;
using ScriptingApi;

namespace ScriptingApiSandbox.Dialog;

public partial class Dialog : Window
{
    public Dialog()
    {
        InitializeComponent();
    }

    private void OkButton_OnClick(object? sender, RoutedEventArgs e)
    {
        Close(DialogResult.Ok);
    }

    private void CancelButton_OnClick(object? sender, RoutedEventArgs e)
    {
        Close(DialogResult.Cancel);
    }
}