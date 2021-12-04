using System;

namespace FiringRangeMG38
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using (var game = new FiringRange())
                game.Run();
        }
    }
}
