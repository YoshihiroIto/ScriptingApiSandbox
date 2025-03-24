using System;
using System.Collections.ObjectModel;
using ScriptingApi;

namespace ScriptingApiSandbox.Dialog;

using static GuiHelper;

public sealed class DialogImpl : IDialog
{
    public DialogResult DialogResult { get; internal set; } = DialogResult.Cancel;

    public event EventHandler? Closed;

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

        if (Closed is { })
            dialog.Closed += (_, _) => Closed.Invoke(this, EventArgs.Empty);

        var result = dialog.ShowDialogSync<DialogResult?>(ActiveWindow);
        return result ?? DialogResult.Cancel;
    }

    public void Show()
    {
        var dialog = new Dialog
        {
            Title = _title,
            DataContext = this
        };

        if (Closed is { })
            dialog.Closed += (_, _) => Closed.Invoke(this, EventArgs.Empty);

        dialog.Show(ActiveWindow);
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

    public IDialogBool Bool(string caption, bool initial)
    {
        var elem = new Element.DialogBoolImpl
        {
            Caption = caption,
            Value = initial,
        };
        _elements.Add(elem);
        return elem;
    }

    public IDialogInt Int(string caption, int initial, int min, int max)
    {
        var elem = new Element.DialogIntImpl
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
        var elem = new Element.DialogFloatImpl
        {
            Caption = caption,
            Value = initial,
            Min = min,
            Max = max,
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