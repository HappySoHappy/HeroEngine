using System.ComponentModel;
using System.Drawing.Drawing2D;

namespace HeroEngine.Desktop.Design
{
    public partial class BorderPanel : System.Windows.Forms.Panel
    {
        private Color borderColor = Color.Black;
        private int borderRadius = 7;
        private int borderWidth = 1;
        private AnchorStyles borderRadiusSides = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;

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
        public int BorderWidth
        {
            get => borderWidth;
            set
            {
                borderWidth = Math.Max(0, value);
                Invalidate();
            }
        }

        [Category("Appearance")]
        public AnchorStyles BorderRadiusSides
        {
            get => borderRadiusSides;
            set
            {
                borderRadiusSides = value;
                Invalidate();
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var rectBorderSmooth = ClientRectangle;
            var rectBorder = Rectangle.Inflate(rectBorderSmooth, -BorderWidth, -BorderWidth);
            int smoothSize = BorderWidth > 0 ? BorderWidth : 1;

            if (BorderRadius <= 1)
            {
                using (Pen penBorder = new Pen(borderColor, BorderWidth))
                {
                    Region = new Region(ClientRectangle);
                    penBorder.Alignment = PenAlignment.Inset;
                    e.Graphics.DrawRectangle(penBorder, 0, 0, Width - 0.5F, Height - 0.5F);
                }

                return;
            }

            using (GraphicsPath pathBorderSmooth = CreateRoundedRectanglePath(rectBorderSmooth, BorderRadius))
            using (GraphicsPath pathBorder = CreateRoundedRectanglePath(rectBorder, BorderRadius - BorderWidth))
            using (Pen penBorderSmooth = new Pen(Parent?.BackColor ?? Color.Transparent, smoothSize))
            using (Pen penBorder = new Pen(borderColor, BorderWidth))
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
            return CreateRoundedRectanglePath(rect, radius, BorderRadiusSides);

            GraphicsPath path = new GraphicsPath();
            int diameter = radius * 2;

            path.AddArc(rect.X, rect.Y, diameter, diameter, 180, 90);
            path.AddArc(rect.Right - diameter, rect.Y, diameter, diameter, 270, 90);
            path.AddArc(rect.Right - diameter, rect.Bottom - diameter, diameter, diameter, 0, 90);
            path.AddArc(rect.X, rect.Bottom - diameter, diameter, diameter, 90, 90);
            path.CloseFigure();

            return path;
        }

        private GraphicsPath CreateRoundedRectanglePath(Rectangle rect, int radius, AnchorStyles sides)
        {
            GraphicsPath path = new GraphicsPath();
            int diameter = radius * 2;

            // Top-left corner
            if (sides.HasFlag(AnchorStyles.Top) && sides.HasFlag(AnchorStyles.Left))
            {
                path.AddArc(rect.X, rect.Y, diameter, diameter, 180, 90);
            }
            else
            {
                path.AddLine(rect.X, rect.Y, rect.X + radius, rect.Y);
            }

            // Top-right corner
            if (sides.HasFlag(AnchorStyles.Top) && sides.HasFlag(AnchorStyles.Right))
            {
                path.AddArc(rect.Right - diameter, rect.Y, diameter, diameter, 270, 90);
            }
            else
            {
                path.AddLine(rect.Right - radius, rect.Y, rect.Right, rect.Y);
            }

            // Bottom-right corner
            if (sides.HasFlag(AnchorStyles.Bottom) && sides.HasFlag(AnchorStyles.Right))
            {
                path.AddArc(rect.Right - diameter, rect.Bottom - diameter, diameter, diameter, 0, 90);
            }
            else
            {
                path.AddLine(rect.Right, rect.Bottom - radius, rect.Right, rect.Bottom);
            }

            // Bottom-left corner
            if (sides.HasFlag(AnchorStyles.Bottom) && sides.HasFlag(AnchorStyles.Left))
            {
                path.AddArc(rect.X, rect.Bottom - diameter, diameter, diameter, 90, 90);
            }
            else
            {
                path.AddLine(rect.X + radius, rect.Bottom, rect.X, rect.Bottom);
            }

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
