using CommunityToolkit.Mvvm.ComponentModel;
using ScriptingApi;

namespace ScriptingApiSandbox.Dialog.Element;

public sealed partial class DialogTextImpl : ObservableObject, IDialogText
{
    [ObservableProperty]
    public partial string Caption { get; set; } = "";
    
    [ObservableProperty]
    public partial bool IsEnabled { get; set; } = true;

    [ObservableProperty]
    public partial string Text { get; set; } = "";
    
    [ObservableProperty]
    public partial double Width { get; set; } = 300;
}