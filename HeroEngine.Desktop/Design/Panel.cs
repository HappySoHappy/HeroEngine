using System.ComponentModel;
using System.Drawing.Drawing2D;

namespace HeroEngine.Desktop.Design
{
    public partial class Panel : System.Windows.Forms.Panel
    {
        private int borderRadius = 7;
        private Color borderColor = Color.Black;

        [Category("Appearance")]
        public int BorderRadius
        {
            get => borderRadius;
            set
            {
                borderRadius = Math.Max(0, value);
                Invalidate();
            }
        }

        [Category("Appearance")]
        public Color BorderColor
        {
            get => borderColor;
            set
            {
                borderColor = value;
                Invalidate();
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var rectBorderSmooth = ClientRectangle;
            var rectBorder = Rectangle.Inflate(rectBorderSmooth, -1, -1);

            if (BorderRadius <= 1)
            {
                using (Pen penBorder = new Pen(BackColor, Height))
                {
                    Region = new Region(ClientRectangle);
                    penBorder.Alignment = PenAlignment.Inset;
                    e.Graphics.DrawRectangle(penBorder, 0, 0, Width - 0.5F, Height - 0.5F);
                }

                return;
            }

            using (GraphicsPath pathBorderSmooth = CreateRoundedRectanglePath(rectBorderSmooth, BorderRadius))
            using (GraphicsPath pathBorder = CreateRoundedRectanglePath(rectBorder, BorderRadius - 1))
            using (Pen penBorderSmooth = new Pen(Parent?.BackColor ?? Color.Transparent, 1))
            using (Pen penBorder = new Pen(BorderColor, 1))
            {
                Region = new Region(pathBorderSmooth);
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                penBorder.Alignment = PenAlignment.Outset;

                e.Graphics.DrawPath(penBorderSmooth, pathBorderSmooth);
                e.Graphics.DrawPath(penBorder, pathBorder);
            }
        }

        private GraphicsPath CreateRoundedRectanglePath(Rectangle rect, int radius)
        {
            GraphicsPath path = new GraphicsPath();
            int diameter = radius * 2;

            path.AddArc(rect.X, rect.Y, diameter, diameter, 180, 90);
            path.AddArc(rect.Right - diameter, rect.Y, diameter, diameter, 270, 90);
            path.AddArc(rect.Right - diameter, rect.Bottom - diameter, diameter, diameter, 0, 90);
            path.AddArc(rect.X, rect.Bottom - diameter, diameter, diameter, 90, 90);
            path.CloseFigure();

            return path;
        }

        #region hide properties
        [Browsable(false)]
        public new Image BackgroundImage
        {
            get => base.BackgroundImage;
            set => base.BackgroundImage = value;
        }

        [Browsable(false)]
        public new ImageLayout BackgroundImageLayout
        {
            get => base.BackgroundImageLayout;
            set => base.BackgroundImageLayout = value;
        }

        [Browsable(false)]
        public new BorderStyle BorderStyle
        {
            get => base.BorderStyle;
            set => base.BorderStyle = value;
        }
        #endregion
    }
}
