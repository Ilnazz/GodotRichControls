using CommunityToolkit.Mvvm.ComponentModel;
using Godot;

[GlobalClass]
[INotifyPropertyChanged]
public partial class RichSpinBox : SpinBox
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

    public RichSpinBox() => ValueChanged += _ =>
	{
        OnPropertyChanged(nameof(Value));
        OnPropertyChanged(nameof(IntValue));
    };
}
