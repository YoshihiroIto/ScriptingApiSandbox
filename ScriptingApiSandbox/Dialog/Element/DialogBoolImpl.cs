using System;
using CommunityToolkit.Mvvm.ComponentModel;
using ScriptingApi;

namespace ScriptingApiSandbox.Dialog.Element;

public sealed partial class DialogBoolImpl : ObservableObject, IDialogBool
{
    public event EventHandler? ValueChanged;

    [ObservableProperty]
    public partial string Caption { get; set; } = "";

    [ObservableProperty]
    public partial bool IsEnabled { get; set; } = true;

    [ObservableProperty]
    public partial bool Value { get; set; }

    partial void OnValueChanged(bool value)
    {
        ValueChanged?.Invoke(this, EventArgs.Empty);
    }
}