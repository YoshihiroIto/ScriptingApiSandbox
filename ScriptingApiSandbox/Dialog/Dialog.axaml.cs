using System;
using Avalonia;
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
    
    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs e)
    {
        base.OnPropertyChanged(e);

        if (e.Property == DataContextProperty)
            ViewModel.RequireClose += CloseInternal;
    }

    private void OkButton_OnClick(object? sender, RoutedEventArgs e)
    {
        CloseInternal(DialogResult.Ok);
    }

    private void CancelButton_OnClick(object? sender, RoutedEventArgs e)
    {
        CloseInternal(DialogResult.Cancel);
    }
    
    private void CloseInternal(DialogResult result)
    {
        ViewModel.DialogResult = result;
        Close(DialogResult.Ok);
    }
}