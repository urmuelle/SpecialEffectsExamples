// <copyright file="Program.cs" company="Urs Müller">
// Copyright (c) Urs Müller. All rights reserved.
// </copyright>

namespace SimpleParticleSystem
{
    public static class Program
    {
        public static void Main()
        {
            using (var game = new Game1())
                game.Run();
        }
    }
}
