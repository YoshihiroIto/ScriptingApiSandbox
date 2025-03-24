using System;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ScriptingApi;

namespace ScriptingApiSandbox.Dialog.Element;

public sealed partial class DialogButtonImpl : ObservableObject, IDialogButton
{
    [ObservableProperty]
    public partial string Caption { get; set; } = "";
    
    [ObservableProperty]
    public partial bool IsEnabled { get; set; } = true;

    public event EventHandler? Clicked;
    
    [RelayCommand]
    private void Click()
    {
        Clicked?.Invoke(this, EventArgs.Empty);
    }
}
