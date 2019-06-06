using Foundation;
using UIKit;

namespace ChristianHelle.Controls.iOS
{
    public class ScrollableScrollView : UIScrollView
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
}