using System;
using CommunityToolkit.Mvvm.ComponentModel;
using ScriptingApi;

namespace ScriptingApiSandbox.Dialog.Element;

public sealed partial class DialogFloatImpl : ObservableObject, IDialogFloat
{
    public event EventHandler? ValueChanged;

    [ObservableProperty]
    public partial string Caption { get; set; } = "";

    [ObservableProperty]
    public partial bool IsEnabled { get; set; } = true;

    [ObservableProperty]
    public partial double Value { get; set; }

    [ObservableProperty]
    public partial double Min { get; set; }

    [ObservableProperty]
    public partial double Max { get; set; }

    [ObservableProperty]
    public partial double Width { get; set; } = 300;

    partial void OnValueChanged(double value)
    {
        ValueChanged?.Invoke(this, EventArgs.Empty);
    }
}