namespace HeroEngine.Desktop.Interface
{
    public class Language
    {
        public static Language Polish = new Language()
        {
            LicenseKey = "Klucz licencji",
            Username = "Użytkownik",
            Password = "Hasło",
            RememberMe = "Zapamietaj mnie",
            DontHaveAccount = "Nie mam konta",
            Login = "Zaloguj",
            Register = "Rejestruj",
            DarkMode = "Ciemny motyw",
            LightMode = "Jasny motyw",
            SaveLogs = "Zapisuj logi do pliku",

            LicenseIncorrectCreds = "Zle dane logowania",
            LicenseExpires = "Wygasa: {0}",
            LicenseNeverExpries = "Licencja nigdy nie wygasa",
        };

        public static Language English = new Language()
        {
            LicenseKey = "License key",
            Username = "Username",
            Password = "Password",
            RememberMe = "Remember me",
            DontHaveAccount = "Don't have an account",
            Login = "Login",
            Register = "Register",
            DarkMode = "Dark theme",
            LightMode = "Light theme",
            SaveLogs = "Save logs to file",

            LicenseIncorrectCreds = "You have provided invalid login",
            LicenseExpires = "Expires: {0}",
            LicenseNeverExpries = "License never expries",
        };

        public string LicenseKey = "";
        public string Username = "";
        public string Password = "";
        public string RememberMe = "";
        public string DontHaveAccount = "";
        public string Login = "";
        public string Register = "";
        public string DarkMode = "";
        public string LightMode = "";
        public string SaveLogs = "";

        
        public string LicenseIncorrectCreds = "";
        public string LicenseExpires = "";
        public string LicenseNeverExpries = "";

        //license responses
    }
}
