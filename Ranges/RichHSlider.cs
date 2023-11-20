using CommunityToolkit.Mvvm.ComponentModel;

namespace Godot.RichControls.Ranges;

[INotifyPropertyChanged]
public partial class RichHSlider : HSlider
{
	public new double Value
	{
		get => base.Value;
		set
		{
			if (base.Value == value)
				return;

			SetBlockSignals(true);
			base.Value = value;
			SetBlockSignals(false);

			OnPropertyChanged();
			OnPropertyChanged(nameof(IntValue));
        }
	}

	public int IntValue
	{
		get => (int)Value;
		set => Value = value;
	}

	public RichHSlider() => ValueChanged += _ =>
	{
		OnPropertyChanged(nameof(Value));
        OnPropertyChanged(nameof(IntValue));
	};
}