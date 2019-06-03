using System;
using CoreAnimation;

namespace ChristianHelle.Controls.iOS
{
    public class DisposableCALayer : CALayer
    {
        protected override void Dispose(bool disposing)
        {
            Console.WriteLine("[DISPOSE] DisposableCALayer");
            base.Dispose(disposing);
        }
    }
}