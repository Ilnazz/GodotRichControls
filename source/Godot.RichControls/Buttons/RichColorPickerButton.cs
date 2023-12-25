using CommunityToolkit.Mvvm.ComponentModel;

namespace Godot.RichControls.Buttons;

[INotifyPropertyChanged]
public partial class RichColorPickerButton : ColorPickerButton
{
	public Color SelectedColor
	{
		get => Color;
		set
		{
			if (Color == value)
				return;

			SetBlockSignals(true);
			Color = value;
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

	public RichColorPickerButton() => ColorChanged += _ => OnPropertyChanged(nameof(SelectedColor));
}