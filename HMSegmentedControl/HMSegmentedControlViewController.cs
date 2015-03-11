using System;
using MonoTouch.UIKit;
using System.Drawing;
using System.Collections.Generic;
using MonoTouch.Foundation;

namespace HMSegmentedControlSample
{
    public partial class HMSegmentedControlViewController : UIViewController
    {
        public HMSegmentedControlViewController(IntPtr handle)
            : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            Title = "Segmented Control Demo";
            View.BackgroundColor = UIColor.Black;
            EdgesForExtendedLayout = UIRectEdge.None;

            var segmentedControl = new HMSegmentedControl(new List<string>{"JUKE", "Top", "All", "Mine"});
            segmentedControl.Font = UIFont.FromName("STHeitiSC-Light", 18.0f);
            segmentedControl.Frame = new RectangleF(0, 20, View.Frame.Width, 40);
            segmentedControl.SegmentEdgeInset = new UIEdgeInsets(0, 10, 0, 10);
            segmentedControl.SelectionStyle = HMSegmentedControlSelectionStyle.TextWidthStripe;
            segmentedControl.SelectionIndicatorLocation = HMSegmentedControlIndicatorLocation.Up;
            segmentedControl.SelectionIndicatorColor = UIColor.Yellow;
            segmentedControl.TextColor = UIColor.White;
            segmentedControl.SelectedTextColor = UIColor.Yellow;
            segmentedControl.SegmentWidthStyle = HMSegmentedControlWidthStyle.Fixed;
//            segmentedControl.VerticalDividerEnabled = true;
//            segmentedControl.VerticalDividerColor = UIColor.Yellow;
//            segmentedControl.VerticalDividerWidth = 1.0f;
            View.AddSubview(segmentedControl);
        }
    }
}