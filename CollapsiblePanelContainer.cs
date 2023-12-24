using EasyBindings;
using EasyBindings.Interfaces;
using Godot.RichControls.Buttons;

namespace Godot.RichControls;

public partial class CollapsiblePanelContainer : PanelContainer, IUnsubscribe
{
	private const string
        ResourcesDirPath = "res://Addons/RichControls/Assets/CollapsiblePanelContainer",
		IconsDirName = "Icons",
		StyleBoxesDirName = "StyleBoxes",

        CollapsedIconName = "Collapsed",
        VisibleIconName = "Visible",
        TitleStyleBoxName = "TitleStyleBox";

	private static CompressedTexture2D _collapsedIcon = null!,
									   _visibleIcon = null!;

    private static StyleBoxFlat _titleStyleBox = null!;

    #region Preloading resources
	static CollapsiblePanelContainer()
    {
        LoadIcons();
        LoadStyleBoxes();
    }

    private static void LoadIcons()
    {
        _collapsedIcon = LoadIcon(CollapsedIconName);
        _visibleIcon = LoadIcon(VisibleIconName);
    }

	private static CompressedTexture2D LoadIcon(string name) =>
        ResourceLoader.Load<CompressedTexture2D>($"{ResourcesDirPath}/{IconsDirName}/{name}.png");

    private static void LoadStyleBoxes()
    {
        _titleStyleBox = LoadStyleBox(TitleStyleBoxName);
    }

    private static StyleBoxFlat LoadStyleBox(string name) =>
        ResourceLoader.Load<StyleBoxFlat>($"{ResourcesDirPath}/{StyleBoxesDirName}/{name}.tres");
    #endregion

    #region Setting up
    public override void _Ready()
	{
		SetupCollapseToggleButton();
		SetupTitleContainer();
		SetupBodyContainer();
	}
	#endregion

	#region Controls
	#region Collapse toggle button
	protected ToggleButton CollapseToggleButton = null!;

	private void SetupCollapseToggleButton()
	{
		CollapseToggleButton = (ToggleButton)FindChild("CollapseToggleButton");

		PropertyBinder.BindOneWay(this, CollapseToggleButton, t => t.Icon, CollapseToggleButton, s => s.IsToggled,
			isToggled => isToggled ? _visibleIcon : _collapsedIcon);
	}
	#endregion

	#region Title container
	private PanelContainer _titlePanelContainer = null!;

	private void SetupTitleContainer()
	{
		_titlePanelContainer = (PanelContainer)FindChild("TitlePanelContainer");
		_titlePanelContainer.AddThemeStyleboxOverride("panel", _titleStyleBox.Duplicate() as StyleBox);

		TriggerBinder.OnPropertyChanged(this, CollapseToggleButton, o => o.IsToggled, UpdateTitleStyleBoxCorners);
		UpdateTitleStyleBoxCorners(CollapseToggleButton.IsToggled);
	}

	private void UpdateTitleStyleBoxCorners(bool isCollapsed)
    {
        var titleStyleBox = (StyleBoxFlat)_titlePanelContainer.GetThemeStylebox("panel");

		if (isCollapsed)
		{
            titleStyleBox.CornerRadiusBottomLeft = 0;
            titleStyleBox.CornerRadiusBottomRight = 0;
		}
		else
		{
            titleStyleBox.CornerRadiusBottomLeft = 3;
            titleStyleBox.CornerRadiusBottomRight = 3;
		}
	}
	#endregion

	#region Body container
	private Container _bodyContainer = null!;
	private void SetupBodyContainer()
	{
		_bodyContainer = (Container)FindChild("BodyPanelContainer");

		PropertyBinder.BindOneWay(this, _bodyContainer, t => t.Visible, CollapseToggleButton, s => s.IsToggled);
	}
	#endregion
	#endregion

	public virtual void Unsubscribe()
	{
        PropertyBinder.UnbindFromTarget(this, CollapseToggleButton);
        PropertyBinder.UnbindFromTarget(this, _bodyContainer);
        TriggerBinder.UnbindPropertyChanged(this, CollapseToggleButton);
    }
}