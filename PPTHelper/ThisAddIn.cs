using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using PowerPoint = Microsoft.Office.Interop.PowerPoint;
using Office = Microsoft.Office.Core;
using System.Drawing;
using Microsoft.Office.Interop.PowerPoint;
using Microsoft.Office.Core;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Threading;

namespace PPTHelper
{
    public class Controller: IController
    {
        private readonly Size SlideShowMargin;

        private readonly PowerPoint.SlideShowView view;
        private readonly PowerPoint.SlideShowWindow window;
        public Controller(PowerPoint.SlideShowWindow window)
        {
            view = window.View;
            this.window = window;
            window.Application.SlideShowOnNext += (s) => SlideShowChanged.Invoke(this, s);
            window.Application.SlideShowOnPrevious += (s) => SlideShowChanged.Invoke(this, s);
            window.Application.SlideShowNextSlide += (s) => SlideShowChanged.Invoke(this, s);


            var pageWidth = window.Application.ActivePresentation.PageSetup.SlideWidth;
            var pageHeight = window.Application.ActivePresentation.PageSetup.SlideHeight;
            var screenSize = Screen.PrimaryScreen.Bounds;
            double rateWidth = screenSize.Width / pageWidth;
            double rateHeight = screenSize.Height / pageHeight;
            
            if (rateWidth < rateHeight)
            {
                SlideShowMargin = new Size(0, (int)(screenSize.Height - pageHeight * rateWidth) / 2);
            }
            else
            {
                SlideShowMargin = new Size((int) (screenSize.Width - pageWidth * rateHeight) / 2, 0);
            }
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

        public override event EventHandler<ISelection> ToolSelectionChanged;

        private ISelection mSelection;
        public override ISelection ToolSelection {
            get => mSelection;
            set {
                if (value is CursorSelection)
                {
                    view.PointerType = PowerPoint.PpSlideShowPointerType.ppSlideShowPointerAutoArrow;
                }
                else if (value is PenSelection)
                {
                    view.PointerType = PowerPoint.PpSlideShowPointerType.ppSlideShowPointerPen;
                    var color = Color.FromArgb((value as PenSelection).RGB);
                    var bgr = (int)color.B;
                    bgr = (bgr << 8) + color.G;
                    bgr = (bgr << 8) + color.R;

                    view.PointerColor.RGB = bgr;
                }
                else if (value is EraserSelection)
                {
                    view.PointerType = PowerPoint.PpSlideShowPointerType.ppSlideShowPointerEraser;
                }
                
                mSelection = value;
                ToolSelectionChanged.Invoke(this, value);
            }
        }

        public override event EventHandler<object> SlideShowChanged;

        public override bool HasText(System.Drawing.Point position, Size size)
        {
            bool calc()
            {
                try
                {
                    var shapes = view.Slide.Shapes;
                    var slideHeight = view.Application.ActivePresentation.PageSetup.SlideHeight;
                    var slideWidth = view.Application.ActivePresentation.PageSetup.SlideWidth;
                    var screenSize = Screen.PrimaryScreen.Bounds;
                    foreach (PowerPoint.Shape shape in shapes)
                    {
                        if (shape.Visible == MsoTriState.msoFalse) continue;
                        var status = shape.TextFrame.HasText;
                        if (status == MsoTriState.msoTrue || status == MsoTriState.msoCTrue)
                        {
                            var fixedLeft = shape.Left / slideWidth * screenSize.Width + SlideShowMargin.Width;
                            var fixedTop = shape.Top / slideHeight * screenSize.Height + SlideShowMargin.Height;
                            var shapeBound = new Rectangle((int)fixedLeft, (int)fixedTop, (int)shape.Width, (int)shape.Height);
                            return new Rectangle(position, size).IntersectsWith(shapeBound);
                        }
                    }
                }
                catch (COMException)
                {
                    Thread.Sleep(300);
                    return calc();
                }
                return false;
            }
            
            return calc();
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
            if (Wn.IsFullScreen == MsoTriState.msoFalse) return;
            if (helper.ContainsKey(Wn.Presentation))
            {
                helper[Wn.Presentation].Close();
            }
            var form = new HelperForm(new Controller(Wn));
            
            form.Show();
            helper[Wn.Presentation] = form;

            new Thread(() =>
            {
                Thread.Sleep(400);
                Wn.Activate();
            }).Start();
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
