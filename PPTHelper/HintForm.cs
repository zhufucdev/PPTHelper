using System;
using System.Drawing;
using System.Windows.Forms;
using WinFormAnimation;

namespace PPTHelper
{
    public partial class HintForm : Form
    {
        private System.Timers.Timer Timer;
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

            Timer = new System.Timers.Timer()
            {
                Interval = 10000,
                AutoReset = false
            };
            Timer.Elapsed += (s, e) =>
            {
                Close();
                Dispose();
            };
            Timer.Enabled = true;

            controller.Focus();
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            base.OnPaintBackground(e);
            var pen = new Pen(Color.Blue, 10f);
            Point[] points = { 
                new Point(0, 0), new Point(Width, 0),
                new Point(Width, Height), new Point(0, Height),
                new Point(0, 0)
            };
            e.Graphics.DrawLines(pen, points);
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            Close();
            Controller.Focus();
            Dispose();
        }
    }
}
