using HeroEngine.HeroZero.Modules;

namespace HeroEngine.HeroZero
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            //Application.Run(new Form1());

            RequestListGenerator.Run();

            Console.ReadLine();
        }
    }
}