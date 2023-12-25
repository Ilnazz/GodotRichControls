using CommunityToolkit.Mvvm.ComponentModel;

namespace Godot.RichControls.Buttons;

[INotifyPropertyChanged]
public partial class ToggleButton : Button
{
	public bool IsToggled
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

	public ToggleButton()
	{
		ToggleMode = true;

        Toggled += _ => OnPropertyChanged(nameof(IsToggled));
    }
}