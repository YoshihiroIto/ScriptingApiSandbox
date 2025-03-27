using System;
using System.Collections.ObjectModel;
using ScriptingApi;
using ScriptingApiSandbox.Dialog.Element;

namespace ScriptingApiSandbox.Dialog;

using static GuiHelper;

public sealed class DialogImpl(string title, Orientation orientation = Orientation.Vertical) : IDialog
{
    public DialogResult DialogResult { get; internal set; } = DialogResult.Cancel;

    public event EventHandler? Closed;

    internal ReadOnlyObservableCollection<IDialogElement> Elements => _group.Elements;
    internal Avalonia.Layout.Orientation AvaloniaOrientation => _group.AvaloniaOrientation;

    internal Action<DialogResult>? RequireClose { get; set; }

    private readonly DialogGroupImpl _group = new()
    {
        Orientation = orientation
    };

    public DialogResult ShowModal()
    {
        var dialog = new Dialog
        {
            Title = title,
            DataContext = this
        };

        dialog.Closed += (_, _) => Closed?.Invoke(this, EventArgs.Empty);

        var result = dialog.ShowDialogSync<DialogResult?>(ActiveWindow);
        return result ?? DialogResult.Cancel;
    }

    public void Show()
    {
        var dialog = new Dialog
        {
            Title = title,
            DataContext = this
        };

        dialog.Closed += (_, _) => Closed.Invoke(this, EventArgs.Empty);

        dialog.Show(ActiveWindow);
    }

    public void Close(DialogResult result = DialogResult.Ok)
    {
        RequireClose?.Invoke(result);
    }

    public IDialogText Text(string caption, string initial)
        => _group.Text(caption, initial);

    public IDialogBool Bool(string caption, bool initial)
        => _group.Bool(caption, initial);

    public IDialogInt Int(string caption, int initial, int min, int max)
        => _group.Int(caption, initial, min, max);

    public IDialogFloat Float(string caption, double initial, double min, double max)
        => _group.Float(caption, initial, min, max);

    public IDialogChoice Choice(params string[] items)
        => _group.Choice(items);

    public IDialogButton Button(string caption)
        => _group.Button(caption);

    public IDialogLabel Label(string caption)
        => _group.Label(caption);

    public IDialogSeparator Separator()
        => _group.Separator();

    public IDialogGroup Group(Orientation orientation)
        => _group.Group(orientation);

    public IDialogGroupBox GroupBox(string caption, Orientation orientation)
        => _group.GroupBox(caption, orientation);

    public IDialogTab Tab()
        => _group.Tab();
}