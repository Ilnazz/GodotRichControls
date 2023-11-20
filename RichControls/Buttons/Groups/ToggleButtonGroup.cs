using System.Collections.Generic;
using System.Linq;
using System;
using CommunityToolkit.Mvvm.ComponentModel;
using EasyBindings;

public class ToggleButtonGroup : ObservableObject
{
    #region Properties
    private readonly ICollection<ToggleButton> _buttons = new List<ToggleButton>();
    public IEnumerable<ToggleButton> Buttons
    {
        get => _buttons;
        set
        {
            foreach (var button in value)
                Add(button);
        }
    }

    public IEnumerable<ToggleButton> ToggledButtons
    {
        get => _buttons.Where(tb => tb.IsToggled);
        set
        {
            foreach (var toggleButton in value)
            {
                if (_buttons.Contains(toggleButton) == false)
                    throw new ArgumentException($"The button is not in the group to toggle it: {toggleButton}");

                _skipIsToggledNotification = true;
                toggleButton.IsToggled = true;
            }

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

    private bool _skipIsToggledNotification;

    #region Public methods 
    public void Add(ToggleButton toggleButton)
    {
        if (_buttons.Contains(toggleButton))
            throw new ArgumentException("This toggle button is already in this group", nameof(toggleButton));

        TriggerBinder.OnPropertyChanged(this, toggleButton, o => o.IsToggled, () =>
        {
            if (_skipIsToggledNotification)
                _skipIsToggledNotification = false;
            else
                OnPropertyChanged(nameof(ToggledButtons));
        });

        _buttons.Add(toggleButton);
    }

    public void Remove(ToggleButton toggleButton)
    {
        if (_buttons.Remove(toggleButton))
            TriggerBinder.UnbindPropertyChanged(this, toggleButton);
    }

    public void Clear()
    {
        foreach (var button in _buttons)
            Remove(button);
    }

    public void ToggleAll() => SetToggledAll(true);

    public void UntoggleAll() => SetToggledAll(false);

    public void SetToggledAll(bool isToggled)
    {
        foreach (var button in _buttons)
            button.IsToggled = isToggled;
    }

    public void ToggleAllNoSignal() => SetToggledAllNoSignal(true);

    public void UntoggleAllNoSignal() => SetToggledAllNoSignal(false);

    public void SetToggledAllNoSignal(bool isToggled)
    {
        foreach (var button in _buttons)
        {
            _skipIsToggledNotification = true;
            button.IsToggled = isToggled;
        }
    }
    #endregion
}