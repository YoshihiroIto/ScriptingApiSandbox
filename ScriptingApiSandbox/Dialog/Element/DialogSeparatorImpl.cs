using CommunityToolkit.Mvvm.ComponentModel;
using ScriptingApi;

namespace ScriptingApiSandbox.Dialog.Element;

public sealed partial class DialogSeparatorImpl : ObservableObject, IDialogSeparator
{
    [ObservableProperty]
    public partial string Caption { get; set; } = "";

    [ObservableProperty]
    public partial bool IsEnabled { get; set; } = true;
    
    [ObservableProperty]
    public partial Orientation Orientation { get; set; }
}