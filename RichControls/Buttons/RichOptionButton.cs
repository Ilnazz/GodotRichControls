using System;
using System.Collections.Generic;
using Godot;

public class OptionItem
{
	public string Text { get; }

	public OptionItemType Type { get; }

	public OptionItem(string text, OptionItemType type = OptionItemType.Text)
    {
        Text = text;
        Type = type;
    }
}

public enum OptionItemType
{
	Text,
	Separator
}

[GlobalClass]
public partial class RichOptionButton : OptionButton
{
	#region Properties
	public OptionItem? SelectedItem
	{
		get => SelectedIndex is not -1 ? _items[SelectedIndex] : null;
		set
		{
			if (value is null)
			{
				SelectedIndex = -1;
				return;
			}

			EnsureContainsItem(value);

			SelectedIndex = _items.IndexOf(value);
		}
	}

	public int SelectedIndex
	{
		get => Selected;
		set
		{
			EnsureContainsIndex(value);
			Selected = value;
			Text = _items[value].Text;
		}
	}

	public bool IsEnabled
	{
		get => Disabled is false;
		set => Disabled = value is false;
	}

	private readonly IList<OptionItem> _items = new List<OptionItem>();
	public IEnumerable<OptionItem> Items
	{
		get => _items;
		set
		{
			for (var i = 0; i < _items.Count; i++)
				RemoveItem(0);

			_items.Clear();

            AddItems(value);
		}
	}
	#endregion

	public new event Action<OptionItem>? ItemSelected;

	public RichOptionButton() => base.ItemSelected += OnItemSelected;

	#region Private methods 
	private void OnItemSelected(long index) => ItemSelected?.Invoke(_items[(int)index]);

	private void AddItems(IEnumerable<OptionItem> items)
	{
		foreach (var item in items)
			AddItem(item);
	}

	private void AddItem(OptionItem item)
	{
		_items.Add(item);

		if (item.Type is OptionItemType.Text)
		    AddItem(item.Text);
		else
			AddSeparator(item.Text);
	}

	private void EnsureContainsIndex(int itemIndex)
	{
		if (itemIndex < 0)
			throw new ArgumentException("Should be greater than zero.", nameof(itemIndex));
		
        if (itemIndex > _items.Count - 1)
			throw new ArgumentOutOfRangeException(nameof(itemIndex), "Index is out of range.");
	}

	private void EnsureContainsItem(OptionItem item)
	{
		if (!_items.Contains(item))
			throw new ArgumentException("This item is not in the list of options.", nameof(item));
	}
	#endregion
}