using CommunityToolkit.Mvvm.ComponentModel;
using Godot;

[GlobalClass]
[INotifyPropertyChanged]
public partial class RadioButton : CheckBox
{
	private RadioButtonGroup? _group;
	public RadioButtonGroup? Group
	{
		get => _group;
		set => ButtonGroup = _group = value;
	}

	public bool IsSelected
	{
		get => ButtonPressed;
		set
		{
			if (ButtonPressed == value)
				return;

			SetBlockSignals(true);
			ButtonPressed = value;
			SetBlockSignals(false);

			OnPropertyChanged();
		}
	}

	public bool IsEnabled
	{
		get => Disabled is false;
		set
		{
			Disabled = value is false;

            OnPropertyChanged();
        }
	}

	public RadioButton() => Toggled += _ => OnPropertyChanged(nameof(IsSelected));
}