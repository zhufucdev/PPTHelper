using System;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.CompilerServices;
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
        private Size defaultSize;
        public HelperForm(IController controller)
        {
            this.controller = controller;
            InitializeComponent();

            var bound = Screen.PrimaryScreen.Bounds;
            commonTop = bound.Height - Height;
            Location = new Point((bound.Width - Width) / 2, commonTop);
            StartPosition = FormStartPosition.Manual;
            CheckForIllegalCrossThreadCalls = false;
            defaultSize = Size;

            pinned = Settings.Default.fix;
            checkPinBoxImage();

            controller.ToolSelectionChanged += Controller_ToolSelectionChanged;
            controller.SlideShowChanged += Controller_SlideShowChanged;

            controller.Focus();
        }

        protected override void OnHandleDestroyed(EventArgs e)
        {
            base.OnHandleDestroyed(e);

            controller.ToolSelectionChanged -= Controller_ToolSelectionChanged;
            controller.SlideShowChanged -= Controller_SlideShowChanged;
            hintForm?.Close();
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

        private static readonly Color defaultBlurColor = Color.FromArgb(31, Control.DefaultBackColor);
        private static readonly Color defaultInactiveColor = Color.FromArgb(31, Color.Black);
        private Color blurColor = defaultBlurColor;
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
            if (!IsUp())
            {
                SlipUp();
                return;
            }
            controller.Previous();
            controller.Focus();
        }

        private DateTime lastRightBoxClicked = DateTime.Now;
        private int countRightBoxClicked = 0;
        private void rightBox_Click(object sender, EventArgs e)
        {
            if (!IsUp())
            {
                SlipUp();
                return;
            }
            if (!skipToolShown)
            {
                controller.Next();
                if ((DateTime.Now - lastRightBoxClicked).TotalMilliseconds <= 700)
                    countRightBoxClicked++;
                if (countRightBoxClicked >= 3)
                {
                    ShowSkipTool();
                    countRightBoxClicked = 0;
                }
                lastRightBoxClicked = DateTime.Now;
                controller.Focus();
            } 
            else
            {
                HideSkipTool();
                controller.Switch();
            }
        }

        private Animator skipToolAminator;
        private bool skipToolShown = false;
        private SafeInvoker<float> SetAlpha(PictureBox pictureBox)
        {
            var bmpIn = (Bitmap)pictureBox.BackgroundImage.Clone();
            Bitmap setBitmap(int alpha)
            {
                Bitmap bmpOut = new Bitmap(bmpIn.Width, bmpIn.Height);
                float a = alpha / 255f;
                Rectangle r = new Rectangle(0, 0, bmpIn.Width, bmpIn.Height);

                float[][] matrixItems = {
                    new float[] {1, 0, 0, 0, 0},
                    new float[] {0, 1, 0, 0, 0},
                    new float[] {0, 0, 1, 0, 0},
                    new float[] {0, 0, 0, a, 0},
                    new float[] {0, 0, 0, 0, 1}
                };

                ColorMatrix colorMatrix = new ColorMatrix(matrixItems);

                ImageAttributes imageAtt = new ImageAttributes();
                imageAtt.SetColorMatrix(colorMatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);

                using (Graphics g = Graphics.FromImage(bmpOut))
                    g.DrawImage(bmpIn, r, r.X, r.Y, r.Width, r.Height, GraphicsUnit.Pixel, imageAtt);

                return bmpOut;
            }
            return new SafeInvoker<float>((v) => pictureBox.BackgroundImage = setBitmap((int)(255 * v)));
        }

        private System.Timers.Timer skipHideTimer;
        private void ShowSkipTool()
        {
            lock (this)
            {
                if (skipToolShown) return;
                skipToolShown = true;
                skipToolAminator?.Stop();

                var path = new Path(1f, 0f, 200);
                skipToolAminator = new Animator(path, FPSLimiterKnownValues.LimitSixty);
                skipToolAminator.Play(SetAlpha(rightBox), 
                    new SafeInvoker(() =>
                    {
                        rightBox.BackgroundImage = Properties.Resources.skip_next;
                        path = new Path(0, 1, 200);
                        skipToolAminator = new Animator(path, FPSLimiterKnownValues.LimitSixty);
                        skipToolAminator.Play(SetAlpha(rightBox),
                            new SafeInvoker(() => skipToolAminator = null)
                        );
                    }));

                skipHideTimer = new System.Timers.Timer()
                {
                    Interval = 1400,
                    AutoReset = false
                };
                skipHideTimer.Elapsed += (e, s) => HideSkipTool();
                skipHideTimer.Start();
            }
            
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        private void HideSkipTool()
        {
            lock (this)
            {
                skipHideTimer?.Stop();
                if (!skipToolShown) return;
                skipToolAminator?.Stop();

                var path = new Path(1, 0, 200);

                skipToolAminator = new Animator(path, FPSLimiterKnownValues.LimitSixty);
                skipToolAminator.Play(SetAlpha(rightBox),
                    new SafeInvoker(() =>
                    {
                        rightBox.BackgroundImage = Properties.Resources.next;
                        path = new Path(0, 1, 200);
                        skipToolAminator = new Animator(path, FPSLimiterKnownValues.LimitSixty);
                        skipToolAminator.Play(SetAlpha(rightBox),
                            new SafeInvoker(() => skipToolAminator = null)
                        );
                    }));
                skipToolShown = false;
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            if (!IsUp())
            {
                SlipUp();
                return;
            }
            controller.Exit();
        }

        private PenOptionForm penForm;
        private bool mArcray = true;
        public bool AcraylicEnabled
        {
            get => mArcray;
            set
            {
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
            var location = Location;
            location.Y = commonTop;
            if (controller.HasText(location, Size) && !pinned)
            {
                SlipDown();
            }
            else
            {
                SlipUp();
            }
        }
        private Animator presentAnimator;
        private bool isHintShown = false;
        private HintForm hintForm;
        private bool IsUp()
        {
            return Top == commonTop;
        }
        private void SlipDown()
        {
            if (!IsUp()) return;
            var path = new Path(Top, (int)(commonTop + Height * 0.8), 500);
            if (presentAnimator?.CurrentStatus == AnimatorStatus.Playing)
            {
                if (presentAnimator.ActivePath.End == path.End) return;
                presentAnimator.Stop();
            }

            presentAnimator = new Animator(path, FPSLimiterKnownValues.LimitSixty);
            presentAnimator.Play(new SafeInvoker<float>((v) => Top = (int)v));

            if (!isHintShown)
            {
                isHintShown = true;
                hintForm = new HintForm(new Point(Location.X, (int)path.End), controller);
                hintForm.Show();
                hintForm.FormClosed += (s, e) => hintForm = null;
                Blink();
            } else {
                // Change color for visibility
                blurColor = defaultInactiveColor;
                AcraylicEnabled = true;
            }
        }

        private void Blink()
        {
            var timer = new System.Timers.Timer()
            {
                AutoReset = true,
                Interval = 1000
            };
            var count = 0;
            timer.Elapsed += (e, s) =>
            {
                if (IsUp())
                {
                    timer.Stop();
                    blurColor = defaultBlurColor;
                    AcraylicEnabled = true;
                    return;
                }
                blurColor = count % 2 == 0 ? Color.Red : defaultInactiveColor;
                AcraylicEnabled = true;
                count++;
                if (count > 5) timer.Stop();
            };
            timer.Start();
        }

        private void SlipUp()
        {
            if (IsUp()) return;
            var path = new Path(Top, commonTop, 300);
            if (presentAnimator?.CurrentStatus == AnimatorStatus.Playing)
            {
                if (presentAnimator.ActivePath.End == path.End) return;
                presentAnimator.Stop();
            }
            presentAnimator = new Animator(path, FPSLimiterKnownValues.LimitSixty);
            presentAnimator.Play(new SafeInvoker<float>((v) => Top = (int)v));

            if (hintForm != null)
            {
                hintForm.Close();
            }
            blurColor = defaultBlurColor;
            AcraylicEnabled = true;
        }

        private Color defaultPenColor = Color.Red;
        private void ShowPenForm()
        {
            if (penForm != null && !penForm.IsDisposed)
            {
                penForm.Close();
                penForm.Dispose();
                penForm = null;
                return;
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
            if (!IsUp())
            {
                SlipUp();
                return;
            }
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
            if (!IsUp())
            {
                SlipUp();
                return;
            }
            controller.ToolSelection = new CursorSelection();
            controller.Focus();
        }

        private void eraserBox_Click(object sender, EventArgs e)
        {
            if (!IsUp())
            {
                SlipUp();
                return;
            }
            controller.ToolSelection = new EraserSelection();
            controller.Focus();
        }

        private void penOptionBox_Click(object sender, EventArgs e)
        {
            if (!IsUp())
            {
                SlipUp();
                return;
            }
            ShowPenForm();
        }

        private void checkPinBoxImage()
        {
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
        }
        private void pinBox_Click(object sender, EventArgs e)
        {
            if (!IsUp())
            {
                SlipUp();
                return;
            }
            pinned = !pinned;
            checkPinBoxImage();

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
