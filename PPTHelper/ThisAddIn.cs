using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using PowerPoint = Microsoft.Office.Interop.PowerPoint;
using Office = Microsoft.Office.Core;

namespace PPTHelper
{
    public class Controller: IController
    {
        private readonly PowerPoint.SlideShowView view;
        private readonly PowerPoint.SlideShowWindow window;
        public Controller(PowerPoint.SlideShowWindow window)
        {
            this.view = window.View;
            this.window = window;
        }

        public override void Next()
        {
            view.Next();
        }

        public override void Previous()
        {
            view.Previous();
        }

        public override void Exit()
        {
            view.Exit();
        }

        public override void Focus()
        {
            window.Activate();
        }

        private ISelection mSelection;
        public override ISelection ToolSelection {
            get => mSelection;
            set {
                if (value is CursorSelection)
                {
                    view.PointerType = PowerPoint.PpSlideShowPointerType.ppSlideShowPointerArrow;
                }
                else if (value is PenSelection)
                {
                    view.PointerType = PowerPoint.PpSlideShowPointerType.ppSlideShowPointerPen;
                    view.PointerColor.RGB = (value as PenSelection).RGB;
                }
                else if (value is EraserSelection)
                {
                    view.PointerType = PowerPoint.PpSlideShowPointerType.ppSlideShowPointerEraser;
                }
                mSelection = value;
            }
        }
    }
    public partial class ThisAddIn
    {
        private Dictionary<PowerPoint.Presentation, HelperForm> helper = new Dictionary<PowerPoint.Presentation, HelperForm>();

        private void ThisAddIn_Startup(object sender, System.EventArgs e)
        {
            
        }

        private void ThisAddIn_Shutdown(object sender, System.EventArgs e)
        {
        }
        
        private void Application_SlideShowBegin(PowerPoint.SlideShowWindow Wn)
        {
            if (helper.ContainsKey(Wn.Presentation))
            {
                helper[Wn.Presentation].Close();
            }
            var form = new HelperForm(new Controller(Wn));
            
            form.Show();
            helper[Wn.Presentation] = form;
        }

        private void Application_SlideShowEnd(PowerPoint.Presentation Pres)
        {
            if (!helper.ContainsKey(Pres)) return;
            helper[Pres].Close();
        }

        #region VSTO 生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InternalStartup()
        {
            this.Startup += new System.EventHandler(ThisAddIn_Startup);
            this.Shutdown += new System.EventHandler(ThisAddIn_Shutdown);
            Application.SlideShowBegin += Application_SlideShowBegin;
            Application.SlideShowEnd += Application_SlideShowEnd;
        }
        #endregion
    }
}
