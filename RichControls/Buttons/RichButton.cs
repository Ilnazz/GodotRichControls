using System;
using CommunityToolkit.Mvvm.ComponentModel;
using EasyBindings.Interfaces;
using Godot;

[GlobalClass]
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
		set => IsEnabled = value;
	}

	public event Action? CommandExecutionRequested;

	public RichButton() => Pressed += () => CommandExecutionRequested?.Invoke();
}