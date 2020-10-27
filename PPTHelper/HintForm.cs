using System;
using System.Drawing;
using System.Media;
using System.Windows.Forms;
using WinFormAnimation;

namespace PPTHelper
{
    public partial class HintForm : Form
    {
        private IController Controller;
        public HintForm(Point anchor, IController controller)
        {
            Controller = controller;
            InitializeComponent();

            Location = new Point(anchor.X, anchor.Y - Height);
            // Fade in
            Opacity = 0f;
            new Animator(new Path(0f, 1f, 100), FPSLimiterKnownValues.LimitSixty)
                .Play(this, Animator.KnownProperties.Opacity);

            controller.Focus();
        }

        private bool drawStress = false;
        protected override void OnPaintBackground(PaintEventArgs e)
        {
            base.OnPaintBackground(e);
            var pen = new Pen(Color.Blue, 10f);
            void draw()
            {
                Point[] points = {
                    new Point(0, 0), new Point(Width, 0),
                    new Point(Width, Height), new Point(0, Height),
                    new Point(0, 0)
                };
                e.Graphics.DrawLines(pen, points);
            }

            if (drawStress)
            {
                var timer = new System.Timers.Timer()
                {
                    AutoReset = true,
                    Interval = 500
                };
                var count = 0;
                timer.Elapsed += (s, _) =>
                {
                    if (count >= 4)
                    {
                        timer.Dispose();
                        return;
                    }
                    pen = new Pen(count % 2 == 0 ? Color.Blue : Color.Red, 10f);
                    draw();
                    count++;
                };
                timer.Enabled = true;
                drawStress = false;
            } else
            {
                draw();
            }
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            Close();
            Controller.Focus();
            Dispose();
        }
    }
}
