using System;
using CoreGraphics;
using UIKit;

namespace ChristianHelle.Controls.iOS.Samples
{
    public partial class SegmentedControlViewController : UIViewController
    {
        private readonly string[] sectionTitles = 
        {
            "Cool",
            "Great",
            "Awesome",
            "Not bad",
            "Terrible"
        };

        public SegmentedControlViewController(IntPtr handle)
            : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            Title = "Scrollable Segmented Control Demo";
            View.BackgroundColor = UIColor.Black;
            EdgesForExtendedLayout = UIRectEdge.None;

            CreateSegmentedControlWithTextWidthStripeUp(20);
            CreateSegmentedControlWithFullWidthStripeUp(100);

            CreateSegmentedControlWithTextWidthStripeDown(180);
            CreateSegmentedControlWithFullWidthStripeDown(260);

            CreateSegmentedControlWithBoxDown(340);
            CreateSegmentedControlWithBoxUp(420);
        }

        void CreateSegmentedControlWithTextWidthStripeDown(float y)
        {
            var segmentedControl = new ScrollableSegmentedControl(sectionTitles);
            segmentedControl.Font = UIFont.FromName("STHeitiSC-Light", 18.0f);
            segmentedControl.Frame = new CGRect(0, y, View.Frame.Width, 40);
            segmentedControl.SegmentEdgeInset = new UIEdgeInsets(0, 10, 0, 10);
            segmentedControl.SelectionStyle = ScrollableSegmentedControlSelectionStyle.TextWidthStripe;
            segmentedControl.SelectionIndicatorLocation = ScrollableSegmentedControlIndicatorLocation.Down;
            segmentedControl.SelectionIndicatorColor = UIColor.Yellow;
            segmentedControl.TextColor = UIColor.White;
            segmentedControl.SelectedTextColor = UIColor.Yellow;
            View.AddSubview(segmentedControl);
        }

        void CreateSegmentedControlWithFullWidthStripeDown(float y)
        {
            var segmentedControl = new ScrollableSegmentedControl(sectionTitles);
            segmentedControl.Font = UIFont.FromName("STHeitiSC-Light", 18.0f);
            segmentedControl.Frame = new CGRect(0, y, View.Frame.Width, 40);
            segmentedControl.SegmentEdgeInset = new UIEdgeInsets(0, 10, 0, 10);
            segmentedControl.SelectionStyle = ScrollableSegmentedControlSelectionStyle.FullWidthStripe;
            segmentedControl.SelectionIndicatorLocation = ScrollableSegmentedControlIndicatorLocation.Down;
            segmentedControl.SelectionIndicatorColor = UIColor.Yellow;
            segmentedControl.TextColor = UIColor.White;
            segmentedControl.SelectedTextColor = UIColor.Yellow;
            View.AddSubview(segmentedControl);
        }

        void CreateSegmentedControlWithTextWidthStripeUp(float y)
        {
            var segmentedControl = new ScrollableSegmentedControl(sectionTitles);
            segmentedControl.Font = UIFont.FromName("STHeitiSC-Light", 18.0f);
            segmentedControl.Frame = new CGRect(0, y, View.Frame.Width, 40);
            segmentedControl.SegmentEdgeInset = new UIEdgeInsets(0, 10, 0, 10);
            segmentedControl.SelectionStyle = ScrollableSegmentedControlSelectionStyle.TextWidthStripe;
            segmentedControl.SelectionIndicatorLocation = ScrollableSegmentedControlIndicatorLocation.Up;
            segmentedControl.SelectionIndicatorColor = UIColor.Yellow;
            segmentedControl.TextColor = UIColor.White;
            segmentedControl.SelectedTextColor = UIColor.Yellow;
            View.AddSubview(segmentedControl);
        }

        void CreateSegmentedControlWithFullWidthStripeUp(float y)
        {
            var segmentedControl = new ScrollableSegmentedControl(sectionTitles);
            segmentedControl.Font = UIFont.FromName("STHeitiSC-Light", 18.0f);
            segmentedControl.Frame = new CGRect(0, y, View.Frame.Width, 40);
            segmentedControl.SegmentEdgeInset = new UIEdgeInsets(0, 10, 0, 10);
            segmentedControl.SelectionStyle = ScrollableSegmentedControlSelectionStyle.FullWidthStripe;
            segmentedControl.SelectionIndicatorLocation = ScrollableSegmentedControlIndicatorLocation.Up;
            segmentedControl.SelectionIndicatorColor = UIColor.Yellow;
            segmentedControl.TextColor = UIColor.White;
            segmentedControl.SelectedTextColor = UIColor.Yellow;
            View.AddSubview(segmentedControl);
        }

        void CreateSegmentedControlWithBoxUp(float y)
        {
            var segmentedControl = new ScrollableSegmentedControl(sectionTitles);
            segmentedControl.Font = UIFont.FromName("STHeitiSC-Light", 18.0f);
            segmentedControl.Frame = new CGRect(0, y, View.Frame.Width, 40);
            segmentedControl.SegmentEdgeInset = new UIEdgeInsets(0, 10, 0, 10);
            segmentedControl.SelectionStyle = ScrollableSegmentedControlSelectionStyle.Box;
            segmentedControl.SelectionIndicatorLocation = ScrollableSegmentedControlIndicatorLocation.Up;
            segmentedControl.SelectionIndicatorColor = UIColor.Yellow;
            segmentedControl.TextColor = UIColor.White;
            segmentedControl.SelectedTextColor = UIColor.Yellow;
            View.AddSubview(segmentedControl);
        }

        void CreateSegmentedControlWithBoxDown(float y)
        {
            var segmentedControl = new ScrollableSegmentedControl(sectionTitles);
            segmentedControl.Font = UIFont.FromName("STHeitiSC-Light", 18.0f);
            segmentedControl.Frame = new CGRect(0, y, View.Frame.Width, 40);
            segmentedControl.SegmentEdgeInset = new UIEdgeInsets(0, 10, 0, 10);
            segmentedControl.SelectionStyle = ScrollableSegmentedControlSelectionStyle.Box;
            segmentedControl.SelectionIndicatorLocation = ScrollableSegmentedControlIndicatorLocation.Down;
            segmentedControl.SelectionIndicatorColor = UIColor.Yellow;
            segmentedControl.TextColor = UIColor.White;
            segmentedControl.SelectedTextColor = UIColor.Yellow;
            View.AddSubview(segmentedControl);
        }
    }
}
