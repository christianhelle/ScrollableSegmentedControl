using System;
using CoreAnimation;

namespace ChristianHelle.Controls.iOS
{
    public class DisposableCATextLayer : CATextLayer
    {
        protected override void Dispose(bool disposing)
        {
            Console.WriteLine("[DISPOSE] DisposableCATextLayer");
            base.Dispose(disposing);
        }
    }
}