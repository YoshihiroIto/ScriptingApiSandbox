using CommunityToolkit.Mvvm.ComponentModel;
using ScriptingApi;

namespace ScriptingApiSandbox.Dialog.Element;

public sealed partial class DialogLabelImpl : ObservableObject, IDialogLabel
{
    [ObservableProperty]
    public partial string Caption { get; set; } = "";

    [ObservableProperty]
    public partial bool IsEnabled { get; set; } = true;
}