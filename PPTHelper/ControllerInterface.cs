namespace PPTHelper
{
    public abstract class IController
    {
        public abstract ISelection ToolSelection { set; get; }

        public abstract void Next();
        public abstract void Previous();
        public abstract void Exit();

        public abstract void Focus();
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
