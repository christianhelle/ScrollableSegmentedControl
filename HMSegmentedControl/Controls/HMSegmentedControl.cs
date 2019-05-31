using System;
using System.Collections.Generic;
using CoreGraphics;
//using CoreGraphics;
using Foundation;
using UIKit;
using CoreAnimation;
using System.Linq;
using System.Diagnostics;

namespace HMSegmentedControlSample
{
	public class HMScrollView : UIScrollView
	{
		public override void TouchesBegan(NSSet touches, UIEvent evt)
		{
			if (!Dragging)
				NextResponder.TouchesBegan(touches, evt);
			else
				base.TouchesBegan(touches, evt);
		}

		public override void TouchesMoved(NSSet touches, UIEvent evt)
		{
			if (!Dragging)
				NextResponder.TouchesMoved(touches, evt);
			else
				base.TouchesMoved(touches, evt);
		}

		public override void TouchesEnded(NSSet touches, UIEvent evt)
		{
			if (!Dragging)
				NextResponder.TouchesEnded(touches, evt);
			else
				base.TouchesEnded(touches, evt);
		}
	}

	public enum HMSegmentedControlType
	{
		Text,
		Image,
		TextAndImage
	}

	public enum HMSegmentedControlSelectionStyle
	{
		TextWidthStripe,
		FullWidthStripe,
		Box,
		Arrow
	}

	public enum HMSegmentedControlIndicatorLocation
	{
		Up,
		Down,
		None
	}

	public enum HMSegmentedControlWidthStyle
	{
		Fixed,
		Dynamic
	}

	public enum HMSegmentedControlBorderType
	{
		None,
		Top,
		Left,
		Bottom,
		Right
	}

	public class HMSegmentedControl : UIControl
	{
		private HMSegmentedControlType type;
		private HMScrollView scrollView;

		private List<string> sectionTitles;
		private List<UIImage> sectionImages;
		private List<UIImage> sectionSelectedImages;
		private List<float> segmentWidths;
		private nfloat segmentWidth;
		private float borderWidth;

		private UIEdgeInsets selectionIndicatorEdgeInsets;
		private HMSegmentedControlWidthStyle segmentWidthStyle;
		private HMSegmentedControlBorderType borderType;
		private HMSegmentedControlIndicatorLocation selectionIndicatorLocation;
		private float selectionIndicatorBoxOpacity;

		public Func<HMSegmentedControl, string, int, bool, NSAttributedString> TitleFormatter { get; set; }
		public EventHandler<int> IndexChange;

		public UIColor BorderColor { get; set; }
		public bool TouchEnabled { get; set; }
		public bool UserDraggable { get; set; }
		public int SelectedIndex { get; set; }
		public UIFont Font { get; set; }
		public UIFont SelectedFont { get; set; }
		public UIColor TextColor { get; set; }
		public UIColor SelectedTextColor { get; set; }
		public UIColor SelectionIndicatorColor { get; set; }
		public float SelectionIndicatorHeight { get; set; }
		public UIColor VerticalDividerColor { get; set; }
		public bool VerticalDividerEnabled { get; set; }
		public float VerticalDividerWidth { get; set; }
		public HMSegmentedControlSelectionStyle SelectionStyle { get; set; }
		public UIEdgeInsets SegmentEdgeInset { get; set; }
		public CALayer SelectionIndicatorBoxLayer { get; set; }
		public CALayer SelectionIndicatorArrowLayer { get; set; }
		public CALayer SelectionIndicatorStripLayer { get; set; }
		public bool ShouldAnimateUserSelection { get; set; }

		public HMSegmentedControlBorderType BorderType
		{
			get { return borderType; }
			set
			{
				borderType = value;
				SetNeedsDisplay();
			}
		}

		public HMSegmentedControlIndicatorLocation SelectionIndicatorLocation
		{
			get { return selectionIndicatorLocation; }
			set
			{
				selectionIndicatorLocation = value;
				if (value == HMSegmentedControlIndicatorLocation.None)
					SelectionIndicatorHeight = 0.0f;
			}
		}

		public float SelectionIndicatorBoxOpacity
		{
			get { return selectionIndicatorBoxOpacity; }
			set
			{
				selectionIndicatorBoxOpacity = value;
				SelectionIndicatorBoxLayer.Opacity = value;
			}
		}

		public HMSegmentedControlWidthStyle SegmentWidthStyle
		{
			get { return segmentWidthStyle; }
			set
			{
				segmentWidthStyle = type == HMSegmentedControlType.Image ? HMSegmentedControlWidthStyle.Fixed : value;
			}
		}

		public HMSegmentedControl(IEnumerable<string> sectionTitles)
		{
			Initialize();
			this.sectionTitles = new List<string>(sectionTitles);
			type = HMSegmentedControlType.Text;
		}

		public HMSegmentedControl(IEnumerable<UIImage> sectionImages, IEnumerable<UIImage> sectionSelectedImages)
		{
			Initialize();
			this.sectionImages = new List<UIImage>(sectionImages);
			this.sectionSelectedImages = new List<UIImage>(sectionSelectedImages);
			type = HMSegmentedControlType.Image;
		}

		public HMSegmentedControl(IEnumerable<UIImage> sectionImages, IEnumerable<UIImage> sectionSelectedImages, IEnumerable<string> sectionTitles)
		{
			Initialize();
			this.sectionSelectedImages = new List<UIImage>(sectionSelectedImages);
			this.sectionTitles = new List<string>(sectionTitles);
			this.sectionImages = new List<UIImage>(sectionImages);
			type = HMSegmentedControlType.TextAndImage;
		}

		private void Initialize()
		{
			scrollView = new HMScrollView { ScrollsToTop = false, ShowsVerticalScrollIndicator = false, ShowsHorizontalScrollIndicator = false };
			AddSubview(scrollView);

			Opaque = false;
			SelectionIndicatorColor = UIColor.FromRGBA(52.0f / 255.0f, 181.0f / 255.0f, 229.0f / 255.0f, 1.0f);

			SelectedIndex = 0;
			SegmentEdgeInset = new UIEdgeInsets(0, 5, 0, 5);
			SelectionIndicatorHeight = 5.0f;
			selectionIndicatorEdgeInsets = new UIEdgeInsets(0, 0, 0, 0);
			SelectionStyle = HMSegmentedControlSelectionStyle.TextWidthStripe;
			SelectionIndicatorLocation = HMSegmentedControlIndicatorLocation.Up;
			segmentWidthStyle = HMSegmentedControlWidthStyle.Fixed;
			UserDraggable = true;
			TouchEnabled = true;
			VerticalDividerEnabled = false;
			VerticalDividerColor = UIColor.Black;
			BorderColor = UIColor.Black;
			borderWidth = 1.0f;

			ShouldAnimateUserSelection = true;
			selectionIndicatorBoxOpacity = 0.2f;
			SelectionIndicatorArrowLayer = new DisposableCALayer();
			SelectionIndicatorStripLayer = new DisposableCALayer();
			SelectionIndicatorBoxLayer = new DisposableCALayer { Opacity = selectionIndicatorBoxOpacity, BorderWidth = 1.0f };

			ContentMode = UIViewContentMode.Redraw;
		}

		public override void LayoutSubviews()
		{
			base.LayoutSubviews();
			UpdateSegmentRects();
		}

		public override CGRect Frame
		{
			get { return base.Frame; }
			set
			{
				base.Frame = value;
				if (value != CGRect.Empty)
					UpdateSegmentRects();
			}
		}

		public void SetSelectionIndicatorLocation(HMSegmentedControlIndicatorLocation value)
		{
			SelectionIndicatorLocation = value;
			if (value == HMSegmentedControlIndicatorLocation.None)
				SelectionIndicatorHeight = 0.0f;
		}

		#region Drawing

		private CGSize MeasureTitle(int index)
		{
			var title = sectionTitles[index];
			var size = CGSize.Empty;
			var selected = index == SelectedIndex;

			if (TitleFormatter == null)
			{
				var nsTitle = new NSString(title);
				var stringAttributes = selected ? GetSelectedTitleTextAttributes() : GetTitleTextAttributes();
				size = new Version(UIDevice.CurrentDevice.SystemVersion).Major < 7 ? nsTitle.StringSize(stringAttributes.Font) : nsTitle.GetSizeUsingAttributes(stringAttributes);
			}
			else
			{
				size = TitleFormatter(this, title, index, selected).Size;
			}

			return size;
		}

		private NSAttributedString AttributedTitle(int index)
		{
			var title = sectionTitles[index];
			var selected = index == SelectedIndex;

			return TitleFormatter != null
					? TitleFormatter(this, title, index, selected)
					: new NSAttributedString(title, selected ? GetSelectedTitleTextAttributes() : GetTitleTextAttributes());
		}

		public override void Draw(CGRect rect)
		{
			if (BackgroundColor != null)
				BackgroundColor.SetFill();

			SelectionIndicatorArrowLayer.BackgroundColor = SelectionIndicatorColor.CGColor;
			SelectionIndicatorStripLayer.BackgroundColor = SelectionIndicatorColor.CGColor;

			SelectionIndicatorBoxLayer.BackgroundColor = SelectionIndicatorColor.CGColor;
			SelectionIndicatorBoxLayer.BorderColor = SelectionIndicatorColor.CGColor;

			scrollView.Layer.Sublayers = new DisposableCALayer[0];
			ClearScrollViewSubLayers();
			var oldRect = rect;

			if (type == HMSegmentedControlType.Text)
			{
				for (int idx = 0; idx < sectionTitles.Count; idx++)
				{
					var size = MeasureTitle(idx);
					var stringWidth = size.Width;
					var stringHeight = size.Height;
					CGRect rectangle, rectDiv, rectFull;

					var locationUp = SelectionIndicatorLocation == HMSegmentedControlIndicatorLocation.Up;
					var selectionStyleNotBox = SelectionStyle != HMSegmentedControlSelectionStyle.Box;
					var y = (float)Math.Round(((Frame.Height - (selectionStyleNotBox ? 1 : 0 * SelectionIndicatorHeight)) / 2) - (stringHeight / 2) + (SelectionIndicatorHeight * (locationUp ? 1 : 0)));

					if (segmentWidthStyle == HMSegmentedControlWidthStyle.Fixed)
					{
						rectangle = new CGRect((segmentWidth * idx) + (segmentWidth - stringWidth) / 2, y, stringWidth, stringHeight);
						rectDiv = new CGRect((segmentWidth * idx) + (VerticalDividerWidth / 2), SelectionIndicatorHeight * 2, VerticalDividerWidth, Frame.Size.Height - (SelectionIndicatorHeight * 4));
						rectFull = new CGRect(segmentWidth * idx, 0, segmentWidth, oldRect.Size.Height);
					}
					else
					{
						var xOffset = 0.0f;
						var i = 0;
						foreach (var width in segmentWidths)
						{
							if (idx == i)
								break;
							xOffset += width;
							i++;
						}

						var widthForIndex = segmentWidths[idx];
						rectangle = new CGRect(xOffset, y, widthForIndex, stringHeight);
						rectFull = new CGRect(segmentWidth * idx, 0, widthForIndex, oldRect.Size.Height);
						rectDiv = new CGRect(xOffset - (VerticalDividerWidth / 2),
							SelectionIndicatorHeight * 2, VerticalDividerWidth, Frame.Size.Height - (SelectionIndicatorHeight * 4));
					}

					rectangle = new CGRect((float)Math.Ceiling(rectangle.X), (float)Math.Ceiling(rectangle.Y), (float)Math.Ceiling(rectangle.Size.Width), (float)Math.Ceiling(rectangle.Size.Height));

					var titleLayer = new DisposableCATextLayer
					{
						Frame = rectangle,
						AlignmentMode = CATextLayer.AlignmentCenter,
						TruncationMode = CATextLayer.TruncantionEnd,
						AttributedString = AttributedTitle(idx),
						ContentsScale = UIScreen.MainScreen.Scale
					};
					AddScrollViewSubLayer(titleLayer);

					if (VerticalDividerEnabled)
					{
						var verticalDividerLayer = new DisposableCALayer { Frame = rectDiv, BackgroundColor = VerticalDividerColor.CGColor };
						AddScrollViewSubLayer(verticalDividerLayer);
					}

					AddBackgroundAndBorderLayer(rectFull);
				}
			}
			else if (type == HMSegmentedControlType.Image)
			{
				for (int idx = 0; idx < sectionTitles.Count; idx++)
				{
					var icon = sectionImages[idx];
					var imageWidth = icon.Size.Width;
					var imageHeight = icon.Size.Height;
					var y = (float)Math.Round(Frame.Height - SelectionIndicatorHeight) / 2 - imageHeight / 2 + (SelectionIndicatorLocation == HMSegmentedControlIndicatorLocation.Up ? SelectionIndicatorHeight : 0);
					var x = segmentWidth * idx + (segmentWidth - imageWidth) / 2.0f;
					var rectNew = new CGRect(x, y, imageWidth, imageHeight);

					var imageLayer = new DisposableCALayer { Frame = rectNew };
					if (SelectedIndex == idx && sectionSelectedImages != null)
						imageLayer.Contents = sectionSelectedImages[idx].CGImage;
					else
						imageLayer.Contents = icon.CGImage;

					AddScrollViewSubLayer(imageLayer);

					if (VerticalDividerEnabled && idx > 0)
					{
						var verticalDividerLayer = new DisposableCALayer
						{
							Frame = new CGRect((segmentWidth * idx) - (VerticalDividerWidth / 2), SelectionIndicatorHeight * 2, VerticalDividerWidth, Frame.Size.Height - (SelectionIndicatorHeight * 4)),
							BackgroundColor = VerticalDividerColor.CGColor
						};
						AddScrollViewSubLayer(verticalDividerLayer);
					}

					AddBackgroundAndBorderLayer(rectNew);
				}
			}
			else if (type == HMSegmentedControlType.TextAndImage)
			{
				for (int idx = 0; idx < sectionTitles.Count; idx++)
				{
					var icon = sectionImages[idx];
					var imageWidth = icon.Size.Width;
					var imageHeight = icon.Size.Height;

					var stringHeight = MeasureTitle(idx).Height;
					var yOffset = (float)Math.Round(((Frame.Height - SelectionIndicatorHeight) / 2) - (stringHeight / 2));

					var imageXOffset = SegmentEdgeInset.Left;
					var textXOffset = SegmentEdgeInset.Left;
					var textWidth = (nfloat)0.0f;

					if (segmentWidthStyle == HMSegmentedControlWidthStyle.Fixed)
					{
						imageXOffset = (segmentWidth * idx) + (segmentWidth / 2.0f) - (imageWidth / 2.0f);
						textXOffset = segmentWidth * idx;
						textWidth = segmentWidth;
					}
					else
					{
						var xOffset = 0.0f;
						var i = 0;
						foreach (var width in segmentWidths)
						{
							if (idx == i)
								break;
							xOffset += width;
							i++;
						}

						imageXOffset = xOffset + (segmentWidths[idx] / 2.0f) - (imageWidth / 2.0f);
						textXOffset = xOffset;
						textWidth = segmentWidths[idx];
					}

					var imageYOffset = (float)Math.Round((Frame.Height - SelectionIndicatorHeight) / 2.0f);
					var imageRect = new CGRect(imageXOffset, imageYOffset, imageWidth, imageHeight);
					var textRect = new CGRect((float)Math.Ceiling(textXOffset), (float)Math.Ceiling(yOffset), (float)Math.Ceiling(textWidth), (float)Math.Ceiling(stringHeight));

					var titleLayer = new DisposableCATextLayer
					{
						Frame = textRect,
						AlignmentMode = CATextLayer.AlignmentCenter,
						AttributedString = AttributedTitle(idx),
						TruncationMode = CATextLayer.TruncantionEnd,
						ContentsScale = UIScreen.MainScreen.Scale
					};

					var imageLayer = new DisposableCALayer
					{
						Frame = imageRect,
						Contents = (SelectedIndex == idx && sectionSelectedImages != null) ? sectionSelectedImages[idx].CGImage : icon.CGImage
					};

					AddScrollViewSubLayer(imageLayer);
					AddScrollViewSubLayer(titleLayer);

					AddBackgroundAndBorderLayer(imageRect);
				}
			}

			if (SelectedIndex != -1)
			{
				if (SelectionStyle == HMSegmentedControlSelectionStyle.Arrow)
				{
					SetArrowFrame();
					AddScrollViewSubLayer(SelectionIndicatorArrowLayer);
				}
				else
				{
					if (SelectionIndicatorStripLayer.SuperLayer == null)
					{
						SelectionIndicatorStripLayer.Frame = FrameForSelectionIndicator();
						AddScrollViewSubLayer(SelectionIndicatorStripLayer);

						if (SelectionStyle == HMSegmentedControlSelectionStyle.Box && SelectionIndicatorBoxLayer.SuperLayer == null)
						{
							SelectionIndicatorBoxLayer.Frame = FrameForFillerSelectionIndicator();
							InsertScrollViewSubLayer(SelectionIndicatorBoxLayer, 0);
						}
					}
				}
			}
		}

		private readonly List<CALayer> layers = new List<CALayer>();
		private void AddScrollViewSubLayer(CALayer layer)
		{
			scrollView.Layer.AddSublayer(layer);
			layers.Add(layer);
		}

		private void InsertScrollViewSubLayer(CALayer layer, int index)
		{
			scrollView.Layer.InsertSublayer(layer, index);
			layers.Add(layer);
		}

		private void ClearScrollViewSubLayers()
		{
			var diposables = layers.Where(c => c != SelectionIndicatorArrowLayer && c != SelectionIndicatorBoxLayer && c != SelectionIndicatorStripLayer);
			foreach (var layer in diposables)
			{
				layer.RemoveFromSuperLayer();
				layer.Dispose();
			}

			layers.Clear();
		}

		private void AddBackgroundAndBorderLayer(CGRect fullRect)
		{
			var backgroundLayer = new DisposableCALayer { Frame = fullRect };
			InsertScrollViewSubLayer(backgroundLayer, 0);

			var borderLayer = new DisposableCALayer { BackgroundColor = BorderColor.CGColor };
			switch (borderType)
			{
				case HMSegmentedControlBorderType.Top:
					borderLayer.Frame = new CGRect(0, 0, fullRect.Size.Width, borderWidth);
					break;
				case HMSegmentedControlBorderType.Left:
					borderLayer.Frame = new CGRect(0, 0, borderWidth, fullRect.Size.Height);
					break;
				case HMSegmentedControlBorderType.Bottom:
					borderLayer.Frame = new CGRect(0, fullRect.Size.Height - borderWidth, fullRect.Size.Width, borderWidth);
					break;
				case HMSegmentedControlBorderType.Right:
					borderLayer.Frame = new CGRect(fullRect.Size.Width - borderWidth, 0, borderWidth, fullRect.Size.Height);
					break;
			}
			AddScrollViewSubLayer(borderLayer);
		}

		private void SetArrowFrame()
		{
			SelectionIndicatorArrowLayer.Frame = FrameForSelectionIndicator();
			SelectionIndicatorArrowLayer.Mask = null;

			CGPoint p1, p2, p3;
			if (SelectionIndicatorLocation == HMSegmentedControlIndicatorLocation.Down)
			{
				p1 = new CGPoint(SelectionIndicatorArrowLayer.Bounds.Size.Width / 2, 0);
				p2 = new CGPoint(0, SelectionIndicatorArrowLayer.Bounds.Size.Height);
				p3 = new CGPoint(SelectionIndicatorArrowLayer.Bounds.Size.Width, SelectionIndicatorArrowLayer.Bounds.Size.Height);
			}
			else if (SelectionIndicatorLocation == HMSegmentedControlIndicatorLocation.Up)
			{
				p1 = new CGPoint(SelectionIndicatorArrowLayer.Bounds.Size.Width / 2, SelectionIndicatorArrowLayer.Bounds.Size.Height);
				p2 = new CGPoint(SelectionIndicatorArrowLayer.Bounds.Size.Width, 0);
				p3 = new CGPoint(0, 0);
			}
			else
			{
				p1 = p2 = p3 = CGPoint.Empty;
			}

			var arrowPath = new UIBezierPath();
			arrowPath.MoveTo(p1);
			arrowPath.AddLineTo(p2);
			arrowPath.AddLineTo(p3);
			arrowPath.ClosePath();

			SelectionIndicatorArrowLayer.Mask = new CAShapeLayer
			{
				Frame = SelectionIndicatorArrowLayer.Bounds,
				Path = arrowPath.CGPath
			};
		}

		private CGRect FrameForSelectionIndicator()
		{
			var indicatorYOffset = (nfloat)0.0f;

			if (SelectionIndicatorLocation == HMSegmentedControlIndicatorLocation.Down)
				indicatorYOffset = Bounds.Size.Height - SelectionIndicatorHeight + selectionIndicatorEdgeInsets.Bottom;
			else if (SelectionIndicatorLocation == HMSegmentedControlIndicatorLocation.Up)
				indicatorYOffset = selectionIndicatorEdgeInsets.Top;

			var sectionWidth = (nfloat)0.0f;

			switch (type)
			{
				case HMSegmentedControlType.Text:
					sectionWidth = MeasureTitle(SelectedIndex).Width;
					break;
				case HMSegmentedControlType.Image:
					sectionWidth = sectionImages[SelectedIndex].Size.Width;
					break;
				case HMSegmentedControlType.TextAndImage:
					var stringWidth = MeasureTitle(SelectedIndex).Width;
					var imageWidth = sectionImages[SelectedIndex].Size.Width;
					sectionWidth = (nfloat)Math.Max(stringWidth, imageWidth);
					break;
			}

			if (SelectionStyle == HMSegmentedControlSelectionStyle.Arrow)
			{
				var widthToEndOfSelectedSegment = (segmentWidth * SelectedIndex) + segmentWidth;
				var widthToStartOfSelectedIndex = segmentWidth * SelectedIndex;
				var x = widthToStartOfSelectedIndex + ((widthToEndOfSelectedSegment - widthToStartOfSelectedIndex) / 2) - (SelectionIndicatorHeight / 2);
				return new CGRect(x - (SelectionIndicatorHeight / 2), indicatorYOffset, SelectionIndicatorHeight * 2, SelectionIndicatorHeight);
			}
			else
			{
				if (SelectionStyle == HMSegmentedControlSelectionStyle.TextWidthStripe &&
					sectionWidth <= segmentWidth &&
					segmentWidthStyle != HMSegmentedControlWidthStyle.Dynamic)
				{
					var widthToEndOfSelectedSegment = (segmentWidth * SelectedIndex) + segmentWidth;
					var widthToStartOfSelectedIndex = segmentWidth * SelectedIndex;
					var x = ((widthToEndOfSelectedSegment - widthToStartOfSelectedIndex) / 2) + (widthToStartOfSelectedIndex - sectionWidth / 2);
					return new CGRect(x + selectionIndicatorEdgeInsets.Left, indicatorYOffset, sectionWidth - selectionIndicatorEdgeInsets.Right, SelectionIndicatorHeight);
				}
				else
				{
					if (segmentWidthStyle == HMSegmentedControlWidthStyle.Dynamic)
					{
						var selectedSegmentOffset = (nfloat)0.0f;
						var i = 0;
						foreach (var width in segmentWidths)
						{
							if (SelectedIndex == i)
								break;
							selectedSegmentOffset += width;
							i++;
						}

						return new CGRect(selectedSegmentOffset + selectionIndicatorEdgeInsets.Left, indicatorYOffset, segmentWidths[SelectedIndex] - selectionIndicatorEdgeInsets.Right, SelectionIndicatorHeight + selectionIndicatorEdgeInsets.Bottom);
					}

					return new CGRect((segmentWidth + selectionIndicatorEdgeInsets.Left) * SelectedIndex, indicatorYOffset, segmentWidth - selectionIndicatorEdgeInsets.Right, SelectionIndicatorHeight);
				}
			}
		}

		private CGRect FrameForFillerSelectionIndicator()
		{
			if (segmentWidthStyle == HMSegmentedControlWidthStyle.Dynamic)
			{
				var selectedSegmentOffset = (nfloat)0.0f;
				int i = 0;

				foreach (var width in segmentWidths)
				{
					if (SelectedIndex == i)
						break;
					selectedSegmentOffset += width;
					i++;
				}

				return new CGRect(selectedSegmentOffset, 0, segmentWidths[SelectedIndex], Frame.Height);
			}

			return new CGRect(segmentWidth * SelectedIndex, 0, segmentWidth, Frame.Height);
		}

		public int SectionCount
		{
			get
			{
				switch (type)
				{
					case HMSegmentedControlType.Text:
						return sectionTitles.Count;
					case HMSegmentedControlType.Image:
					case HMSegmentedControlType.TextAndImage:
						return sectionImages.Count;
				}
				return 0;
			}
		}

		private void UpdateSegmentRects()
		{
			scrollView.ContentInset = UIEdgeInsets.Zero;
			scrollView.Frame = new CGRect(0, 0, Frame.Width, Frame.Height);

			if (SectionCount > 0)
				segmentWidth = Frame.Size.Width / SectionCount;

			if (type == HMSegmentedControlType.Text && segmentWidthStyle == HMSegmentedControlWidthStyle.Fixed)
			{
				for (int i = 0; i < sectionTitles.Count; i++)
				{
					var stringWidth = MeasureTitle(i).Width + SegmentEdgeInset.Left + SegmentEdgeInset.Right;
					segmentWidth = (nfloat)Math.Max(stringWidth, segmentWidth);
				}
			}
			else if (type == HMSegmentedControlType.Text && segmentWidthStyle == HMSegmentedControlWidthStyle.Dynamic)
			{
				segmentWidths = new List<float>();
				for (int i = 0; i < sectionTitles.Count; i++)
				{
					var stringWidth = MeasureTitle(i).Width + SegmentEdgeInset.Left + SegmentEdgeInset.Right;
					segmentWidths.Add((float)stringWidth);
				}
			}
			else if (type == HMSegmentedControlType.Image)
			{
				for (int i = 0; i < sectionImages.Count; i++)
				{
					var image = sectionImages[i];
					var imageWidth = image.Size.Width + SegmentEdgeInset.Left + SegmentEdgeInset.Right;
					segmentWidth = (nfloat)Math.Max(imageWidth, segmentWidth);
				}
			}
			else if (type == HMSegmentedControlType.TextAndImage && segmentWidthStyle == HMSegmentedControlWidthStyle.Fixed)
			{
				for (int i = 0; i < sectionTitles.Count; i++)
				{
					var stringWidth = MeasureTitle(i).Width + SegmentEdgeInset.Left + SegmentEdgeInset.Right;
					segmentWidth = (nfloat)Math.Max(stringWidth, segmentWidth);
				}
			}
			else if (type == HMSegmentedControlType.TextAndImage && segmentWidthStyle == HMSegmentedControlWidthStyle.Dynamic)
			{
				for (int i = 0; i < sectionImages.Count; i++)
				{
					var image = sectionImages[i];
					var stringWidth = MeasureTitle(i).Width + SegmentEdgeInset.Right;
					var imageWidth = image.Size.Width + SegmentEdgeInset.Left;
					var combinedWidth = Math.Max(imageWidth, stringWidth);
					segmentWidths.Add((float)combinedWidth);
				}
			}

			scrollView.ScrollEnabled = UserDraggable;
			scrollView.ContentSize = new CGSize(TotalSegmentControlWidth(), Frame.Size.Height);
		}

		private nfloat TotalSegmentControlWidth()
		{
			if (type == HMSegmentedControlType.Text && segmentWidthStyle == HMSegmentedControlWidthStyle.Fixed)
				return sectionTitles.Count * segmentWidth;
			else if (segmentWidthStyle == HMSegmentedControlWidthStyle.Dynamic)
				return segmentWidths.Sum();
			else
				return sectionImages.Count * segmentWidth;
		}

		#endregion

		#region Index Change

		private void SetSelectedSegmentIndex(int index, bool animated = false, bool notify = false)
		{
			SelectedIndex = index;
			SetNeedsDisplay();

			if (index == -1)
			{
				SelectionIndicatorArrowLayer.RemoveFromSuperLayer();
				SelectionIndicatorStripLayer.RemoveFromSuperLayer();
				SelectionIndicatorBoxLayer.RemoveFromSuperLayer();
			}
			else
			{
				ScrollToSelectedSegmentIndex(animated);

				if (animated)
				{
					if (SelectionStyle == HMSegmentedControlSelectionStyle.Arrow)
					{
						AddScrollViewSubLayer(SelectionIndicatorArrowLayer);
						SetSelectedSegmentIndex(index, false, true);
						return;
					}
					else
					{
						if (SelectionIndicatorStripLayer.SuperLayer == null)
						{
							AddScrollViewSubLayer(SelectionIndicatorStripLayer);
							if (SelectionStyle == HMSegmentedControlSelectionStyle.Box && SelectionIndicatorBoxLayer.SuperLayer == null)
								InsertScrollViewSubLayer(SelectionIndicatorBoxLayer, 0);
							SetSelectedSegmentIndex(index, false, true);
						}
					}

					if (notify)
						NotifyForSegmentChange(index);

					SelectionIndicatorArrowLayer.Actions = new NSDictionary();
					SelectionIndicatorStripLayer.Actions = new NSDictionary();
					SelectionIndicatorBoxLayer.Actions = new NSDictionary();

					CATransaction.Begin();
					CATransaction.AnimationDuration = 0.15f;
					CATransaction.AnimationTimingFunction = CAMediaTimingFunction.FromName(CAMediaTimingFunction.Linear);
					SetArrowFrame();
					SelectionIndicatorBoxLayer.Frame = FrameForSelectionIndicator();
					SelectionIndicatorStripLayer.Frame = FrameForSelectionIndicator();
					SelectionIndicatorBoxLayer.Frame = FrameForFillerSelectionIndicator();
					CATransaction.Commit();
				}
				else
				{
					var newActions = new NSMutableDictionary();
					newActions.Add(new NSString("position"), NSNull.Null);
					newActions.Add(new NSString("bounds"), NSNull.Null);

					SelectionIndicatorArrowLayer.Actions = newActions;
					SelectionIndicatorStripLayer.Actions = newActions;
					SelectionIndicatorBoxLayer.Actions = newActions;

					SelectionIndicatorStripLayer.Frame = FrameForSelectionIndicator();
					SelectionIndicatorBoxLayer.Frame = FrameForFillerSelectionIndicator();

					if (notify)
						NotifyForSegmentChange(index);
				}
			}
		}

		private void NotifyForSegmentChange(int index)
		{
			if (Superview != null)
				SendActionForControlEvents(UIControlEvent.ValueChanged);

			if (IndexChange != null)
				IndexChange(this, index);
		}

		private void ScrollToSelectedSegmentIndex(bool animated)
		{
			CGRect rectForSelectedIndex;
			var selectedSegmentOffset = (nfloat)0.0f;

			if (segmentWidthStyle == HMSegmentedControlWidthStyle.Fixed)
			{
				rectForSelectedIndex = new CGRect(segmentWidth * SelectedIndex, 0, segmentWidth, Frame.Size.Height);
				selectedSegmentOffset = (Frame.Width / 2) - (segmentWidth / 2);
			}
			else
			{
				var i = 0;
				var offsetter = 0.0f;
				foreach (var width in segmentWidths)
				{
					if (SelectedIndex == i)
						break;
					offsetter += width;
					i++;
				}

				rectForSelectedIndex = new CGRect(offsetter, 0, segmentWidths[SelectedIndex], Frame.Size.Height);
				selectedSegmentOffset = (Frame.Width / 2) - (segmentWidths[SelectedIndex] / 2);
			}

			var rectToScrollTo = rectForSelectedIndex;
			rectToScrollTo.X -= selectedSegmentOffset;
			rectToScrollTo.Size = new CGSize(selectedSegmentOffset * 2, rectToScrollTo.Size.Height);
			scrollView.ScrollRectToVisible(rectToScrollTo, animated);
		}

		#endregion

		#region Touch

		public override void TouchesEnded(NSSet touches, UIEvent evt)
		{
			var touch = (UITouch)touches.AnyObject;
			var touchLocation = touch.LocationInView(this);

			if (!Bounds.Contains(touchLocation))
				return;

			var segment = 0;
			if (segmentWidthStyle == HMSegmentedControlWidthStyle.Fixed)
				segment = (int)Math.Truncate((touchLocation.X + scrollView.ContentOffset.X) / segmentWidth);
			else
			{
				var widthLeft = touchLocation.X + scrollView.ContentOffset.X;
				foreach (var width in segmentWidths)
				{
					widthLeft -= width;
					if (widthLeft <= 0)
						break;
					segment++;
				}
			}

			if (segment != SelectedIndex && segment < SectionCount)
			{
				if (TouchEnabled)
					SetSelectedSegmentIndex(segment, true, true);
			}
		}

		#endregion

		#region Styling

		private UIStringAttributes GetTitleTextAttributes()
		{

			var attributes = new UIStringAttributes();
			if (Font != null)
				attributes.Font = Font;
			if (TextColor != null)
				attributes.ForegroundColor = TextColor;
			return attributes;
		}

		private UIStringAttributes GetSelectedTitleTextAttributes()
		{
			var attributes = new UIStringAttributes();
			if (Font != null)
				attributes.Font = Font;
			if (SelectedTextColor != null)
				attributes.ForegroundColor = SelectedTextColor;
			return attributes;
		}

		#endregion

		#region Dispose

		protected override void Dispose(bool disposing)
		{
			ClearScrollViewSubLayers();

			SelectionIndicatorBoxLayer.Dispose();
			SelectionIndicatorStripLayer.Dispose();
			SelectionIndicatorArrowLayer.Dispose();

			foreach (var view in Subviews)
			{
				view.RemoveFromSuperview();
				view.Dispose();
			}

			base.Dispose(disposing);
		}

		#endregion
	}

	public class DisposableCALayer : CALayer
	{
		protected override void Dispose(bool disposing)
		{
			Console.WriteLine("[DISPOSE] DisposableCALayer");
			base.Dispose(disposing);
		}
	}

	public class DisposableCATextLayer : CATextLayer
	{
		protected override void Dispose(bool disposing)
		{
			Console.WriteLine("[DISPOSE] DisposableCATextLayer");
			base.Dispose(disposing);
		}
	}
}
