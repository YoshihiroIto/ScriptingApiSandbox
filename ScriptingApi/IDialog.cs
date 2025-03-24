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
    
    IDialogButton Button(string caption);
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

public interface IDialogButton : IDialogElement
{
    event EventHandler? Clicked;
}

public enum DialogResult
{
    Ok,
    Cancel,
}