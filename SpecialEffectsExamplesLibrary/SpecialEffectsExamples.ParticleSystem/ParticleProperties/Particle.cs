// <copyright file="Particle.cs" company="Urs Müller">
// </copyright>

namespace SpecialEffectsExamplesLibrary.ParticleSystem.ParticleProperties
{
    using Microsoft.Xna.Framework;

    public class Particle
    {
        private float weight;
        private float size;
        private float lifetime;
        private float age;

        private Color color;
        private Color colorStep;

        private Vector2[] corners;
        private Vector3[] vertexPositions;
        private Vector3 position;
        private Vector3 direction;

        private bool alive;

        public Particle()
        {
            weight = 1.0f;
            size = 1.0f;
            lifetime = 1.0f;
            age = 0.0f;
            corners = new Vector2[4];
            corners[0] = new Vector2(-1, -1);
            corners[1] = new Vector2(1, -1);
            corners[2] = new Vector2(1, 1);
            corners[3] = new Vector2(-1, 1);

            vertexPositions = new Vector3[4];
        }

        public Vector3 Position
        {
            get
            {
                return position;
            }

            set
            {
                position = value;
                CalculateCornerVertices();
            }
        }

        public Vector3 Direction
        {
            get
            {
                return direction;
            }

            set
            {
                direction = value;
            }
        }

        public Color Color
        {
            get { return color; }
            set { color = value; }
        }

        public Color ColorStep
        {
            get { return colorStep; }
            set { colorStep = value; }
        }

        public float Weight
        {
            get { return weight; }
            set { weight = value; }
        }

        public float Size
        {
            get { return size; }
            set { size = value; }
        }

        public float Lifetime
        {
            get { return lifetime; }
            set { lifetime = value; }
        }

        public float Age
        {
            get { return age; }
            set { age = value; }
        }

        public bool Alive
        {
            get { return alive; }
            set { alive = value; }
        }

        public Vector3[] VertexPositions
        {
            get { return vertexPositions; }
        }

        public Vector2[] Corners
        {
            get { return corners; }
        }

        public void ResetParticle()
        {
            weight = 1.0f;
            size = 1.0f;
            lifetime = 1.0f;
            age = 0.0f;
            corners[0].X = -1;
            corners[0].Y = -1;
            corners[1].X = 1;
            corners[1].Y = -1;
            corners[2].X = 1;
            corners[2].Y = 1;
            corners[3].X = -1;
            corners[3].Y = 1;
        }

        public void CalculateCornerVertices()
        {
            var halfWidth = size / 2;

            vertexPositions[0] = new Vector3(position.X + halfWidth, position.Y + halfWidth, position.Z);
            vertexPositions[1] = new Vector3(position.X + halfWidth, position.Y - halfWidth, position.Z);
            vertexPositions[2] = new Vector3(position.X - halfWidth, position.Y - halfWidth, position.Z);
            vertexPositions[3] = new Vector3(position.X - halfWidth, position.Y + halfWidth, position.Z);
        }

        public bool Update(GameTime gameTime)
        {
            // age the particle
            age += (float)gameTime.ElapsedGameTime.TotalSeconds;

            // if this particle's age is greater than it's lifetime, it dies.
            if (age >= lifetime)
            {
                return false;
            }

            // move particle
            position += direction * (float)gameTime.ElapsedGameTime.TotalSeconds;

            color = new Color(
                color.R += colorStep.R,
                color.G += colorStep.G,
                color.B += colorStep.B,
                color.A += colorStep.A);

            // Update corner vertices
            CalculateCornerVertices();

            return true;
        }
    }
}
