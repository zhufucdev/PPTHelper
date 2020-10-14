using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using WinFormAnimation;

namespace PPTHelper
{
    public partial class HelperForm : Form
    {
        private readonly IController controller;
        private bool pinned = false;
        private readonly int commonTop;
        public HelperForm(IController controller)
        {
            this.controller = controller;
            InitializeComponent();

            var bound = Screen.PrimaryScreen.Bounds;
            commonTop = bound.Height - Height;
            Location = new Point((bound.Width - Width) / 2, commonTop);
            StartPosition = FormStartPosition.Manual;
            CheckForIllegalCrossThreadCalls = false;

            controller.ToolSelectionChanged += Controller_ToolSelectionChanged;
            controller.SlideShowChanged += Controller_SlideShowChanged;

            controller.Focus();
        }

        protected override void OnHandleDestroyed(EventArgs e)
        {
            base.OnHandleDestroyed(e);

            controller.ToolSelectionChanged -= Controller_ToolSelectionChanged;
            controller.SlideShowChanged -= Controller_SlideShowChanged;
        }

        private void Controller_SlideShowChanged(object sender, object e) => InvalidateContextAwarePosition();

        private void Controller_ToolSelectionChanged(object sender, ISelection e)
        {
            if (e is CursorSelection)
            {
                penBox.BackgroundImage = Properties.Resources.pencil;
                cursorBox.BackgroundImage = Properties.Resources.cursor_selected;
                eraserBox.BackgroundImage = Properties.Resources.eraser;
            } 
            else if (e is PenSelection)
            {
                penBox.BackgroundImage = Properties.Resources.pencil_selected;
                cursorBox.BackgroundImage = Properties.Resources.cursor;
                eraserBox.BackgroundImage = Properties.Resources.eraser;
            }
            else
            {
                penBox.BackgroundImage = Properties.Resources.pencil;
                cursorBox.BackgroundImage = Properties.Resources.cursor;
                eraserBox.BackgroundImage = Properties.Resources.eraser_selected;
            }
        }

        private readonly Color blurColor = Color.FromArgb(31, Control.DefaultBackColor);
        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            WindowUtils.EnableAcrylic(this, blurColor);
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            base.OnPaintBackground(e);
            e.Graphics.Clear(blurColor);
        }

        private void leftBox_Click(object sender, EventArgs e)
        {
            controller.Previous();
            controller.Focus();
        }

        private void rightBox_Click(object sender, EventArgs e)
        {
            controller.Next();
            controller.Focus();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            controller.Exit();
        }

        private PenOptionForm penForm;
        private bool mArcray = true;
        public bool AcraylicEnabled
        {
            get => mArcray;
            set
            {
                if (value == mArcray) return;
                mArcray = value;
                if (value)
                {
                    WindowUtils.EnableAcrylic(this, blurColor);
                    Invalidate();
                }
                else
                {
                    WindowUtils.EnableAcrylic(this, Color.FromArgb(255, blurColor));
                    Invalidate();
                }
            }
        }

        private void InvalidateContextAwarePosition()
        {
            if (controller.HasText(Location, Size) && !pinned)
            {
                SlipDown();
            }
            else
            {
                SlipUp();
            }
        }
        private Animator presentAnimator;
        private void SlipDown()
        {
            if (Top != commonTop) return;
            var path = new Path(Top, (int)(commonTop + Height * 0.8), 500);
            if (presentAnimator?.CurrentStatus == AnimatorStatus.Playing)
            {
                if (presentAnimator.ActivePath.End == path.End) return;
                presentAnimator.Stop();
            }

            presentAnimator = new Animator(path, FPSLimiterKnownValues.LimitSixty);
            presentAnimator.Play(new SafeInvoker<float>((v) => Top = (int)v));
        }

        private void SlipUp()
        {
            if (Top == commonTop) return;
            var path = new Path(Top, commonTop, 500);
            if (presentAnimator?.CurrentStatus == AnimatorStatus.Playing)
            {
                if (presentAnimator.ActivePath.End == path.End) return;
                presentAnimator.Stop();
            }
            presentAnimator = new Animator(path, FPSLimiterKnownValues.LimitSixty);
            presentAnimator.Play(new SafeInvoker<float>((v) => Top = (int)v));
        }

        private Color defaultPenColor = Color.Red;
        private void ShowPenForm()
        {
            if (penForm != null && !penForm.IsDisposed)
            {
                penForm.Close();
                penForm.Dispose();
            }
            var anchor = new Point(Location.X + penBox.Location.X, Location.Y);
            penForm = new PenOptionForm(anchor, controller);
            penForm.Show();
            // Disable acrylic
            AcraylicEnabled = false;
            penForm.FormClosed += (s, e) =>
            {
                AcraylicEnabled = true;
                // Lose focus
                new Thread(controller.Focus).Start();
            };
            penForm.ColorSelect += (s, e) => defaultPenColor = (s as ColorPanel.ColorChunk).Color;
        }

        private void penBox_Click(object sender, EventArgs e)
        {
            if (controller.ToolSelection is PenSelection)
            {
                ShowPenForm();
            }
            else
            {
                controller.ToolSelection = new PenSelection(defaultPenColor.ToArgb());
                controller.Focus();
            }
        }

        private void cursorBox_Click(object sender, EventArgs e)
        {
            controller.ToolSelection = new CursorSelection();
            controller.Focus();
        }

        private void eraserBox_Click(object sender, EventArgs e)
        {
            controller.ToolSelection = new EraserSelection();
            controller.Focus();
        }

        private void penOptionBox_Click(object sender, EventArgs e)
        {
            ShowPenForm();
        }

        private void pinBox_Click(object sender, EventArgs e)
        {
            pinned = !pinned;
            if (pinned)
            {
                pinBox.Image = Properties.Resources.pin_selected;
                SlipUp();
            }
            else
            {
                pinBox.Image = Properties.Resources.pin;
                InvalidateContextAwarePosition();
            }

            controller.Focus();
        }

        private void HelperForm_MouseEnter(object sender, EventArgs e)
        {
            SlipUp();
        }

        private void HelperForm_MouseLeave(object sender, EventArgs e)
        {
            var timer = new System.Timers.Timer()
            {
                AutoReset = false,
                Interval = 2000,
                Enabled = true
            };
            timer.Elapsed += (_, s) => InvalidateContextAwarePosition();
        }
    }
}
