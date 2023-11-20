using CommunityToolkit.Mvvm.ComponentModel;
using EasyBindings;

namespace Godot.RichControls.Buttons.Groups;

[INotifyPropertyChanged]
public partial class RadioButtonGroup : ButtonGroup
{
    #region Properties
    private readonly IList<RadioButton> _buttons = new List<RadioButton>();
    public IEnumerable<RadioButton> Buttons => _buttons;

    private RadioButton? _selectedButton;
    public RadioButton? SelectedButton
    {
        get => _selectedButton;
        set
        {
            if (value is null)
                foreach (var button in _buttons)
                    button.IsSelected = false;
            
            else if (_buttons.Contains(value) == false)
                throw new ArgumentException("Given radio button is not in this group to be selected.", nameof(value));
            
            else
                value.IsSelected = true;

            _selectedButton = value;

            OnPropertyChanged();
        }
    }

    private bool _isEnabled = true;
    public bool IsEnabled
    {
        get => _isEnabled;
        set
        {
            foreach (var button in _buttons)
                button.IsEnabled = value;

            _isEnabled = value;
        }
    }
    #endregion

    #region Public methods 
    public void Add(RadioButton radioButton)
    {
        if (_buttons.Contains(radioButton))
            throw new ArgumentException("This radio button is already in this group to be added", nameof(radioButton));

        radioButton.Group = this;

        TriggerBinder.OnPropertyChanged(this, radioButton, o => o.IsSelected, isSelected =>
        {
            if (!isSelected)
                return;

            _selectedButton = radioButton;
            OnPropertyChanged(nameof(SelectedButton));
        });

        _buttons.Add(radioButton);
    }

    public void Remove(RadioButton radioButton)
    {
        if (_buttons.Remove(radioButton))
            TriggerBinder.UnbindPropertyChanged(this, radioButton);
    }

    public void Clear()
    {
        foreach (var button in _buttons)
            Remove(button);
    }
    #endregion
}