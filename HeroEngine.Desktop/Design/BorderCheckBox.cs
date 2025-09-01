using System.ComponentModel;
using System.Drawing.Drawing2D;

namespace HeroEngine.Desktop.Design
{
    [DefaultEvent("_CheckedChanged")]
    public partial class BorderCheckBox : UserControl
    {
        private Color backcolorChecked = Color.Black;
        private Color checkMarkColor = Color.White;
        private int checkMarkSize = 2;
        private int borderRadius = 4;
        private bool isChecked = false;
        private string _text = string.Empty;

        public event EventHandler CheckedChanged;

        public BorderCheckBox()
        {
            MouseClick += CheckBox_MouseClick!;
            MouseDoubleClick += CheckBox_MouseClick!;
        }

        public override Color ForeColor { get => base.ForeColor; set { base.ForeColor = value; Invalidate(); } }

        [Category("Appearance")]
        public Color BackColorChecked
        {
            get => backcolorChecked;
            set
            {
                backcolorChecked = value;
                Invalidate();
            }
        }

        [Category("Appearance")]
        public Color CheckMarkColor
        {
            get => checkMarkColor;
            set
            {
                checkMarkColor = value;
                Invalidate();
            }
        }

        [Category("Appearance")]
        public int CheckMarkSize
        {
            get => checkMarkSize;
            set
            {
                checkMarkSize = Math.Max(0, value);
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
        public bool Checked
        {
            get => isChecked;
            set
            {
                isChecked = value;
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
                _text = value;
                Invalidate();
            }
        }

        protected override Size DefaultSize => new Size(150, 18);

        protected override void OnPaint(PaintEventArgs e)
        {
            var rectBorderSmooth = Rectangle.FromLTRB(0, 0, Height, Height);
            var rectBorder = Rectangle.Inflate(rectBorderSmooth, -2, -2);

            //override back color bs
            using (var brush = new SolidBrush(Parent?.BackColor ?? Color.Transparent))
            {
                e.Graphics.FillRectangle(brush, Rectangle.FromLTRB(0, 0, Width, Height));
            }

            Color backColor = Checked ? BackColorChecked : BackColor;
            if (BorderRadius <= 1)
            {
                using (var brush = new SolidBrush(backColor))
                {
                    e.Graphics.FillRectangle(brush, rectBorder);
                }
            }
            else
            {
                using (GraphicsPath pathBorderSmooth = CreateRoundedRectanglePath(rectBorderSmooth, BorderRadius))
                using (GraphicsPath pathBorder = CreateRoundedRectanglePath(rectBorder, BorderRadius - 1))
                using (Pen penBorderSmooth = new Pen(Parent?.BackColor ?? Color.Transparent, 1))
                using (var penBorder = new Pen(backColor, 1))
                {
                    e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                    //penBorder.Alignment = PenAlignment.Outset;

                    //e.Graphics.DrawPath(penBorderSmooth, pathBorderSmooth);
                    e.Graphics.DrawPath(penBorder, pathBorder);
                }
            }

            if (Checked && CheckMarkSize >= 1)
            {
                using (Pen penCheck = new Pen(CheckMarkColor, CheckMarkSize))
                {
                    int margin = 5;
                    Point[] checkPoints = {
                        new Point(rectBorderSmooth.Left + margin, rectBorderSmooth.Top + rectBorderSmooth.Height / 2),
                        new Point(rectBorderSmooth.Left + rectBorderSmooth.Width / 3, rectBorderSmooth.Bottom - margin),
                        new Point(rectBorderSmooth.Right - margin, rectBorderSmooth.Top + margin)
                    };

                    e.Graphics.DrawLines(penCheck, checkPoints);
                }
            }

            if (!string.IsNullOrEmpty(Text))
            {
                using (Brush textBrush = new SolidBrush(ForeColor))
                {
                    StringFormat stringFormat = new StringFormat
                    {
                        Alignment = StringAlignment.Near,
                        LineAlignment = StringAlignment.Center
                    };

                    int textPadding = Height + 4;
                    Rectangle textRect = new Rectangle(textPadding, 0, Width - textPadding, Height);

                    e.Graphics.DrawString(Text, Font, textBrush, textRect, stringFormat);
                }
            }
        }









        private void CheckBox_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Checked = !Checked; // Toggle the Checked property
            }
        }

        protected virtual void OnCheckedChanged(EventArgs e)
        {
            CheckedChanged?.Invoke(this, e); // Invoke the CheckedChanged event
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
