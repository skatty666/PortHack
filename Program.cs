using System;

namespace PortHackDebug
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using (var game = new GameDebug())
                game.Run();
        }
    }
}