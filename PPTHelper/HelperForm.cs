using System;
using System.Drawing;
using System.Windows.Forms;

namespace PPTHelper
{
    public partial class HelperForm : Form
    {
        private IController controller;
        public HelperForm(IController controller)
        {
            this.controller = controller;
            InitializeComponent();

            var bound = Screen.PrimaryScreen.Bounds;
            Location = new Point((bound.Width - Width) / 2, bound.Height - Height);
            StartPosition = FormStartPosition.Manual;

            controller.Focus();
        }

        private readonly Color blurColor = Color.FromArgb(32, Color.White);
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

        private void penBox_Click(object sender, EventArgs e)
        {
            penBox.BackgroundImage = Properties.Resources.pencil_selected;
            cursorBox.BackgroundImage = Properties.Resources.cursor;
            eraserBox.BackgroundImage = Properties.Resources.eraser;

            if (controller.ToolSelection is PenSelection)
            {

            }
            else
            {
                var color = Color.Red;
                var rgb = (int)color.B;
                rgb = (rgb << 8) + color.G;
                rgb = (rgb << 8) + color.R;

                controller.ToolSelection = new PenSelection(rgb);
                controller.Focus();
            }
        }

        private void cursorBox_Click(object sender, EventArgs e)
        {
            penBox.BackgroundImage = Properties.Resources.pencil;
            cursorBox.BackgroundImage = Properties.Resources.cursor_selected;
            eraserBox.BackgroundImage = Properties.Resources.eraser;
            controller.ToolSelection = new CursorSelection();
            controller.Focus();
        }

        private void eraserBox_Click(object sender, EventArgs e)
        {
            penBox.BackgroundImage = Properties.Resources.pencil;
            cursorBox.BackgroundImage = Properties.Resources.cursor;
            eraserBox.BackgroundImage = Properties.Resources.eraser_selected;
            controller.ToolSelection = new EraserSelection();
            controller.Focus();
        }
    }
}
