namespace ScriptingApi;

public interface IDialog : IDialogUIElementsHost
{
    DialogResult ShowModal();
    //void Show();
}

public interface IDialogUIElementsHost
{
    IDialogText Text(string caption, string initial);
    IDialogButton Button(string caption);
}

public interface IDialogElement
{
    string Caption { get; set; }
    bool IsEnabled { get; set; }
}

public interface IDialogText : IDialogElement
{
    string Text { get; set; }
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
