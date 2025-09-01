using System.ComponentModel;
using System.Drawing.Drawing2D;

namespace HeroEngine.Desktop.Design
{
    [DefaultEvent("_ValueChanged")]
    public class NumberSlider : UserControl
    {
        private Color _thumbColor = Color.White;
        private Color _thumbColorDragging = Color.White;
        private double _minimumValue = 0;
        private double _maximumValue = 10.0;
        private double _valueInterval = 0.5;
        private double _value;
        private int _borderRadius = 7;

        private string _valueFormat = "{0:0.##}";

        private bool _isDragging = false;
        private Point _dragStartPoint;

        public event EventHandler _ValueChanged;

        public NumberSlider()
        {
            _value = _minimumValue;
        }

        #region properties
        [Category("Appearance")]
        public Color ThumbColor
        {
            get => _thumbColor;
            set
            {
                _thumbColor = value;
                Invalidate();
            }
        }

        [Category("Appearance")]
        public Color ThumbColorDragging
        {
            get => _thumbColorDragging;
            set
            {
                _thumbColorDragging = value;
                Invalidate();
            }
        }

        [Category("Appearance")]
        public double MinimumValue
        {
            get => _minimumValue;
            set
            {
                if (value > _maximumValue)
                    _maximumValue = value;

                _minimumValue = value;
                Value = Math.Max(_minimumValue, _value);

                Invalidate();
            }
        }

        [Category("Appearance")]
        public double MaximumValue
        {
            get => _maximumValue;
            set
            {
                if (value < _minimumValue)
                    _minimumValue = value;

                _maximumValue = value;
                Value = Math.Min(_maximumValue, _value);

                Invalidate();
            }
        }

        [Category("Appearance")]
        public double ValueInterval
        {
            get => _valueInterval;
            set
            {
                _valueInterval = value;
                Invalidate();
            }
        }

        [Category("Appearance")]
        public double Value
        {
            get => _value;
            set
            {
                double clampedValue = Math.Max(_minimumValue, Math.Min(_maximumValue, value));

                if (_value != clampedValue)
                {
                    _value = clampedValue;
                    Invalidate();
                    _ValueChanged?.Invoke(this, EventArgs.Empty);
                }
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
        public string ValueFormat
        {
            get => _valueFormat;
            set
            {
                _valueFormat = value;
                Invalidate();
            }
        }

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override string Text
        {
            get => base.Text;
            set => base.Text = value;
        }
        #endregion

        protected override Size DefaultSize => new Size(150, 23);

        protected override void OnPaint(PaintEventArgs e)
        {
            var rectBorderSmooth = ClientRectangle;
            var rectBorder = Rectangle.Inflate(rectBorderSmooth, -1, -1);

            Color backColor = this.BackColor;

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

            Rectangle sliderTrackRect = new Rectangle(BorderRadius, Height / 2 - 4, Width - 2 * BorderRadius, 8);
            float sliderWidth = sliderTrackRect.Width;
            float sliderValuePosition = sliderWidth * (float)((_value - _minimumValue) / (_maximumValue - _minimumValue));

            Rectangle sliderThumbRect = new Rectangle(
                (int)(sliderTrackRect.Left + sliderValuePosition - 5),
                rectBorder.Top,
                10,
                rectBorder.Bottom - rectBorder.Top
            );

            Color thumbColor = _isDragging ? _thumbColorDragging : _thumbColor;
            using (Brush thumbBrush = new SolidBrush(thumbColor))
            {
                e.Graphics.FillRectangle(thumbBrush, sliderThumbRect);
            }

            string text = string.Format(_valueFormat, _value);
            if (!string.IsNullOrEmpty(text))
            {
                using (Brush textBrush = new SolidBrush(ForeColor))
                {
                    StringFormat stringFormat = new StringFormat
                    {
                        Alignment = StringAlignment.Near,
                        LineAlignment = StringAlignment.Center
                    };

                    int textPadding = Height;
                    Rectangle textRect = new Rectangle(textPadding, 0, Width - textPadding, Height);
                    textRect.X = (int)(Width - e.Graphics.MeasureString(text, Font).Width) / 2;

                    e.Graphics.DrawString(text, Font, textBrush, textRect, stringFormat);
                }
            }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (ClientRectangle.Contains(e.Location))
            {
                _isDragging = true;
                _dragStartPoint = e.Location;
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (_isDragging)
            {
                float mousePosition = e.X;
                float controlWidth = Width;

                mousePosition = Math.Min(Math.Max(mousePosition, 0), controlWidth);

                float valuePosition = mousePosition / controlWidth;

                double newValue = _minimumValue + valuePosition * (_maximumValue - _minimumValue);
                newValue = Math.Round(newValue / _valueInterval) * _valueInterval;
                if (_value != newValue)
                {
                    Value = newValue;
                }
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            _isDragging = false;
            Invalidate();
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
