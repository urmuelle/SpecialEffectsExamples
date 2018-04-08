// <copyright file="PlasmaGun.cs" company="Urs Müller">
// </copyright>

namespace FiringRange
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Content;
    using Microsoft.Xna.Framework.Graphics;
    using SpecialEffectsExamplesLibrary.Camera;

    public class PlasmaGun : Gun
    {
        private PlasmaBulletArray bulletArray;
        private SpecialEffectsExamplesLibrary.Helper helper;

        public PlasmaGun()
        {
            helper = new SpecialEffectsExamplesLibrary.Helper();
        }

        public PlasmaBulletArray BulletArray
        {
            get { return bulletArray; }
            set { bulletArray = value; }
        }

        public override void LoadContent(ContentManager content, GraphicsDevice graphicsDevice, string meshFileName)
        {
            bulletArray = new PlasmaBulletArray();
            bulletArray.LoadContent(content, graphicsDevice);

            base.LoadContent(content, graphicsDevice, meshFileName);
        }

        public override bool CanFire()
        {
            return !Timer.IsRunning();
        }

        public override void Fire(Camera camera)
        {
            Matrix matWorld, matInvView;

            Vector3 position = new Vector3(0.0f, 0.0f, 0.0f);
            Vector3 velocity = new Vector3(0.0f, 0.0f, -1.0f);

            Vector3 vPositionOut, vVelocityOut;

            // invert view and remove translation
            matInvView = Matrix.Invert(camera.ViewMatrix);
            matInvView.Translation = Vector3.Zero;

            matWorld = AssembleWorldMatrix(camera, Vector3.Zero);

            vPositionOut = Vector3.Transform(position, matWorld);
            vVelocityOut = Vector3.Transform(velocity, matInvView);

            bulletArray.AddBullet(vPositionOut, vVelocityOut * 50.0f);

            Timer.Begin();
        }

        public override void Update(GameTime gameTime)
        {
            if ((Timer.Time / 1000.0f) > 0.1f)
            {
                Timer.Stop();
            }
        }

        public override void Draw(GraphicsDevice graphicsDevice, Camera camera, Matrix projectionMatrix, GameTime gameTime)
        {
            // render gun model
            Vector3 translation;

            float fOffsetZ = -1.0f + ((Timer.Time / 1000.0f) * 5.0f);

            if (fOffsetZ > 0.0f)
            {
                fOffsetZ = 0.0f;
            }

            if (Timer.IsRunning())
            {
                translation = new Vector3(0.0f, 0.0f, fOffsetZ);
            }
            else
            {
                translation = Vector3.Zero;
            }

            var worldMatrix = AssembleWorldMatrix(camera, translation);

            graphicsDevice.BlendState = BlendState.Opaque;
            graphicsDevice.DepthStencilState = DepthStencilState.Default;
            graphicsDevice.RasterizerState = RasterizerState.CullCounterClockwise;

            var center = Mesh.Meshes[0].BoundingSphere.Center;

            foreach (var mesh in Mesh.Meshes)
            {
                // "Effect" refers to a shader. Each mesh may
                // have multiple shaders applied to it for more
                // advanced visuals.
                foreach (BasicEffect effect in mesh.Effects)
                {
                    // Create a light
                    // Plasma gun looks metallic & white
                    effect.LightingEnabled = true;
                    effect.AmbientLightColor = new Vector3(0.0f, 0.1f, 0.0f);
                    effect.DirectionalLight0.DiffuseColor = new Vector3(0.3f, 0.0f, 0.0f);
                    effect.DirectionalLight0.Direction = new Vector3(1, -1, 1);
                    effect.DirectionalLight0.SpecularColor = new Vector3(0.1f, 1.0f, 0.1f);
                    effect.World = worldMatrix;
                    effect.Projection = projectionMatrix;
                    effect.View = camera.ViewMatrix;
                }

                mesh.Draw();
            }
        }
    }
}
