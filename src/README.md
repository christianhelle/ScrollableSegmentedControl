# ScrollableSegmentedControl

A drop-in replacement for UISegmentedControl mimicking the style of the segmented control used in Google Currents and various other Google products.

A C# port for Xamarin.iOS of HMSegmentedControl written by Hesham Abd-Elmegid (https://github.com/HeshamMegid/HMSegmentedControl). 

## Screenshot

![ScrollableSegmentedControl](https://raw.githubusercontent.com/christianhelle/scrollablesegmentedcontrol/master/images/screenshot.png)

## Usages

The easiest way to get started with using this is by installing the [ScrollableSegmentedControl NuGet package](https://www.nuget.org/packages/scrollablesegmentedcontrol) to your Xamarin.iOS project

Here's an example of how to create the ScrollableSegmentedControl

``` csharp
private void CreateScrollableSegmentedControl()
{
    var sectionTitles = new[] { "One", "Two", "Three", "Four", "Five", "Six" };
    var segmentedControl = new ScrollableSegmentedControl(sectionTitles)
    {
        Font = UIFont.FromName("STHeitiSC-Light", 18.0f),
        Frame = new CGRect(0, 60, View.Frame.Width, 40),
        SegmentEdgeInset = new UIEdgeInsets(0, 10, 0, 10),
        SelectionStyle = ScrollableSegmentedControlSelectionStyle.FullWidthStripe,
        SelectionIndicatorLocation = ScrollableSegmentedControlIndicatorLocation.Down
    };
    View.AddSubview(segmentedControl);
}
```

Selection Indicator Position

``` csharp
enum ScrollableSegmentedControlIndicatorLocation
{
    Up,
    Down,
    None
}
```

Selection Style

``` csharp
enum ScrollableSegmentedControlSelectionStyle
{
    TextWidthStripe,
    FullWidthStripe,
    Box,
    Arrow
}
```

Segment Width Styles

``` csharp
enum ScrollableSegmentedControlWidthStyle
{
    Fixed,
    Dynamic
}
```


For tips and tricks on software development, check out [my blog](https://christianhelle.com)

If you find this useful and feel a bit generous then feel free to [buy me a coffee](https://www.buymeacoffee.com/christianhelle) :)
