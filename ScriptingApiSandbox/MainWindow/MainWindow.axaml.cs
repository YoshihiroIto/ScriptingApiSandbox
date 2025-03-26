using System;
using Avalonia;
using Avalonia.Controls;

namespace ScriptingApiSandbox.MainWindow;

public partial class MainWindow : Window
{
    private MainWindowViewModel ViewModel => DataContext as MainWindowViewModel ?? throw new InvalidOperationException();
    
    public MainWindow()
    {
        InitializeComponent();
    }

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs e)
    {
        base.OnPropertyChanged(e);

        if (e.Property == DataContextProperty)
            TextEditor.Text = ViewModel.Script;
    }

    private void TextEditor_OnTextChanged(object? sender, EventArgs e)
    {
        ViewModel.Script = TextEditor.Text;
    }
}