using System;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using ScriptingApi;

namespace ScriptingApiSandbox.Dialog.Element;

public sealed partial class DialogChoiceImpl : ObservableObject, IDialogChoice
{
    [ObservableProperty]
    public partial string Caption { get; set; } = "";

    [ObservableProperty]
    public partial bool IsEnabled { get; set; } = true;

    public event EventHandler? SelectedChanged;

    [ObservableProperty]
    public partial int SelectedIndex { get; set; }

    public object? SelectedItem => _items.Count is 0 ? null : _items[SelectedIndex];

    public ReadOnlyObservableCollection<string> Items { get; }

    private readonly ObservableCollection<string> _items;

    public DialogChoiceImpl(string[] items)
    {
        _items = new ObservableCollection<string>(items);
        Items = new ReadOnlyObservableCollection<string>(_items);
        
        SelectedIndex = Items.Count is 0 ? -1 : 0;
    }

    // ReSharper disable once UnusedParameterInPartialMethod
    partial void OnSelectedIndexChanged(int value)
    {
        SelectedChanged?.Invoke(this, EventArgs.Empty);
    }
}