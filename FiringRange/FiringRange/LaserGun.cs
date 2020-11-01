// <copyright file="LaserGun.cs" company="Urs Müller">
// Copyright (c) Urs Müller. All rights reserved.
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
    using SpecialEffectsExamplesLibrary.ParticleSystem.AdvancedParticleSystem;

    public class LaserGun : Gun
    {
        private AnimationSequence animation;
        private SpecialEffectsExamplesLibrary.Helper helper;
        private ParticleEmitter particleEmitter;
        private Matrix worldMatrix;
        private VertexBuffer vertexBuffer;
        private IndexBuffer indexBuffer;
        private Texture2D tex;

        public LaserGun()
        {
            helper = new SpecialEffectsExamplesLibrary.Helper();
        }

        public override void LoadContent(ContentManager content, GraphicsDevice graphicsDevice, string meshFileName)
        {
            animation = new AnimationSequence(graphicsDevice);
            animation.AddFrame(content.Load<Texture2D>("Ch21p1_Laser"), 200000.0f);

            tex = content.Load<Texture2D>("Ch21p1_Laser");

            // Create a dynamic vertex buffer.
            vertexBuffer = new DynamicVertexBuffer(graphicsDevice, VertexPositionColorTexture.VertexDeclaration, 4, BufferUsage.WriteOnly);

            indexBuffer = new IndexBuffer(graphicsDevice, typeof(ushort), 6, BufferUsage.WriteOnly);

            // Create and populate the index buffer.
            ushort[] indices = new ushort[6];

            indices[0] = 3;
            indices[1] = 2;
            indices[2] = 1;
            indices[3] = 3;
            indices[4] = 1;
            indices[5] = 0;

            indexBuffer.SetData(indices);

            particleEmitter = content.Load<ParticleEmitter>("Ch21p1_LaserScript");

            base.LoadContent(content, graphicsDevice, meshFileName);
        }

        public override bool CanFire()
        {
            return !Timer.IsRunning();
        }

        public override void Fire(Camera camera)
        {
            Timer.Start();
            particleEmitter.Start();

            var matInvView = Matrix.Invert(camera.ViewMatrix);
            var matTrans = Matrix.CreateTranslation(0.0f, 0.0f, 0.0f);
            var matRot = Matrix.CreateFromYawPitchRoll((float)Math.PI, 0.0f, 0.0f);
            var matGunTrans = Matrix.CreateTranslation(Position.X, Position.Y, Position.Z);
            worldMatrix = matRot * matGunTrans * matTrans * matInvView;

            matTrans = Matrix.CreateTranslation(0.0f, 0.0f, -2.8f);
            var matEmitterWorld = matRot * matGunTrans * matTrans * matInvView;

            particleEmitter.Position = new Vector3(
              matEmitterWorld.M41,
              matEmitterWorld.M42,
              matEmitterWorld.M43);
        }

        public override void Update(GameTime gameTime)
        {
            particleEmitter.Update(gameTime);

            if ((Timer.Time / 1000.0f) > 1.0f)
            {
                Timer.Stop();
                particleEmitter.Stop();
            }
        }

        public override void Draw(GraphicsDevice graphicsDevice, Camera camera, Matrix projectionMatrix, GameTime gameTime)
        {
            // if firing, render laser
            if (Timer.IsRunning())
            {
                float a = 1.0f - (Timer.Time * 2);

                if (a < 0.0f)
                {
                    a = 0.0f;
                }

                var vertices = new VertexPositionColorTexture[4];

                vertices[0].Color = new Color(1.0f, 0.0f, 0.0f, 0.0f);
                vertices[1].Color = new Color(1.0f, 0.0f, 0.0f, a);
                vertices[2].Color = new Color(1.0f, 0.0f, 0.0f, 0.0f);
                vertices[3].Color = new Color(1.0f, 0.0f, 0.0f, a);

                vertices[0].TextureCoordinate.X = 0.0f;
                vertices[0].TextureCoordinate.Y = 0.0f;
                vertices[1].TextureCoordinate.X = 0.0f;
                vertices[1].TextureCoordinate.Y = 1.0f;
                vertices[2].TextureCoordinate.X = 1.0f;
                vertices[2].TextureCoordinate.Y = 0.0f;
                vertices[3].TextureCoordinate.X = 1.0f;
                vertices[3].TextureCoordinate.Y = 1.0f;

                vertices[0].Position = new Vector3(-0.15f, -0.05f, 2.0f);
                vertices[1].Position = new Vector3(-0.15f, -0.05f, 100.0f);
                vertices[2].Position = new Vector3(0.15f, -0.05f, 2.0f);
                vertices[3].Position = new Vector3(0.15f, -0.05f, 100.0f);

                // now render!
                var shaderEffect = new BasicEffect(graphicsDevice)
                {
                    Texture = animation.GetCurFrameTexture(Timer),
                    World = worldMatrix,
                    View = camera.ViewMatrix,
                    Projection = projectionMatrix,
                    LightingEnabled = false,
                    TextureEnabled = true,
                };

                // Set other Render States
                RasterizerState rasterizerState = new RasterizerState()
                {
                    CullMode = CullMode.None,
                };

                graphicsDevice.RasterizerState = rasterizerState;
                graphicsDevice.DepthStencilState = DepthStencilState.Default;
                graphicsDevice.BlendState = BlendState.Opaque;
                graphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;

                graphicsDevice.RasterizerState = rasterizerState;
                graphicsDevice.DepthStencilState = DepthStencilState.Default;
                graphicsDevice.BlendState = BlendState.AlphaBlend;
                graphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;

                // Activate the particle effect.
                foreach (EffectPass pass in shaderEffect.CurrentTechnique.Passes)
                {
                    pass.Apply();

                    graphicsDevice.DrawUserPrimitives<VertexPositionColorTexture>(PrimitiveType.TriangleStrip, vertices, 0, 2);
                }

                graphicsDevice.BlendState = BlendState.Opaque;
            }

            // render gun model
            float fOffsetZ = -1.0f + ((Timer.Time / 1000) * 3);

            if (fOffsetZ > 0.0f)
            {
                fOffsetZ = 0.0f;
            }

            Vector3 translation;

            if (Timer.IsRunning())
            {
                translation = new Vector3(0.0f, 0.0f, fOffsetZ);
            }
            else
            {
                translation = Vector3.Zero;
            }

            var worldMatrixTemp = AssembleWorldMatrix(camera, translation);

            graphicsDevice.BlendState = BlendState.Opaque;
            graphicsDevice.DepthStencilState = DepthStencilState.Default;
            graphicsDevice.RasterizerState = RasterizerState.CullCounterClockwise;

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
                    effect.AmbientLightColor = new Vector3(0.0f, 0.0f, 0.1f);
                    effect.DirectionalLight0.DiffuseColor = new Vector3(0.0f, 0.0f, 0.3f);
                    effect.DirectionalLight0.Direction = new Vector3(1, -1, 1);
                    effect.DirectionalLight0.SpecularColor = new Vector3(1.0f, 0.0f, 0.0f);
                    effect.World = worldMatrixTemp;
                    effect.Projection = projectionMatrix;
                    effect.View = camera.ViewMatrix;
                }

                mesh.Draw();
            }

            // render particle system
            graphicsDevice.DepthStencilState = DepthStencilState.Default;
            graphicsDevice.BlendState = BlendState.AlphaBlend;

            particleEmitter.Draw(graphicsDevice, gameTime, camera.ViewMatrix, projectionMatrix);
        }
    }
}
