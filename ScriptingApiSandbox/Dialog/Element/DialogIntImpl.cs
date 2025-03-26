using System;
using CommunityToolkit.Mvvm.ComponentModel;
using ScriptingApi;

namespace ScriptingApiSandbox.Dialog.Element;

public sealed partial class DialogIntImpl : ObservableObject, IDialogInt
{
    public event EventHandler? ValueChanged;

    [ObservableProperty]
    public partial string Caption { get; set; } = "";

    [ObservableProperty]
    public partial bool IsEnabled { get; set; } = true;

    [ObservableProperty]
    public partial int Value { get; set; }

    [ObservableProperty]
    public partial int Min { get; set; }

    [ObservableProperty]
    public partial int Max { get; set; }

    [ObservableProperty]
    public partial double Width { get; set; } = 300;

    // ReSharper disable once UnusedParameterInPartialMethod
    partial void OnValueChanged(int value)
    {
        ValueChanged?.Invoke(this, EventArgs.Empty);
    }
}