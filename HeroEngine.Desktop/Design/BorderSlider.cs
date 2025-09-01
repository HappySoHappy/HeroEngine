using System.ComponentModel;
using System.Drawing.Drawing2D;

namespace HeroEngine.Desktop.Design
{
    [DefaultEvent("_CheckedChanged")]
    public partial class BorderSlider : UserControl
    {
        private Color borderkColor = Color.Black;
        private Color borderkColorChecked = Color.Black;
        private Color knobColor = Color.White;
        private Color knobColorChecked = Color.White;

        private int _knobSize = 2;
        private int _borderRadius = 10;
        private bool _checked = false;
        private string _text = string.Empty;
        private string _textChecked = string.Empty;

        private double _progress = -1;
        private System.Windows.Forms.Timer _animationTimer;

        public event EventHandler _CheckedChanged;

        public BorderSlider()
        {
            MouseClick += ToggleButton_MouseClick!;
            MouseDoubleClick += ToggleButton_MouseClick!;

            _animationTimer = new System.Windows.Forms.Timer();
            _animationTimer.Interval = 10;
            _animationTimer.Tick += AnimationTimer_Tick!;
        }

        [Category("Appearance")]
        public Color BorderColor
        {
            get => borderkColor;
            set
            {
                borderkColor = value;
                Invalidate();
            }
        }

        [Category("Appearance")]
        public Color BorderColorChecked
        {
            get => borderkColorChecked;
            set
            {
                borderkColorChecked = value;
                Invalidate();
            }
        }

        [Category("Appearance")]
        public Color KnobColor
        {
            get => knobColor;
            set
            {
                knobColor = value;
                Invalidate();
            }
        }

        [Category("Appearance")]
        public Color KnobColorChecked
        {
            get => knobColorChecked;
            set
            {
                knobColorChecked = value;
                Invalidate();
            }
        }

        [Category("Appearance")]
        public int KnobSize
        {
            get => _knobSize;
            set
            {
                _knobSize = value;
                Invalidate();
            }
        }

        [Category("Appearance")]
        public int BorderRadius
        {
            get => _borderRadius;
            set
            {
                _borderRadius = value;
                Invalidate();
            }
        }

        [Category("Appearance")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Bindable(true)]
        [Browsable(true)]
        public bool Checked
        {
            get => _checked;
            set
            {
                if (_progress == -1)
                {
                    _progress = value ? 1 : 0;
                }

                if (_checked == value) return;

                _checked = value;
                Invalidate();

                if (_checked && _progress < 1.0 || !_checked && _progress > 0.0)
                {
                    _animationTimer.Start();
                }
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

        [Category("Appearance")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Bindable(true)]
        [Browsable(true)]
        public string TextChecked
        {
            get => _textChecked;
            set
            {
                _textChecked = value;
                Invalidate();
            }
        }

        protected override Size DefaultSize => new Size(150, 23);

        protected override void OnPaint(PaintEventArgs e)
        {
            var rectBorderSmooth = ClientRectangle;
            var rectBorder = Rectangle.Inflate(rectBorderSmooth, -1, -1);

            Color backColor = Checked ? BorderColorChecked : BorderColor;

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

            Color knobColor = Checked ? KnobColorChecked : KnobColor;
            using (Brush brush = new SolidBrush(knobColor))
            {
                int knobLocationY = (Height - KnobSize) / 2;
                int knobLocationX = (int)(knobLocationY + (Width - KnobSize - knobLocationY - knobLocationY) * _progress);

                Rectangle knobRectangle = new Rectangle(knobLocationX, knobLocationY, KnobSize, KnobSize);
                e.Graphics.FillEllipse(brush, knobRectangle);
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

                    string text = Checked ? TextChecked : Text;

                    int textPadding = Height;
                    Rectangle textRect = new Rectangle(textPadding, 0, Width - textPadding, Height);
                    textRect.X = (int)(Width - e.Graphics.MeasureString(text, Font).Width) / 2;

                    e.Graphics.DrawString(text, Font, textBrush, textRect, stringFormat);
                }
            }
        }

        private void ToggleButton_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Checked = !Checked;
                _CheckedChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        private void AnimationTimer_Tick(object sender, EventArgs e)
        {
            if (_progress == -1)
            {
                _progress = Checked ? 1 : 0;
            }

            double stepSize = 0.1;

            if (Checked)
            {
                _progress = Math.Min(_progress + stepSize, 1.0);
            }
            else
            {
                _progress = Math.Max(_progress - stepSize, 0.0);
            }

            Invalidate();

            if ((Checked && _progress >= 1.0) || (!Checked && _progress <= 0.0))
            {
                _animationTimer.Stop();
            }
        }

        protected virtual void OnCheckedChanged(EventArgs e)
        {
            _CheckedChanged?.Invoke(this, e);
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
    }
}
