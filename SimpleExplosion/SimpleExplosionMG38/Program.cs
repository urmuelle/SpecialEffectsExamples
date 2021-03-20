// <copyright file="Program.cs" company="Urs Müller">
// </copyright>

namespace SimpleExplosionMG38
{
    using System;

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
            using (var game = new SimpleExplosion())
            {
                game.Run();
            }
        }
    }
}
