using HeroEngine.Desktop.Interface;
using Newtonsoft.Json;

namespace HeroEngine.Desktop.Persistance
{
    public class ProgramSettings
    {
        public string User = "";
        public string Password  = "";
        public bool Remember = false;

        public bool FileLogs = true;

        public int ThemeIndex = 0;
        public Color Highlight = Color.FromArgb(235, 155, 0);

        [JsonIgnore]
        public Theme Theme = Theme.Dark;

        [JsonIgnore]
        public Language Language = Language.Polish;

    }
}
