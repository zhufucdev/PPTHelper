using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace PPTHelper
{
    public class ColorPanel : FlowLayoutPanel
    {
        public ColorPanel()
        {   
        }

        public event EventHandler ColorSelect;

        private Color mSelected;
        public Color ColorSelcted
        {
            get => mSelected;
            set
            {
                foreach (ColorChunk c in Controls)
                {
                    if (c.Color.ToArgb() == value.ToArgb())
                    {
                        c.BorderColor = Color.DarkGray;
                    } else
                    {
                        c.BorderColor = Color.Transparent;
                    }
                }
                mSelected = value;
            }
        }

        protected override void OnResize(System.EventArgs eventargs)
        {
            base.OnResize(eventargs);
            Relayout();
        }

        private int mCellSize = 40;
        public int CellSize
        {
            get => mCellSize;
            set
            {
                mCellSize = value;
                Relayout();
            }
        }

        private readonly List<Color> colors = new List<Color>()
        {
            Color.White, Color.Black, Color.Brown, Color.RosyBrown,
            Color.Orange, Color.OrangeRed, Color.Red, Color.Yellow,
            Color.YellowGreen, Color.Green, Color.Lime, Color.Aqua,
            Color.Blue, Color.BlueViolet, Color.Purple, Color.MediumPurple
        };
        
        private void Relayout()
        {
            this.SuspendLayout();
            Controls.Clear();
            var size = CellSize - 5;
            for (int i = 0; i < colors.Count; i++)
            {
                var color = colors.Count > i ? colors[i] : Color.Red;
                ColorChunk chunk = new ColorChunk
                {
                    Margin = new Padding(0),
                    Size = new Size(CellSize, CellSize),
                    Color = color,
                    BorderColor = color.ToArgb() == this.ColorSelcted.ToArgb() ? Color.DarkGray 
                                : Color.Transparent
                };

                chunk.MouseEnter += Chunk_MouseEnter;
                chunk.MouseLeave += Chunk_MouseLeave;
                chunk.Click += Chunk_Click;

                Controls.Add(chunk);
            }
            this.ResumeLayout();
        }

        private void Chunk_Click(object sender, EventArgs e)
        {
            ColorSelect.Invoke(sender, e);
        }

        private void Chunk_MouseLeave(object sender, System.EventArgs e)
        {
            var c = (sender as ColorChunk);
            c.BorderColor = c.Color.ToArgb() == ColorSelcted.ToArgb() ? Color.DarkGray 
                : Color.Transparent;
        }

        private void Chunk_MouseEnter(object sender, System.EventArgs e)
        {
            (sender as ColorChunk).BorderColor = Color.Gray;
        }

        public class ColorChunk: Control
        {
            public ColorChunk()
            {
            }


            private Color mColor = Color.Red;
            public Color Color
            {
                get => mColor;
                set
                {
                    mColor = value;
                    if (this.Created)
                        Invalidate();
                }
            }

            private Color mBorderColor = Color.White;
            public Color BorderColor
            {
                get => mBorderColor;
                set
                {
                    mBorderColor = value;
                    if (this.Created)
                        Invalidate();
                }
            }

            protected override void OnPaint(PaintEventArgs e)
            {
                base.OnPaint(e);
                var g = e.Graphics;
                var size = Height < Width ? Height : Width;
                g.DrawRectangle(
                    new Pen(BorderColor, 10f),
                    new Rectangle(0, 0, size, size)
                );
                g.FillRectangle(
                    new SolidBrush(Color),
                    new Rectangle(5, 5, size - 10, size - 10)
                );
            }
        }
    }
}
