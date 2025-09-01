using System.Drawing.Imaging;

namespace HeroEngine.Desktop.Design
{
    public class ColorPictureBox : UserControl
    {
        private Image _backgroundImage;
        private ContentAlignment _imageAlignment = ContentAlignment.MiddleCenter;
        private PictureBoxSizeMode _sizeMode = PictureBoxSizeMode.Zoom;
        private Color _pictureTint = Color.White;

        [System.ComponentModel.Browsable(false)]
        public override Image BackgroundImage { get; set; }

        [System.ComponentModel.Browsable(false)]
        public override ImageLayout BackgroundImageLayout { get; set; }

        [System.ComponentModel.Category("Appearance")]
        public Image BackgroundImageCustom
        {
            get => _backgroundImage;
            set { _backgroundImage = value; Invalidate(); }
        }

        [System.ComponentModel.Category("Appearance")]
        public ContentAlignment ImageAlignment
        {
            get => _imageAlignment;
            set { _imageAlignment = value; Invalidate(); }
        }

        [System.ComponentModel.Category("Appearance")]
        public PictureBoxSizeMode SizeMode
        {
            get => _sizeMode;
            set { _sizeMode = value; Invalidate(); }
        }

        [System.ComponentModel.Category("Appearance")]
        public Color PictureTint
        {
            get => _pictureTint;
            set { _pictureTint = value; Invalidate(); }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            if (_backgroundImage != null)
            {
                Rectangle drawRect = GetImageRectangle();
                e.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                e.Graphics.DrawImage(ApplyTint(_backgroundImage, _pictureTint), drawRect);
            }
        }

        private Rectangle GetImageRectangle()
        {
            if (_backgroundImage == null) return ClientRectangle;

            switch (_sizeMode)
            {
                case PictureBoxSizeMode.StretchImage:
                    return ClientRectangle;
                case PictureBoxSizeMode.Zoom:
                    return GetZoomedRectangle();
                case PictureBoxSizeMode.CenterImage:
                    return new Rectangle((Width - _backgroundImage.Width) / 2, (Height - _backgroundImage.Height) / 2, _backgroundImage.Width, _backgroundImage.Height);
                case PictureBoxSizeMode.Normal:
                    return new Rectangle(0, 0, _backgroundImage.Width, _backgroundImage.Height);
                case PictureBoxSizeMode.AutoSize:
                    this.Size = _backgroundImage.Size;
                    return new Rectangle(0, 0, _backgroundImage.Width, _backgroundImage.Height);
                default:
                    return ClientRectangle;
            }
        }

        private Rectangle GetZoomedRectangle()
        {
            float ratio = Math.Min((float)Width / _backgroundImage.Width, (float)Height / _backgroundImage.Height);
            int newWidth = (int)(_backgroundImage.Width * ratio);
            int newHeight = (int)(_backgroundImage.Height * ratio);
            int x = (Width - newWidth) / 2;
            int y = (Height - newHeight) / 2;
            return new Rectangle(x, y, newWidth, newHeight);
        }

        private Image ApplyTint(Image source, Color tint)
        {
            Bitmap result = new Bitmap(source.Width, source.Height);

            using (Graphics g = Graphics.FromImage(result))
            {
                using (ImageAttributes attributes = new ImageAttributes())
                {
                    ColorMatrix colorMatrix = new ColorMatrix(
                        new float[][]
                        {
                        new float[] {tint.R / 255f, 0, 0, 0, 0},
                        new float[] {0, tint.G / 255f, 0, 0, 0},
                        new float[] {0, 0, tint.B / 255f, 0, 0},
                        new float[] {0, 0, 0, 1, 0},
                        new float[] {0, 0, 0, 0, 1}
                        });
                    attributes.SetColorMatrix(colorMatrix);
                    g.DrawImage(source, new Rectangle(0, 0, result.Width, result.Height), 0, 0, source.Width, source.Height, GraphicsUnit.Pixel, attributes);
                }
            }

            return result;
        }
    }
}
