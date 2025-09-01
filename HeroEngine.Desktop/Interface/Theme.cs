namespace HeroEngine.Desktop.Interface
{
    public class Theme
    {
        public static Theme Light = new Theme()
        {
            Icons = IconSet.Light,
            IconColor = Color.FromArgb(66, 66, 66),
            HighlightColor = Color.FromArgb(235, 155, 0),
            HighlightBorderColor = Color.FromArgb(225, 145, 0),
            TextColor = Color.Black,
            TextPlaceholderColor = Color.Gray,
            PrimaryColor = Color.FromArgb(245, 245, 245),
            SecondaryColor = Color.FromArgb(230, 230, 230),
            SecondaryBorderColor = Color.FromArgb(210, 210, 210),

            TertiaryColor = Color.FromArgb(240, 240, 240),
            TertiaryBorderColor = Color.FromArgb(200, 200, 200),
        };

        public static Theme Dark = new Theme()
        {
            Icons = IconSet.Dark,
            IconColor = Color.FromArgb(197, 197, 197),
            HighlightColor = Color.FromArgb(235, 155, 0),
            HighlightBorderColor = Color.FromArgb(225, 145, 0),
            TextColor = Color.White,
            TextPlaceholderColor = Color.DarkGray,
            PrimaryColor = Color.FromArgb(16, 17, 18),
            SecondaryColor = Color.FromArgb(27, 28, 30),
            SecondaryBorderColor = Color.FromArgb(47, 48, 50),
            TertiaryColor = Color.FromArgb(40, 42, 46),
            TertiaryBorderColor = Color.FromArgb(64, 64, 64),
        };

        public IconSet Icons;
        public Color IconColor;
        public Color HighlightColor;
        public Color HighlightBorderColor;
        public Color TextColor;
        public Color TextPlaceholderColor;
        public Color PrimaryColor;
        public Color PrimaryBorderColor;

        public Color SecondaryColor;
        public Color SecondaryBorderColor;

        public Color TertiaryColor;
        public Color TertiaryBorderColor;

        public enum IconSet
        {
            Light,
            Dark,
        }

        public static IEnumerable<Theme> Themes()
        {
            yield return Light;
            yield return Dark;
        }
    }
}
