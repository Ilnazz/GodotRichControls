using CommunityToolkit.Mvvm.ComponentModel;
using EasyBindings.Interfaces;

namespace Godot.RichControls.Buttons;

[INotifyPropertyChanged]
public partial class RichButton : Button, ICommandExecutor
{
	public bool IsEnabled
	{
		get => Disabled is false;
		set
		{
			Disabled = value is false;

			OnPropertyChanged();
		}
	}

	public bool CanExecuteCommand
    {
        get => IsEnabled;
		set => IsEnabled = value;
	}

	public event Action? CommandExecutionRequested;

	public RichButton() => Pressed += () => CommandExecutionRequested?.Invoke();
}