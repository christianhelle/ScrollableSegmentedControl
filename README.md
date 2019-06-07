[![Build Status](https://christianhelle.visualstudio.com/ScrollableSegmentedControl/_apis/build/status/CI%20Build?branchName=master)](https://christianhelle.visualstudio.com/ScrollableSegmentedControl/_build/latest?definitionId=22&branchName=master) [![NuGet](https://img.shields.io/nuget/v/scrollablesegmentedcontrol.svg?style=flat-square)]( tp://www.nuget.org/packages/scrollablesegmentedcontrol)

# ScrollableSegmentedControl

A drop-in replacement for UISegmentedControl mimicking the style of the segmented control used in Google Currents and various other Google products.

A C# port for Xamarin.iOS of HMSegmentedControl written by Hesham Abd-Elmegid (https://github.com/HeshamMegid/HMSegmentedControl). 

## Usages

Here's an example of how to create the ScrollableSegmentedControl

```
private void CreateScrollableSegmentedControl()
{
    var segmentedControl = new ScrollableSegmentedControl(sectionTitles);
    segmentedControl.Font = UIFont.FromName("STHeitiSC-Light", 18.0f);
    segmentedControl.Frame = new CGRect(0, 20, View.Frame.Width, 40);
    segmentedControl.SegmentEdgeInset = new UIEdgeInsets(0, 10, 0, 10);
    segmentedControl.SelectionStyle = ScrollableSegmentedControlSelectionStyle.TextWidthStripe;
    segmentedControl.SelectionIndicatorLocation = ScrollableSegmentedControlIndicatorLocation.Down;
    segmentedControl.SelectionIndicatorColor = UIColor.Yellow;
    segmentedControl.TextColor = UIColor.White;
    segmentedControl.SelectedTextColor = UIColor.Yellow;
    View.AddSubview(segmentedControl);
}
```

Selection Indicator Position

```
enum ScrollableSegmentedControlIndicatorLocation
{
    Up,
    Down,
    None
}
```

Selection Style

```
enum ScrollableSegmentedControlSelectionStyle
{
    TextWidthStripe,
    FullWidthStripe,
    Box,
    Arrow
}
```

Segment Width Styles

```
enum ScrollableSegmentedControlWidthStyle
{
    Fixed,
    Dynamic
}
```

