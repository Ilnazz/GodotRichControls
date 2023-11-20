using CommunityToolkit.Mvvm.ComponentModel;

namespace Godot.RichControls.Ranges;

// TODO: Refactor this in the future
[GlobalClass]
[INotifyPropertyChanged]
public partial class ItemsHSlider : HSlider
{
    #region Properties

    private IReadOnlyList<object> _items;
    public IEnumerable<object> Items
    {
        get => _items;
        set
        {
            _items = value.ToList();

            if (_items.Count is 0)
            {
                Visible = false;
                SelectedIndex = -1;
            }
            else
            {
                Visible = true;

                MaxValue = _items.Count - 1;
                Value = 0;
            }

            OnPropertyChanged();
        }
    }

    private int _selectedIndex;
    public int SelectedIndex
    {
        get => _selectedIndex;
        set
        {
            _selectedIndex = value;

            SetValueNoSignal(_selectedIndex);

            OnPropertyChanged(nameof(SelectedItem));
        }
    }

    public object? SelectedItem => SelectedIndex is not -1 ? _items[SelectedIndex] : null;
    #endregion

    public ItemsHSlider() => ValueChanged += value => SelectedIndex = (int)value;
}