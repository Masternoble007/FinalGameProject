using System;

namespace FinalGameProject
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using (var game = new FinalGameProject())
                game.Run();
        }
    }
}
