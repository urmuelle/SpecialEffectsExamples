﻿using System;

namespace ExplosionClusterMG38
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using (var game = new ExplosionCluster())
                game.Run();
        }
    }
}
