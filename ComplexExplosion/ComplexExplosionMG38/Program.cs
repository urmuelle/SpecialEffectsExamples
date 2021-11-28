using System;

namespace ComplexExplosionWindows
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using (var game = new ComplexExplosion())
                game.Run();
        }
    }
}
