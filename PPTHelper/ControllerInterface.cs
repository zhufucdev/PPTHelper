using System;
using System.Drawing;

namespace PPTHelper
{
    public abstract class IController
    {
        public abstract ISelection ToolSelection { set; get; }
        public abstract event EventHandler<ISelection> ToolSelectionChanged;

        public abstract event EventHandler<object> SlideShowChanged;

        public abstract void Next();
        public abstract void Previous();
        public abstract void Exit();

        public abstract void Focus();

        public abstract bool HasText(Point position, Size size);
    }

    public interface ISelection { }

    public class CursorSelection: ISelection
    {
        public CursorSelection()
        {
        }
    }

    public class PenSelection: ISelection
    {
        public int RGB;
        public PenSelection(int RGB)
        {
            this.RGB = RGB;
        }
    }

    public class EraserSelection : ISelection
    {
        public EraserSelection()
        {
        }
    }
}
