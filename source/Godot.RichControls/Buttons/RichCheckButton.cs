using CommunityToolkit.Mvvm.ComponentModel;

namespace Godot.RichControls.Buttons;

[INotifyPropertyChanged]
public partial class RichCheckButton : CheckButton
{
	public bool IsChecked
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

	public RichCheckButton() => Toggled += _ => OnPropertyChanged(nameof(IsChecked));
}