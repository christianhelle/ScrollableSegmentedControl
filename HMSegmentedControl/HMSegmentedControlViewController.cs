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

            CreateSegmentedControlWithTextWidthStripeUp(20);
            CreateSegmentedControlWithFullWidthStripeUp(100);

            CreateSegmentedControlWithTextWidthStripeDown(180);
            CreateSegmentedControlWithFullWidthStripeDown(260);

            CreateSegmentedControlWithBoxDown(340);
            CreateSegmentedControlWithBoxUp(420);
        }

        void CreateSegmentedControlWithTextWidthStripeDown(float y)
        {
            var segmentedControl = new HMSegmentedControl(new List<string> {
                "JUKE",
                "Most Popular",
                "All",
                "Bookmarked"
            });
            segmentedControl.Font = UIFont.FromName("STHeitiSC-Light", 18.0f);
            segmentedControl.Frame = new RectangleF(0, y, View.Frame.Width, 40);
            segmentedControl.SegmentEdgeInset = new UIEdgeInsets(0, 10, 0, 10);
            segmentedControl.SelectionStyle = HMSegmentedControlSelectionStyle.TextWidthStripe;
            segmentedControl.SelectionIndicatorLocation = HMSegmentedControlIndicatorLocation.Down;
            segmentedControl.SelectionIndicatorColor = UIColor.Yellow;
            segmentedControl.TextColor = UIColor.White;
            segmentedControl.SelectedTextColor = UIColor.Yellow;
            View.AddSubview(segmentedControl);
        }

        void CreateSegmentedControlWithFullWidthStripeDown(float y)
        {
            var segmentedControl = new HMSegmentedControl(new List<string> {
                "JUKE",
                "Most Popular",
                "All",
                "Bookmarked"
            });
            segmentedControl.Font = UIFont.FromName("STHeitiSC-Light", 18.0f);
            segmentedControl.Frame = new RectangleF(0, y, View.Frame.Width, 40);
            segmentedControl.SegmentEdgeInset = new UIEdgeInsets(0, 10, 0, 10);
            segmentedControl.SelectionStyle = HMSegmentedControlSelectionStyle.FullWidthStripe;
            segmentedControl.SelectionIndicatorLocation = HMSegmentedControlIndicatorLocation.Down;
            segmentedControl.SelectionIndicatorColor = UIColor.Yellow;
            segmentedControl.TextColor = UIColor.White;
            segmentedControl.SelectedTextColor = UIColor.Yellow;
            View.AddSubview(segmentedControl);
        }

        void CreateSegmentedControlWithTextWidthStripeUp(float y)
        {
            var segmentedControl = new HMSegmentedControl(new List<string> {
                "JUKE",
                "Most Popular",
                "All",
                "Bookmarked"
            });
            segmentedControl.Font = UIFont.FromName("STHeitiSC-Light", 18.0f);
            segmentedControl.Frame = new RectangleF(0, y, View.Frame.Width, 40);
            segmentedControl.SegmentEdgeInset = new UIEdgeInsets(0, 10, 0, 10);
            segmentedControl.SelectionStyle = HMSegmentedControlSelectionStyle.TextWidthStripe;
            segmentedControl.SelectionIndicatorLocation = HMSegmentedControlIndicatorLocation.Up;
            segmentedControl.SelectionIndicatorColor = UIColor.Yellow;
            segmentedControl.TextColor = UIColor.White;
            segmentedControl.SelectedTextColor = UIColor.Yellow;
            View.AddSubview(segmentedControl);
        }

        void CreateSegmentedControlWithFullWidthStripeUp(float y)
        {
            var segmentedControl = new HMSegmentedControl(new List<string> {
                "JUKE",
                "Most Popular",
                "All",
                "Bookmarked"
            });
            segmentedControl.Font = UIFont.FromName("STHeitiSC-Light", 18.0f);
            segmentedControl.Frame = new RectangleF(0, y, View.Frame.Width, 40);
            segmentedControl.SegmentEdgeInset = new UIEdgeInsets(0, 10, 0, 10);
            segmentedControl.SelectionStyle = HMSegmentedControlSelectionStyle.FullWidthStripe;
            segmentedControl.SelectionIndicatorLocation = HMSegmentedControlIndicatorLocation.Up;
            segmentedControl.SelectionIndicatorColor = UIColor.Yellow;
            segmentedControl.TextColor = UIColor.White;
            segmentedControl.SelectedTextColor = UIColor.Yellow;
            View.AddSubview(segmentedControl);
        }

        void CreateSegmentedControlWithBoxUp(float y)
        {
            var segmentedControl = new HMSegmentedControl(new List<string> {
                "JUKE",
                "Most Popular",
                "All",
                "Bookmarked"
            });
            segmentedControl.Font = UIFont.FromName("STHeitiSC-Light", 18.0f);
            segmentedControl.Frame = new RectangleF(0, y, View.Frame.Width, 40);
            segmentedControl.SegmentEdgeInset = new UIEdgeInsets(0, 10, 0, 10);
            segmentedControl.SelectionStyle = HMSegmentedControlSelectionStyle.Box;
            segmentedControl.SelectionIndicatorLocation = HMSegmentedControlIndicatorLocation.Up;
            segmentedControl.SelectionIndicatorColor = UIColor.Yellow;
            segmentedControl.TextColor = UIColor.White;
            segmentedControl.SelectedTextColor = UIColor.Yellow;
            View.AddSubview(segmentedControl);
        }

        void CreateSegmentedControlWithBoxDown(float y)
        {
            var segmentedControl = new HMSegmentedControl(new List<string> {
                "JUKE",
                "Most Popular",
                "All",
                "Bookmarked"
            });
            segmentedControl.Font = UIFont.FromName("STHeitiSC-Light", 18.0f);
            segmentedControl.Frame = new RectangleF(0, y, View.Frame.Width, 40);
            segmentedControl.SegmentEdgeInset = new UIEdgeInsets(0, 10, 0, 10);
            segmentedControl.SelectionStyle = HMSegmentedControlSelectionStyle.Box;
            segmentedControl.SelectionIndicatorLocation = HMSegmentedControlIndicatorLocation.Down;
            segmentedControl.SelectionIndicatorColor = UIColor.Yellow;
            segmentedControl.TextColor = UIColor.White;
            segmentedControl.SelectedTextColor = UIColor.Yellow;
            View.AddSubview(segmentedControl);
        }
    }
}