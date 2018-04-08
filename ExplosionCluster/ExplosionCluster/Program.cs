// <copyright file="Program.cs" company="Urs Müller">
// </copyright>

namespace ExplosionCluster
{
    using System;

#if WINDOWS || LINUX
    /// <summary>
    /// The main class.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        public static void Main()
        {
            using (var game = new ExplosionCluster())
            {
                game.Run();
            }
        }
    }
#endif
}
