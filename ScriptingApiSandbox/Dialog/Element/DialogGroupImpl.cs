using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using ScriptingApi;

namespace ScriptingApiSandbox.Dialog.Element;

public sealed partial class DialogGroupImpl : ObservableObject, IDialogGroup
{
    [ObservableProperty]
    public partial string Caption { get; set; } = "";

    [ObservableProperty]
    public partial bool IsEnabled { get; set; } = true;

    [ObservableProperty]
    public partial Orientation Orientation { get; set; }

    public Avalonia.Layout.Orientation AvaloniaOrientation => Orientation == Orientation.Horizontal
        ? Avalonia.Layout.Orientation.Horizontal
        : Avalonia.Layout.Orientation.Vertical;

    public ReadOnlyObservableCollection<IDialogElement> Elements { get; }

    private readonly ObservableCollection<IDialogElement> _elements = new();

    public DialogGroupImpl()
    {
        Elements = new(_elements);
    }

    public IDialogText Text(string caption, string initial)
    {
        var elem = new DialogTextImpl
        {
            Caption = caption,
            Text = initial,
        };
        _elements.Add(elem);
        return elem;
    }

    public IDialogBool Bool(string caption, bool initial)
    {
        var elem = new DialogBoolImpl
        {
            Caption = caption,
            Value = initial,
        };
        _elements.Add(elem);
        return elem;
    }

    public IDialogInt Int(string caption, int initial, int min, int max)
    {
        var elem = new DialogIntImpl
        {
            Caption = caption,
            Value = initial,
            Min = min,
            Max = max,
        };
        _elements.Add(elem);
        return elem;
    }

    public IDialogFloat Float(string caption, double initial, double min, double max)
    {
        var elem = new DialogFloatImpl
        {
            Caption = caption,
            Value = initial,
            Min = min,
            Max = max,
        };
        _elements.Add(elem);
        return elem;
    }

    public IDialogChoice Choice(params string[] items)
    {
        var elem = new DialogChoiceImpl(items);
        _elements.Add(elem);
        return elem;
    }

    public IDialogButton Button(string caption)
    {
        var elem = new DialogButtonImpl
        {
            Caption = caption
        };
        _elements.Add(elem);
        return elem;
    }

    public IDialogLabel Label(string caption)
    {
        var elem = new DialogLabelImpl
        {
            Caption = caption
        };
        _elements.Add(elem);
        return elem;
    }

    public IDialogGroup Group(Orientation orientation)
    {
        var elem = new DialogGroupImpl
        {
            Orientation = orientation
        };
        _elements.Add(elem);
        return elem;
    }

    public IDialogTab Tab()
    {
        var elem = new DialogTabImpl();
        _elements.Add(elem);
        return elem;
    }
}