using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Drawing2D;

namespace HeroEngine.Desktop.Design
{
    [DefaultEvent("IndexChanged")]
    public class DropDownMenu : Control
    {
        private List<string> _elements = new List<string>();

        private Color _color = Color.White;
        private Color _colorMouseOver = Color.Gray;
        private Color _colorMouseDown = Color.Black;

        private Color _borderColor = Color.White;
        private Color _borderColorMouseOver = Color.Gray;
        private Color _borderColorMouseDown = Color.Black;

        private Color _selectedBorderColor = Color.White;
        private Color _selectedBorderColorMouseOver = Color.Gray;
        private Color _selectedBorderColorMouseDown = Color.Black;

        private int _headerHeight = 25;
        private int _borderRadius = 7;
        private int _borderWidth = 1;

        private int _markSize = 2;

        private bool _expanded = false;
        private int _index = 0;

        public event EventHandler IndexChanged;

        public DropDownMenu()
        {
            MinimumSize = new Size(0, _headerHeight);
        }

        #region Properties
        [Category("Appearance")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [Editor(@"System.Windows.Forms.Design.StringCollectionEditor", typeof(UITypeEditor))]
        public List<string> Elements
        {
            get => _elements;
            set
            {
                _elements = value ?? new List<string>();
                UpdateBorderButtons();
                Invalidate();
            }
        }

        [Category("Appearance")]
        public Color Color
        {
            get => _color;
            set
            {
                _color = value;
                UpdateBorderButtons();
                Invalidate();
            }
        }

        [Category("Appearance")]
        public Color ColorMouseOver
        {
            get => _colorMouseOver;
            set
            {
                _colorMouseOver = value;
                UpdateBorderButtons();
                Invalidate();
            }
        }

        [Category("Appearance")]
        public Color ColorMouseDown
        {
            get => _colorMouseDown;
            set
            {
                _colorMouseDown = value;
                UpdateBorderButtons();
                Invalidate();
            }
        }

        [Category("Appearance")]
        public Color BorderColor
        {
            get => _borderColor;
            set
            {
                _borderColor = value;
                UpdateBorderButtons();
                Invalidate();
            }
        }

        [Category("Appearance")]
        public Color BorderColorMouseOver
        {
            get => _borderColorMouseOver;
            set
            {
                _borderColorMouseOver = value;
                UpdateBorderButtons();
                Invalidate();
            }
        }

        [Category("Appearance")]
        public Color BorderColorMouseDown
        {
            get => _borderColorMouseDown;
            set
            {
                _borderColorMouseDown = value;
                UpdateBorderButtons();
                Invalidate();
            }
        }

        [Category("Appearance")]
        public Color SelectedBorderColor
        {
            get => _selectedBorderColor;
            set
            {
                _selectedBorderColor = value;
                UpdateBorderButtons();
                Invalidate();
            }
        }

        [Category("Appearance")]
        public Color SelectBorderColorMouseOver
        {
            get => _selectedBorderColorMouseOver;
            set
            {
                _selectedBorderColorMouseOver = value;
                UpdateBorderButtons();
                Invalidate();
            }
        }

        [Category("Appearance")]
        public Color SelectBorderColorMouseDown
        {
            get => _selectedBorderColorMouseDown;
            set
            {
                _selectedBorderColorMouseDown = value;
                UpdateBorderButtons();
                Invalidate();
            }
        }

        [Category("Appearance")]
        public int HeaderHeight
        {
            get => _headerHeight;
            set
            {
                _headerHeight = Math.Max(1, value);
                Invalidate();
            }
        }

        [Category("Appearance")]
        public int BorderRadius
        {
            get => _borderRadius;
            set
            {
                _borderRadius = Math.Max(0, value);
                Invalidate();
            }
        }

        [Category("Appearance")]
        public int BorderWidth
        {
            get => _borderWidth;
            set
            {
                _borderWidth = Math.Max(0, value);
                Invalidate();
            }
        }

        [Category("Appearance")]
        public int MarkSize
        {
            get => _markSize;
            set
            {
                _markSize = Math.Max(0, value);
                Invalidate();
            }
        }

        [Category("Appearance")]
        public bool Expanded
        {
            get => _expanded;
            set
            {
                _expanded = value;
                UpdateBorderButtons();
                Invalidate();
            }
        }

        [Category("Appearance")]
        public int SelectionIndex
        {
            get => _index;
            set
            {
                int oldIndex = _index;
                _index = Math.Min(Math.Max(0, value), _elements.Count - 1);
                if (oldIndex != _index)
                {
                    IndexChanged?.Invoke(this, EventArgs.Empty);
                }
                Invalidate();
            }
        }

        [Browsable(false)]
        public override string Text
        {
            get => base.Text;
            set => base.Text = value;
        }

        protected override Size DefaultSize => new Size(150, 25);

        public override Size MinimumSize
        {
            get => base.MinimumSize;
            set
            {
                base.MinimumSize = new Size(value.Width, Math.Max(value.Height, HeaderHeight));
            }
        }
        #endregion

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);

            if (Parent != null)
            {
                Parent.MouseClick += Parent_MouseClick!;
            }

            Application.AddMessageFilter(new MessageFilter(this));
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var rectBorderSmooth = Rectangle.FromLTRB(0, 0, Height, Height);
            var rectBorder = Rectangle.Inflate(rectBorderSmooth, -2, -2);

            using (var brush = new SolidBrush(Color))
            {
                e.Graphics.FillRectangle(brush, Rectangle.FromLTRB(0, 0, Width, HeaderHeight));
            }

            if (SelectionIndex >= 0 && SelectionIndex < Elements.Count)
            {
                string element = Elements.ElementAt(SelectionIndex);
                if (!string.IsNullOrEmpty(element))
                {
                    using (Brush textBrush = new SolidBrush(ForeColor))
                    {
                        StringFormat stringFormat = new StringFormat
                        {
                            Alignment = StringAlignment.Near,
                            LineAlignment = StringAlignment.Center
                        };


                        int textPadding = 4;
                        Rectangle textRect = new Rectangle(textPadding, 0, Width - textPadding * 2, HeaderHeight);

                        e.Graphics.DrawString(element, Font, textBrush, textRect, stringFormat);
                    }
                }
            }

            Rectangle squareRect = new Rectangle(Width - HeaderHeight, 0, HeaderHeight, HeaderHeight);
            using (Brush brush = new SolidBrush(Color))
            {
                e.Graphics.FillRectangle(brush, squareRect);
            }

            using (Brush arrowBrush = new SolidBrush(ForeColor))
            {
                Point[] arrowPoints =
                {
                    new Point(squareRect.Left + squareRect.Width / 3, squareRect.Top + squareRect.Height / 3),
                    new Point(squareRect.Right - squareRect.Width / 3, squareRect.Top + squareRect.Height / 3),
                    new Point(squareRect.Left + squareRect.Width / 2, squareRect.Bottom - squareRect.Height / 3)
                };

                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                e.Graphics.FillPolygon(arrowBrush, arrowPoints);
            }
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            UpdateBorderButtons();
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            if (e.Y < HeaderHeight)
            {
                Expanded = !Expanded;
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (Parent != null)
                {
                    Parent.MouseClick -= Parent_MouseClick!;
                }

                Application.RemoveMessageFilter(new MessageFilter(this));
            }

            base.Dispose(disposing);
        }

        private void Parent_MouseClick(object sender, MouseEventArgs e)
        {
            if (!ClientRectangle.Contains(PointToClient(Cursor.Position)))
            {
                Expanded = false;
                UpdateBorderButtons();
                Invalidate();
            }
        }

        private void UpdateBorderButtons()
        {
            Controls.Clear();

            int yOffset = HeaderHeight;

            if (Expanded)
            {
                for (int i = 0; i < Elements.Count; i++)
                {
                    string element = Elements[i];

                    BorderButton button = new BorderButton
                    {
                        BackColor = Color,
                        BackColorMouseDown = ColorMouseDown,
                        BackColorMouseOver = ColorMouseOver,

                        BorderColor = i == SelectionIndex ? SelectedBorderColor : BorderColor,
                        BorderColorMouseDown = i == SelectionIndex ? SelectBorderColorMouseDown : BorderColorMouseDown,
                        BorderColorMouseOver = i == SelectionIndex ? SelectBorderColorMouseOver : BorderColorMouseOver,

                        ForeColor = ForeColor,
                        Text = element,
                        Width = Width,
                        Height = HeaderHeight,
                        Top = yOffset,
                        Left = 0,
                        Tag = i
                    };

                    button.Click += (sender, args) =>
                    {
                        if (sender is BorderButton clickedButton && clickedButton.Tag is int index)
                        {
                            SelectionIndex = index;
                            Expanded = false;
                        }
                    };

                    Controls.Add(button);

                    yOffset += button.Height;
                }
            }

            BringToFront();
            Height = yOffset;
        }

        private class MessageFilter : IMessageFilter
        {
            private readonly DropDownMenu _dropDownMenu;

            public MessageFilter(DropDownMenu dropDownMenu)
            {
                _dropDownMenu = dropDownMenu;
            }

            public bool PreFilterMessage(ref Message m)
            {
                // Handle mouse clicks (WM_LBUTTONDOWN) outside the dropdown
                if (m.Msg == 0x201) // WM_LBUTTONDOWN message
                {
                    Point mousePos = Control.MousePosition;
                    if (!_dropDownMenu.ClientRectangle.Contains(_dropDownMenu.PointToClient(mousePos)))
                    {
                        _dropDownMenu.Expanded = false;
                    }
                }
                return false;
            }
        }
    }
}
