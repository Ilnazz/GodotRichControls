using CommunityToolkit.Mvvm.ComponentModel;

namespace Godot.RichControls.Buttons;

[INotifyPropertyChanged]
public partial class RichCheckBox : CheckBox
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

	public RichCheckBox() => Toggled += _ => OnPropertyChanged(nameof(IsChecked));
}