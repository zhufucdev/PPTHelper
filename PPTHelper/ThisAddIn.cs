using System;
using System.Collections.Generic;
using PowerPoint = Microsoft.Office.Interop.PowerPoint;
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

        private readonly SlideShowView view;
        private readonly SlideShowWindow window;
        public Controller(SlideShowWindow window)
        {
            view = window.View;
            this.window = window;
            window.Application.SlideShowOnPrevious += (s) => SlideShowChanged?.Invoke(this, s);
            window.Application.SlideShowNextSlide += (s) => SlideShowChanged?.Invoke(this, s);

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

        public override void Switch()
        {
            Focus();
            SendKeys.Send("G");
        }

        public override void Previous()
        {
            view.Previous();
        }

        public static ISelection LastSelection;
        public override void Exit()
        {
            view.Exit();
            if (Settings.Default.keep)
            {
                LastSelection = this.ToolSelection;
            }
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
            try
            {
                var shapes = view.Slide.Shapes;
                var slideHeight = view.Application.ActivePresentation.PageSetup.SlideHeight;
                var slideWidth = view.Application.ActivePresentation.PageSetup.SlideWidth;
                var screenSize = Screen.PrimaryScreen.Bounds;
                foreach (PowerPoint.Shape shape in shapes)
                {
                    if (shape.Visible == MsoTriState.msoFalse ||
                        (shape.HasTextFrame == MsoTriState.msoFalse
                        && shape.HasChart == MsoTriState.msoFalse)) continue;

                    var fixedLeft = shape.Left / slideWidth * screenSize.Width + SlideShowMargin.Width;
                    var fixedTop = shape.Top / slideHeight * screenSize.Height + SlideShowMargin.Height;
                    var fixedWidth = shape.Width / slideWidth * (screenSize.Width - SlideShowMargin.Width * 2);
                    var fixedHeight = shape.Height / slideHeight * (screenSize.Height - SlideShowMargin.Height * 2);
                    var shapeBound = new Rectangle((int)fixedLeft, (int)fixedTop, (int)fixedWidth, (int)fixedHeight);
                    var intersect = new Rectangle(position, size).IntersectsWith(shapeBound);
                    if (intersect)
                        return true;
                }
            }
            catch (COMException)
            {
            }
            return false;
        }
    }
    public partial class ThisAddIn
    {
        public Dictionary<Presentation, HelperForm> Helper = new Dictionary<Presentation, HelperForm>();

        private void ThisAddIn_Shutdown(object sender, EventArgs e)
        {
        }
        
        private void Application_SlideShowBegin(SlideShowWindow Wn)
        {
            if (Wn.IsFullScreen == MsoTriState.msoFalse) return;
            if (Helper.ContainsKey(Wn.Presentation))
            {
                Helper[Wn.Presentation].Close();
            }
            var controller = new Controller(Wn);
            var form = new HelperForm(controller);
            
            form.Show();
            Helper[Wn.Presentation] = form;

            new Thread(() =>
            {
                var last = Controller.LastSelection;
                if (last != null)
                {
                    controller.ToolSelection = last;
                }
                // Active the presentation
                Thread.Sleep(400);
                Wn.Activate();
            }).Start();
        }

        private void Application_SlideShowEnd(Presentation Pres)
        {
            if (!Helper.ContainsKey(Pres)) return;
            Helper[Pres].Close();
        }

        protected override IRibbonExtensibility CreateRibbonExtensibilityObject()
        {
            return new Ribbon();
        }

        #region VSTO 生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InternalStartup()
        {
            this.Shutdown += new System.EventHandler(ThisAddIn_Shutdown);
            Application.SlideShowBegin += Application_SlideShowBegin;
            Application.SlideShowEnd += Application_SlideShowEnd;
        }
        #endregion
    }
}
