using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WinFormAnimation;

namespace PPTHelper
{
    public partial class PenOptionForm : Form
    {
        private IController controller;
        public PenOptionForm(Point anchor, IController controller)
        {
            InitializeComponent();
            // Scale
            var cellSize = colorPanel1.Controls[0].Height;
            Height = cellSize * 2 + 20;
            Width = cellSize * 8 + 20;

            var location = new Point(anchor.X, anchor.Y - Height);
            Location = new Point(anchor.X, (int)(anchor.Y - Height * 0.5));
            // Fade in
            new Animator(new Path(location.Y + Height * 0.5f, location.Y, 100, AnimationFunctions.ExponentialEaseIn), FPSLimiterKnownValues.LimitSixty)
                .Play(new SafeInvoker<float>((v) => Location = new Point(location.X, (int)v)));
            Opacity = 0f;
            new Animator(new Path(0f, 1f, 100), FPSLimiterKnownValues.LimitSixty)
                .Play(this, Animator.KnownProperties.Opacity);
            this.controller = controller;

            // Restore selected color
            if (controller.ToolSelection is PenSelection)
                colorPanel1.ColorSelcted = Color.FromArgb((controller.ToolSelection as PenSelection).RGB);
            colorPanel1.ColorSelect += ColorPanel1_ColorSelect;
        }

        public event EventHandler ColorSelect;
        private void ColorPanel1_ColorSelect(object sender, EventArgs e)
        {
            var color = (sender as ColorPanel.ColorChunk).Color;
            controller.ToolSelection = new PenSelection(color.ToArgb());
            ColorSelect.Invoke(sender, e);

            Close();
            Dispose();
        }

        private void PenOptionForm_Deactivate(object sender, EventArgs e)
        {
            Close();
            Dispose();
        }
    }
}
