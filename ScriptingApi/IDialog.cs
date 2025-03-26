using System.Collections.ObjectModel;

namespace ScriptingApi;

public interface IDialog : IDialogUIElementsHost
{
    DialogResult DialogResult { get; }
    event EventHandler? Closed;

    DialogResult ShowModal();
    void Show();
}

public interface IDialogUIElementsHost
{
    IDialogText Text(string caption, string initial);
    IDialogBool Bool(string caption, bool initial);
    IDialogInt Int(string caption, int initial, int min, int max);
    IDialogFloat Float(string caption, double initial, double min, double max);
    IDialogChoice Choice(params string[] items);

    IDialogButton Button(string caption);
    IDialogLabel Label(string caption);

    IDialogGroup Group(Orientation orientation);
    IDialogTab Tab();
}

public interface IDialogElement
{
    string Caption { get; set; }
    bool IsEnabled { get; set; }
}

public interface IDialogText : IDialogElement
{
    event EventHandler? TextChanged;

    string Text { get; set; }
}

public interface IDialogBool : IDialogElement
{
    event EventHandler? ValueChanged;

    bool Value { get; set; }
}

public interface IDialogInt : IDialogElement
{
    event EventHandler? ValueChanged;

    int Value { get; set; }
    int Min { get; set; }
    int Max { get; set; }
}

public interface IDialogFloat : IDialogElement
{
    event EventHandler? ValueChanged;

    double Value { get; set; }
    double Min { get; set; }
    double Max { get; set; }
}

public interface IDialogChoice : IDialogElement
{
    event EventHandler? SelectedChanged;
    
    int SelectedIndex { get; set; }
    object? SelectedItem { get; }
}

public interface IDialogButton : IDialogElement
{
    event EventHandler? Clicked;
}

public interface IDialogLabel : IDialogElement;

public interface IDialogSeparator : IDialogElement;

public interface IDialogGroup : IDialogElement, IDialogUIElementsHost
{
    ReadOnlyObservableCollection<IDialogElement> Elements { get; }
}

public interface IDialogTab : IDialogElement
{
    ReadOnlyObservableCollection<IDialogGroup> Pages { get; }

    IDialogGroup Page(string name);
}

public enum DialogResult
{
    Ok,
    Cancel,
}

public enum Orientation
{
    Horizontal,
    Vertical,
}