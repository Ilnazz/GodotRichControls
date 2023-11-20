namespace Godot.RichControls.Buttons;

public class MenuItem
{
	public string Text { get; }

	public MenuItemType Type { get; }

	public string? ActionName { get; }

    public MenuItem(string text, MenuItemType type = MenuItemType.Text, string? actionName = null)
	{
		Text = text;
		Type = type;
		ActionName = actionName;
	}
}

public enum MenuItemType
{
	Text,
	CheckBox,
	RadioButton,
	Separator
}

[GlobalClass]
public partial class RichMenuButton : MenuButton
{
	#region Properties
	public bool IsEnabled
	{
		get => Disabled is false;
		set => Disabled = value is false;
	}

	private readonly IList<MenuItem> _items = new List<MenuItem>();
	public IEnumerable<MenuItem> Items
	{
		get => _items;
		set
		{
			for (var i = 0; i < _items.Count; i++)
				_popupMenu.RemoveItem(0);

			_items.Clear();

			AddItems(value);
		}
	}
	#endregion

	public event Action<MenuItem>? ItemPressed,
								   ItemChecked,
								   ItemSelected;

	private readonly PopupMenu _popupMenu;

	public RichMenuButton()
	{
		_popupMenu = GetPopup();
		_popupMenu.IndexPressed += OnIndexPressed;
	}

	#region Public methods 
	#region Pressing
	public void PressItem(int itemIndex)
	{
		EnsureContainsIndex(itemIndex);

		if (_items[itemIndex].Type is not MenuItemType.Text)
			throw new Exception("Item is not plain text to be just pressable.");

		ItemPressed?.Invoke(_items[itemIndex]);
	}

	public void PressItem(MenuItem item)
	{
		EnsureContainsItem(item);

		if (item.Type is not MenuItemType.Text)
			throw new Exception("Item is not plain text to be just pressable.");

		ItemPressed?.Invoke(item);
	}
	#endregion

	#region Checking
	public void SetItemChecked(int itemIndex, bool @checked)
	{
		SetItemCheckedNoSignal(itemIndex, @checked);
		ItemChecked?.Invoke(_items[itemIndex]);
	}

	public void SetItemChecked(MenuItem item, bool @checked)
	{
		SetItemCheckedNoSignal(item, @checked);
		ItemChecked?.Invoke(item);
	}

	public void SetItemCheckedNoSignal(int itemIndex, bool @checked)
	{
		EnsureContainsIndex(itemIndex);

		if (_items[itemIndex].Type is not MenuItemType.CheckBox)
			throw new ArgumentException("Item is not checkbox to be checkable.");

		_popupMenu.SetItemChecked(itemIndex, @checked);
	}

	public void SetItemCheckedNoSignal(MenuItem item, bool @checked)
	{
		EnsureContainsItem(item);

		if (item.Type is not MenuItemType.CheckBox)
			throw new ArgumentException("Item is not checkbox to be checkable.");

		_popupMenu.SetItemChecked(_items.IndexOf(item), @checked);
	}
	#endregion

	#region Selecting
	public void SetItemSelected(int itemIndex, bool selected)
	{
		SetItemSelectedNoSignal(itemIndex, selected);
		ItemSelected?.Invoke(_items[itemIndex]);
	}

	public void SetItemSelected(MenuItem item, bool selected)
	{
		SetItemSelectedNoSignal(item, selected);
		ItemSelected?.Invoke(item);
	}

	public void SetItemSelectedNoSignal(int itemIndex, bool selected)
	{
		EnsureContainsIndex(itemIndex);

		if (_items[itemIndex].Type is not MenuItemType.RadioButton)
			throw new ArgumentException("Item is not radio button to be selectable.");

		UnselectAllItems();
		_popupMenu.SetItemChecked(itemIndex, selected);
	}

	public void SetItemSelectedNoSignal(MenuItem item, bool selected)
	{
		EnsureContainsItem(item);

		if (item.Type is not MenuItemType.RadioButton)
			throw new ArgumentException("Item is not radio button to be selectable.");

		UnselectAllItems();
		_popupMenu.SetItemChecked(_items.IndexOf(item), selected);
	}

	public void UnselectAllItems()
	{
		for (var i = 0; i < _items.Count; i++)
			if (_items[i].Type is MenuItemType.RadioButton)
				_popupMenu.SetItemChecked(i, false);
	}
	#endregion
	#endregion

	#region Private methods 
	private void OnIndexPressed(long longIndex)
	{
		var index = (int)longIndex;
		var item = _items.ElementAt(index);

		switch (item.Type)
        {
            case MenuItemType.Text:
                ItemPressed?.Invoke(item);
                break;

            case MenuItemType.CheckBox:
                _popupMenu.ToggleItemChecked(index);
                ItemChecked?.Invoke(item);
                break;

            case MenuItemType.RadioButton:
                UnselectAllItems();
                _popupMenu.ToggleItemChecked(index);
                ItemSelected?.Invoke(item);
                break;
        }
	}

	private void AddItems(IEnumerable<MenuItem> items)
	{
		foreach (var item in items)
			AddItem(item);
	}

	private void AddItem(MenuItem item)
    {
        var itemIndex = _items.Count;
        _popupMenu.AddItem(item.Text, itemIndex);
		_items.Add(item);

		switch (item.Type)
        {
			case MenuItemType.Separator:
                _popupMenu.AddItem(item.Text);
                _popupMenu.SetItemAsSeparator(itemIndex, true);
                return;

            case MenuItemType.CheckBox:
                _popupMenu.SetItemAsCheckable(itemIndex, true);
                break;

            case MenuItemType.RadioButton:
                _popupMenu.SetItemAsRadioCheckable(itemIndex, true);
                break;
        }

		if (item.ActionName is not null)
		{
			if (!InputMap.HasAction(item.ActionName))
				throw new Exception($"Action with name '{item.ActionName}' is not registered in the InputMap.");

			var itemShortcut = new Shortcut();
			var actionEvent = new InputEventAction { Action = item.ActionName };
			    
			itemShortcut.Events.Add(actionEvent);
			_popupMenu.SetItemShortcut(itemIndex, itemShortcut);
		}
	}

	private void EnsureContainsIndex(int itemIndex)
	{
		if (itemIndex < 0)
			throw new ArgumentException("Should be greater than zero.", nameof(itemIndex));
		
        if (itemIndex > _items.Count - 1)
			throw new ArgumentOutOfRangeException(nameof(itemIndex), "Index is out of range.");
	}

	private void EnsureContainsItem(MenuItem item)
	{
		if (!_items.Contains(item))
			throw new ArgumentException("This item is not in menu.", nameof(item));
	}
	#endregion
}