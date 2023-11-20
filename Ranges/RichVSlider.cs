using CommunityToolkit.Mvvm.ComponentModel;

namespace Godot.RichControls.Ranges;

[GlobalClass]
[INotifyPropertyChanged]
public partial class RichVSlider : VSlider
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
    
	public RichVSlider() => ValueChanged += _ =>
    {
        OnPropertyChanged(nameof(Value));
        OnPropertyChanged(nameof(IntValue));
    };
}