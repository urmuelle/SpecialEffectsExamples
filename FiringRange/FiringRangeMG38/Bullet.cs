// <copyright file="Bullet.cs" company="Urs Müller">
// </copyright>

namespace FiringRangeMG38
{
    using Microsoft.Xna.Framework;

    public class Bullet
    {
        private Vector3 position;
        private Vector3 velocity;

        public Bullet()
        {
            position = new Vector3(0, 0, 0);
            velocity = new Vector3(0, 0, 0);
        }

        public Vector3 Position
        {
            get { return this.position; }
            set { this.position = value; }
        }

        public Vector3 Velocity
        {
            get { return this.velocity; }
            set { this.velocity = value; }
        }
    }
}
