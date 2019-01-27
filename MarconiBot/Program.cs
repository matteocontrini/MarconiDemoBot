using System;

namespace MarconiBot
{
    static class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Avvio applicatione...");

            IApplication application = new Application();
            application.Start();

            Console.WriteLine("Applicazione avviata");
            Console.ReadLine();
        }
    }
}
