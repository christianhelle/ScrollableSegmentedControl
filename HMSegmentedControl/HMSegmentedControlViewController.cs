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

            var segmentedControl = new HMSegmentedControl(new List<string>{"JUKE", "Most Popular", "All", "Followed"});
            segmentedControl.Frame = new RectangleF(0, 20, View.Frame.Width, 50);
            segmentedControl.SelectionIndicatorHeight = 4.0f;
            segmentedControl.SelectionStyle = HMSegmentedControlSelectionStyle.Box;
            segmentedControl.BackgroundColor = UIColor.Black;
            segmentedControl.TextColor = UIColor.White;
            segmentedControl.SelectionIndicatorColor = UIColor.Yellow;
            segmentedControl.Font = UIFont.FromName("STHeitiSC-Light", 18.0f);
            segmentedControl.ShouldAnimateUserSelection = false;
            segmentedControl.SelectionIndicatorLocation = HMSegmentedControlIndicatorLocation.Down;
            View.AddSubview(segmentedControl);
        }
    }
}