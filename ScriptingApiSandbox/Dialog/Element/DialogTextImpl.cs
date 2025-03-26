using System;
using CommunityToolkit.Mvvm.ComponentModel;
using ScriptingApi;

namespace ScriptingApiSandbox.Dialog.Element;

public sealed partial class DialogTextImpl : ObservableObject, IDialogText
{
    public event EventHandler? TextChanged;

    [ObservableProperty]
    public partial string Caption { get; set; } = "";

    [ObservableProperty]
    public partial bool IsEnabled { get; set; } = true;

    [ObservableProperty]
    public partial string Text { get; set; } = "";

    [ObservableProperty]
    public partial double Width { get; set; } = 300;

    // ReSharper disable once UnusedParameterInPartialMethod
    partial void OnTextChanged(string value)
    {
        TextChanged?.Invoke(this, EventArgs.Empty);
    }
}