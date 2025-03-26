using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using ScriptingApi;

namespace ScriptingApiSandbox.Dialog.Element;

public sealed partial class DialogTabImpl : ObservableObject, IDialogTab
{
    [ObservableProperty]
    public partial string Caption { get; set; } = "";

    [ObservableProperty]
    public partial bool IsEnabled { get; set; } = true;

    public ReadOnlyObservableCollection<IDialogGroup> Pages { get; }

    private readonly ObservableCollection<IDialogGroup> _pages = new();

    public DialogTabImpl()
    {
        Pages = new(_pages);
    }

    public IDialogGroup Page(string name)
    {
        var elem = new DialogGroupImpl
        {
            Caption = name,
            Orientation = Orientation.Vertical
        };
        _pages.Add(elem);
        return elem;
    }
}