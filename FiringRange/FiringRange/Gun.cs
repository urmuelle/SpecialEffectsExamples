// <copyright file="Gun.cs" company="Urs Müller">
// </copyright>

namespace FiringRange
{
    using System;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Content;
    using Microsoft.Xna.Framework.Graphics;
    using SpecialEffectsExamplesLibrary;
    using SpecialEffectsExamplesLibrary.Camera;

    public abstract class Gun
    {
        private Timer timer;
        private Model mesh;
        private Vector3 position;

        public Gun()
        {
        }

        public Model Mesh
        {
            get { return mesh; }
            set { mesh = value; }
        }

        public Vector3 Position
        {
            get { return position; }
            set { position = value; }
        }

        public Timer Timer
        {
            get { return timer; }
            set { timer = value; }
        }

        public virtual void LoadContent(ContentManager content, GraphicsDevice graphicsDevice, string meshFileName)
        {
            Mesh = content.Load<Model>(meshFileName);
            timer = new Timer();
        }

        public abstract bool CanFire();

        public abstract void Fire(Camera camera);

        public abstract void Update(GameTime gameTime);

        public abstract void Draw(GraphicsDevice graphicsDevice, Camera camera, Matrix projectionMatrix, GameTime gameTime = null);

        protected Matrix AssembleWorldMatrix(Camera camera, Vector3 translation)
        {
            Matrix matView = camera.ViewMatrix;

            var matViewInverse = Matrix.Invert(matView);
            var matTrans = Matrix.CreateTranslation(
                position.X + translation.X,
                position.Y + translation.Y,
                position.Z + translation.Z);

            var matScale = Matrix.CreateScale(0.15f, 0.15f, 0.15f);
            var matRot = Matrix.CreateFromYawPitchRoll(
                -((float)Math.PI - ((float)Math.PI / 15.0f)),
                -(float)Math.PI / 16.0f,
                0.0f);

            return matScale * matRot * matTrans * matViewInverse;
        }
    }
}
