using EasyBindings;
using EasyBindings.Interfaces;

namespace Godot.RichControls;

[GlobalClass]
public partial class CollapsiblePanelContainer : PanelContainer, IUnsubscribe
{
	private const string IconsPath = "res://Addons/RichControls/Icons",
						 CollapsedIconName = "TreeArrowRight.png",
						 VisibleIconName = "TreeArrowDown.png";

	// TODO: Preloading and proper addoning it
	private static CompressedTexture2D _collapsedIcon = null!,
									   _visibleIcon = null!;

	#region Setting up
	public override void _Ready()
	{
		LoadIcons();

		SetupCollapseToggleButton();
		SetupTitleContainer();
		SetupBodyContainer();
	}

	private void LoadIcons()
	{
		_collapsedIcon = ResourceLoader.Load<CompressedTexture2D>($"{IconsPath}/{CollapsedIconName}");
		_visibleIcon = ResourceLoader.Load<CompressedTexture2D>($"{IconsPath}/{VisibleIconName}");
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
	private StyleBoxFlat _titleContainerStylebox = null!;

	private void SetupTitleContainer()
	{
		_titlePanelContainer = (PanelContainer)FindChild("TitlePanelContainer");
		_titleContainerStylebox = (StyleBoxFlat)_titlePanelContainer.GetThemeStylebox("panel");

		TriggerBinder.OnPropertyChanged(this, CollapseToggleButton, o => o.IsToggled, UpdateTitlePanelContainerStyleBox);
		UpdateTitlePanelContainerStyleBox(CollapseToggleButton.IsToggled);
	}

	private void UpdateTitlePanelContainerStyleBox(bool isCollapsed)
	{
		if (isCollapsed)
		{
			_titleContainerStylebox.CornerRadiusBottomLeft = 0;
			_titleContainerStylebox.CornerRadiusBottomRight = 0;
		}
		else
		{
			_titleContainerStylebox.CornerRadiusBottomLeft = 3;
			_titleContainerStylebox.CornerRadiusBottomRight = 3;
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