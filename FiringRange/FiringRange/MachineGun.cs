// <copyright file="MachineGun.cs" company="Urs Müller">
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
    using SpecialEffectsExamplesLibrary.Animation;
    using SpecialEffectsExamplesLibrary.Camera;

    public class MachineGun : Gun
    {
        private AnimationSequence animation;
        private Sprite muzzleFlare;
        private SpecialEffectsExamplesLibrary.Helper helper;

        public MachineGun()
        {
            helper = new SpecialEffectsExamplesLibrary.Helper();
        }

        public override void LoadContent(ContentManager content, GraphicsDevice graphicsDevice, string meshFileName)
        {
            animation = new AnimationSequence(graphicsDevice);
            animation.AddFrame(content.Load<Texture2D>("Ch21p1_MachineGunFlare"), 0.025f);
            muzzleFlare = new Sprite
            {
                AnimationSequemce = animation
            };

            base.LoadContent(content, graphicsDevice, meshFileName);
        }

        public override bool CanFire()
        {
            return !Timer.IsRunning();
        }

        public override void Fire(Camera camera)
        {
            Timer.Start();
            muzzleFlare.Timer.Begin();
            muzzleFlare.Size = helper.RandomNumber(1.0f, 3.0f);
            muzzleFlare.RotationRoll = helper.RandomNumber(0.0f, 2.0f * (float)Math.PI);
        }

        public override void Update(GameTime gameTime)
        {
            if ((Timer.Time / 1000.0f) > 0.05f)
            {
                Timer.Stop();
            }
        }

        public override void Draw(GraphicsDevice graphicsDevice, Camera camera, Matrix projectionMatrix, GameTime gameTime = null)
        {
            // if firing, render machinegun flare
            if (Timer.IsRunning())
            {
                var inverseView = Matrix.Invert(camera.ViewMatrix);
                var translationMatrix = Matrix.CreateTranslation(0.0f, 0.0f, -2.8f);
                var rotationMatrix = Matrix.CreateFromYawPitchRoll((float)Math.PI, 0.0f, 0.0f);
                var gunTranslationMatrix = Matrix.CreateTranslation(Position.X, Position.Y, Position.Z);

                var emitterWorldMatrix = rotationMatrix * gunTranslationMatrix * translationMatrix * inverseView;

                muzzleFlare.Position = emitterWorldMatrix.Translation;

                var state = new BlendState
                {
                    ColorSourceBlend = Blend.One,
                    AlphaSourceBlend = Blend.One,
                    ColorDestinationBlend = Blend.One,
                    AlphaDestinationBlend = Blend.One
                };

                graphicsDevice.BlendState = state;
                graphicsDevice.DepthStencilState = DepthStencilState.None;

                BasicEffect effect = new BasicEffect(graphicsDevice)
                {
                    LightingEnabled = false,
                    World = emitterWorldMatrix,
                    Projection = projectionMatrix,
                    View = camera.ViewMatrix,
                };

                foreach (EffectPass pass in effect.CurrentTechnique.Passes)
                {
                    pass.Apply();

                    muzzleFlare.Draw(graphicsDevice, camera.ViewMatrix, projectionMatrix);
                }
            }

            // render gun model
            Vector3 translation;

            if (Timer.IsRunning())
            {
                translation = new Vector3(0.0f, 0.0f, helper.RandomNumber(0.0f, -0.5f));
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
                    // create a light
                    // Machine gun looks white
                    effect.LightingEnabled = true;
                    effect.AmbientLightColor = new Vector3(0.1f, 0.1f, 0.1f);
                    effect.DirectionalLight0.DiffuseColor = new Vector3(0.3f, 0.3f, 0.3f);
                    effect.DirectionalLight0.Direction = new Vector3(1, -1, 1);  // coming along the x-axis
                    effect.DirectionalLight0.SpecularColor = new Vector3(0.0f, 1.0f, 1.0f); // with green highlights
                    effect.World = worldMatrix;
                    effect.Projection = projectionMatrix;
                    effect.View = camera.ViewMatrix;
                }

                mesh.Draw();
            }
        }
    }
}
