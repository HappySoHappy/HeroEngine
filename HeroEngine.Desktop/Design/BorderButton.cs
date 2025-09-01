using System.ComponentModel;
using System.Drawing.Drawing2D;

namespace HeroEngine.Desktop.Design
{
    [DefaultEvent("Click")]
    public partial class BorderButton : UserControl
    {
        private Color backColorMouseOver = Color.Black;
        private Color backColorMouseDown = Color.Black;

        private Color borderColor = Color.Black;
        private Color borderColorMouseOver = Color.Black;
        private Color borderColorMouseDown = Color.Black;

        private int borderRadius = 7;
        private int borderWidth = 1;

        private bool isMouseOver = false;
        private bool isMouseDown = false;

        private string text = string.Empty;

        [Category("Appearance")]
        public Color BackColorMouseOver
        {
            get => backColorMouseOver;
            set
            {
                backColorMouseOver = value;
                Invalidate();
            }
        }

        [Category("Appearance")]
        public Color BackColorMouseDown
        {
            get => backColorMouseDown;
            set
            {
                backColorMouseDown = value;
                Invalidate();
            }
        }

        [Category("Appearance")]
        [Description("The color")]
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
        public Color BorderColorMouseOver
        {
            get => borderColorMouseOver;
            set
            {
                borderColorMouseOver = value;
                Invalidate();
            }
        }

        [Category("Appearance")]
        public Color BorderColorMouseDown
        {
            get => borderColorMouseDown;
            set
            {
                borderColorMouseDown = value;
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
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Bindable(true)]
        [Browsable(true)]
        public override string Text
        {
            get => text;
            set
            {
                text = value;
                Invalidate(); // Redraw the control
            }
        }

        protected override Size DefaultSize => new Size(150, 30);

        protected override void OnPaint(PaintEventArgs e)
        {
            var rectBorderSmooth = ClientRectangle;
            var rectBorder = Rectangle.Inflate(rectBorderSmooth, -BorderWidth, -BorderWidth);
            int smoothSize = BorderWidth > 0 ? BorderWidth : 1;

            Color backColor = isMouseDown ? BackColorMouseDown
                : isMouseOver ? BackColorMouseOver : BackColor;

            Color borderColor = isMouseDown ? BorderColorMouseDown
                : isMouseOver ? BorderColorMouseOver : BorderColor;

            using (Pen penBorder = new Pen(backColor, Height))
            {
                e.Graphics.DrawRectangle(penBorder, 0, 0, Width, Height);
            }

            if (BorderRadius <= 1)
            {
                using (Pen penBorder = new Pen(borderColor, BorderWidth))
                {
                    Region = new Region(ClientRectangle);
                    penBorder.Alignment = PenAlignment.Inset;
                    e.Graphics.DrawRectangle(penBorder, 0, 0, Width - 0.5F, Height - 0.5F);
                }
            }
            else
            {
                using (GraphicsPath pathBorderSmooth = CreateRoundedRectanglePath(rectBorderSmooth, BorderRadius))
                using (GraphicsPath pathBorder = CreateRoundedRectanglePath(rectBorder, BorderRadius - BorderWidth))
                using (Pen penBorderSmooth = new Pen(Parent?.BackColor ?? Color.Transparent, smoothSize))
                using (Pen penBorder = new Pen(borderColor, BorderWidth))
                {
                    Region = new Region(pathBorderSmooth);
                    e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                    penBorder.Alignment = PenAlignment.Inset;

                    e.Graphics.DrawPath(penBorderSmooth, pathBorderSmooth);
                    e.Graphics.DrawPath(penBorder, pathBorder);
                }
            }

            if (!string.IsNullOrEmpty(Text))
            {
                using (Brush textBrush = new SolidBrush(ForeColor))
                {
                    StringFormat stringFormat = new StringFormat
                    {
                        Alignment = StringAlignment.Center,
                        LineAlignment = StringAlignment.Center
                    };
                    e.Graphics.DrawString(Text, Font, textBrush, ClientRectangle, stringFormat);
                }
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

        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            isMouseOver = true;
            Invalidate();
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            isMouseOver = false;
            isMouseDown = false;
            Invalidate();
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (e.Button == MouseButtons.Left)
            {
                isMouseDown = true;
                Invalidate();
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            if (e.Button == MouseButtons.Left)
            {
                isMouseDown = false;
                Invalidate();
            }
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
        #endregion
    }
}
