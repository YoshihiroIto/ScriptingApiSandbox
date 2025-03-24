using System;
using Avalonia.Controls;
using Avalonia.Interactivity;
using ScriptingApi;

namespace ScriptingApiSandbox.Dialog;

public partial class Dialog : Window
{
    private DialogImpl ViewModel => DataContext as DialogImpl ?? throw new InvalidOperationException();
    
    public Dialog()
    {
        InitializeComponent();
    }

    private void OkButton_OnClick(object? sender, RoutedEventArgs e)
    {
        ViewModel.DialogResult = DialogResult.Ok;
        Close(DialogResult.Ok);
    }

    private void CancelButton_OnClick(object? sender, RoutedEventArgs e)
    {
        ViewModel.DialogResult = DialogResult.Cancel;
        Close(DialogResult.Cancel);
    }
}