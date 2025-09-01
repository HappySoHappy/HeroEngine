using System.ComponentModel;
using System.Drawing.Drawing2D;

namespace HeroEngine.Desktop.Design
{
    [DefaultEvent("Click")]
    public partial class Button : UserControl
    {
        private Color backColorMouseOver = Color.Black;
        private Color backColorMouseDown = Color.Black;
        private bool isMouseOver = false;
        private bool isMouseDown = false;

        private Image? backImage;
        private ImageLayout backImageLayout = ImageLayout.Stretch;

        private string _text = string.Empty;

        private int borderRadius = 7;

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
        [DefaultValue(null)]
        [Browsable(true)]
        [Editor(typeof(System.Drawing.Design.ImageEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public Image? BackImage
        {
            get => backImage;
            set
            {
                backImage = value;
                Invalidate();
            }
        }

        [Category("Appearance")]
        public ImageLayout BackImageLayout
        {
            get => backImageLayout;
            set
            {
                backImageLayout = value;
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
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Bindable(true)]
        [Browsable(true)]
        public override string Text
        {
            get => _text;
            set
            {
                _text = value ?? "";
                Invalidate();
            }
        }

        protected override Size DefaultSize => new Size(150, 30);

        protected override void OnPaint(PaintEventArgs e)
        {
            var rectBorderSmooth = ClientRectangle;
            var rectBorder = Rectangle.Inflate(rectBorderSmooth, -1, -1);

            Color backColor = isMouseDown ? backColorMouseDown
                : isMouseOver ? backColorMouseOver : BackColor;

            using (Pen penBorder = new Pen(backColor, Height))
            {
                e.Graphics.DrawRectangle(penBorder, 0, 0, Width, Height);
            }

            if (BackImage != null)
            {
                switch (BackImageLayout)
                {
                    case ImageLayout.Stretch:
                        e.Graphics.DrawImage(BackImage, rectBorderSmooth);
                        break;

                    case ImageLayout.Center:
                        var centeredRect = new Rectangle(
                            (rectBorderSmooth.Width - BackImage.Width) / 2,
                            (rectBorderSmooth.Height - BackImage.Height) / 2,
                            BackImage.Width,
                            BackImage.Height);
                        e.Graphics.DrawImage(BackImage, centeredRect);
                        break;

                    case ImageLayout.Zoom:
                        var zoomedRect = GetZoomedRectangle(rectBorderSmooth, BackImage);
                        e.Graphics.DrawImage(BackImage, zoomedRect);
                        break;

                    case ImageLayout.Tile:
                        using (var textureBrush = new TextureBrush(BackImage))
                        {
                            e.Graphics.FillRectangle(textureBrush, rectBorderSmooth);
                        }
                        break;

                    default: // Default to ImageLayout.None
                        e.Graphics.DrawImage(BackImage, 0, 0, BackImage.Width, BackImage.Height);
                        break;
                }
            }

            if (BorderRadius <= 1)
            {
                using (Pen penBorder = new Pen(backColor, 1))
                {
                    Region = new Region(ClientRectangle);
                    penBorder.Alignment = PenAlignment.Inset;
                    e.Graphics.DrawRectangle(penBorder, 0, 0, Width - 0.5F, Height - 0.5F);
                }
            } else
            {
                using (GraphicsPath pathBorderSmooth = CreateRoundedRectanglePath(rectBorderSmooth, BorderRadius))
                using (GraphicsPath pathBorder = CreateRoundedRectanglePath(rectBorder, BorderRadius - 1))
                using (Pen penBorderSmooth = new Pen(Parent?.BackColor ?? Color.Transparent, 1))
                using (Pen penBorder = new Pen(backColor, 1))
                {
                    Region = new Region(pathBorderSmooth);
                    e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                    penBorder.Alignment = PenAlignment.Outset;

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

        private Rectangle GetZoomedRectangle(Rectangle containerRect, Image image)
        {
            var containerAspect = (float)containerRect.Width / containerRect.Height;
            var imageAspect = (float)image.Width / image.Height;

            if (containerAspect > imageAspect)
            {
                // Match height, adjust width
                var scaledWidth = (int)(imageAspect * containerRect.Height);
                var x = (containerRect.Width - scaledWidth) / 2;
                return new Rectangle(x, containerRect.Y, scaledWidth, containerRect.Height);
            }
            else
            {
                // Match width, adjust height
                var scaledHeight = (int)(containerRect.Width / imageAspect);
                var y = (containerRect.Height - scaledHeight) / 2;
                return new Rectangle(containerRect.X, y, containerRect.Width, scaledHeight);
            }
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
