using System.Collections.ObjectModel;
using ScriptingApi;

namespace ScriptingApiSandbox.Dialog;

using static GuiHelper;

public sealed class DialogImpl : IDialog
{
    public ReadOnlyObservableCollection<IDialogElement> Elements { get; }

    private readonly ObservableCollection<IDialogElement> _elements = new();
    private readonly string _title;

    public DialogImpl(string title)
    {
        _title = title;
        Elements = new(_elements);
    }

    public DialogResult ShowModal()
    {
        var dialog = new Dialog
        {
            Title = _title,
            DataContext = this
        };

        var result = dialog.ShowDialogSync<DialogResult?>(ActiveWindow);
        return result ?? DialogResult.Cancel;
    }

    public IDialogText Text(string caption, string initial)
    {
        var elem = new Element.DialogTextImpl
        {
            Caption = caption,
            Text = initial,
        };

        _elements.Add(elem);
        return elem;
    }

    public IDialogButton Button(string caption)
    {
        var elem = new Element.DialogButtonImpl
        {
            Caption = caption
        };
        _elements.Add(elem);
        return elem;
    }
}