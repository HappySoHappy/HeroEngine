using System.ComponentModel;
using System.Drawing.Drawing2D;

namespace HeroEngine.Desktop.Design
{
    public partial class GradientLabel : Label
    {
        private Color startColor = Color.Black;
        private Color endColor = Color.Black;

        public GradientLabel()
        {
            //SetStyle(ControlStyles.Opaque, false);
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint |
              ControlStyles.UserPaint |
              ControlStyles.OptimizedDoubleBuffer, true);
            BackColor = Color.Transparent;
        }

        [Category("Appearance")]
        public Color StartColor
        {
            get { return startColor; }
            set
            {
                startColor = value;
                Invalidate();
            }
        }

        [Category("Appearance")]
        public Color EndColor
        {
            get { return endColor; }
            set
            {
                endColor = value;
                Invalidate();
            }
        }

        protected override void OnPaintBackground(PaintEventArgs pevent)
        {
            if (Parent == null) return;

            using (var bmp = new Bitmap(Width, Height))
            {
                DrawParentBackground(bmp);
                pevent.Graphics.DrawImage(bmp, 0, 0);
            }
        }

        private void DrawParentBackground(Bitmap bmp)
        {
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.TranslateTransform(-Left, -Top);
                var clip = new Rectangle(Left, Top, Width, Height);
                bool needsClear = true;

                foreach (Control c in Parent.Controls)
                {
                    if (c == this || !c.Bounds.IntersectsWith(clip)) continue;

                    needsClear = false;
                    using (var tempBmp = new Bitmap(c.Width, c.Height))
                    {
                        c.DrawToBitmap(tempBmp, new Rectangle(0, 0, c.Width, c.Height));
                        g.DrawImage(tempBmp, c.Left, c.Top);
                    }
                }

                if (needsClear && BackColor == Color.Transparent)
                {
                    g.Clear(Parent.BackColor);
                }
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            using (StringFormat stringFormat = new StringFormat())
            {
                stringFormat.Alignment = StringAlignment.Center;
                stringFormat.LineAlignment = StringAlignment.Center;

                SizeF textSize = e.Graphics.MeasureString(Text, Font);

                float x = (ClientRectangle.Width - textSize.Width) / 2;
                float y = (ClientRectangle.Height - textSize.Height) / 2;

                RectangleF textRect = new RectangleF(x, y, textSize.Width, textSize.Height);

                e.Graphics.DrawString(Text, Font, CreateBrush(), textRect, stringFormat);
            }
        }

        private LinearGradientBrush CreateBrush()
        {
            LinearGradientBrush brush = new LinearGradientBrush(ClientRectangle, Color.Black, Color.Black, 0f);
            brush.InterpolationColors = new ColorBlend()
            {
                Colors = new[] { StartColor, EndColor },
                Positions = new[] { 0.0f, 1.0f },
            };

            return brush;
        }

        #region hide properties
        [Browsable(false)]
        public new Color ForeColor
        {
            get => base.ForeColor;
            set => base.ForeColor = value;
        }

        [Browsable(false)]
        public new FlatStyle FlatStyle
        {
            get => base.FlatStyle;
            set => base.FlatStyle = value;
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
