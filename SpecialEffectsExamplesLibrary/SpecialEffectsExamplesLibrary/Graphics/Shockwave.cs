// <copyright file="Shockwave.cs" company="Urs Müller">
// Copyright (c) Urs Müller. All rights reserved.
// </copyright>

namespace SpecialEffectsExamplesLibrary.Graphics
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Content;
    using Microsoft.Xna.Framework.Graphics;

    public class Shockwave
    {
        private bool isRunning;
        private Texture2D texture;
        private VertexBuffer vertexBuffer = null;
        private Vector3 position;
        private int numDivisions;
        private float thickness;
        private float lifetime;
        private float age;
        private float expandRate;
        private float size;
        private float scale;
        private int numVerts;

        public Shockwave(GraphicsDevice graphicsDevice, ContentManager contentManager, string textureFilename, float size, float thickness, int numDivisions, float expandRate, float lifetime)
        {
            this.texture = null;
            this.vertexBuffer = null;
            this.numDivisions = 0;
            this.thickness = 0.0f;
            this.size = 0.0f;
            this.numVerts = 0;
            this.isRunning = false;
            this.texture = contentManager.Load<Texture2D>(textureFilename);
            InitShockwaveGraphics(graphicsDevice, size, thickness, numDivisions, expandRate, lifetime);
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
            }
        }

        public VertexBuffer VertexBuffer
        {
            get { return vertexBuffer; }
        }

        public virtual void Begin()
        {
            isRunning = true;
        }

        public virtual void Pause()
        {
            isRunning = false;
        }

        public virtual void Stop()
        {
            Pause();
            age = 0.0f;
            scale = 0.0001f;
        }

        public virtual bool IsRunning()
        {
            return isRunning;
        }

        public void SetAlpha(int alpha255)
        {
            VertexPositionColorTexture[] vertices = new VertexPositionColorTexture[vertexBuffer.VertexCount];
            vertexBuffer.GetData<VertexPositionColorTexture>(vertices);

            for (int q = 0; q < numVerts; q++)
            {
                vertices[q].Color = new Color(alpha255, 255, 255, 255);
            }

            vertexBuffer.SetData<VertexPositionColorTexture>(vertices);
        }

        public void Update(GameTime gameTime)
        {
            var timeDelta = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (IsRunning())
            {
                scale += expandRate * timeDelta;
                age += timeDelta;
                SetAlpha((int)(255.0f - (255.0f * (age / lifetime))));

                if (age > lifetime)
                {
                    Stop();
                }
            }
        }

        public void Draw(GraphicsDevice graphicsDevice, Matrix viewMatrix, Matrix projectionMatrix)
        {
            var matWorld = Matrix.CreateTranslation(position.X, position.Y, position.Z);
            var matScale = Matrix.CreateScale(scale, 1.0f, scale);
            var mat = matScale * matWorld;

            var shaderEffect = new BasicEffect(graphicsDevice)
            {
                Texture = texture,
                World = mat,
                View = viewMatrix,
                Projection = projectionMatrix,
                TextureEnabled = true
            };

            // Render States setzen
            RasterizerState rasterizerState = new RasterizerState()
            {
                CullMode = CullMode.None
            };

            graphicsDevice.RasterizerState = rasterizerState;
            graphicsDevice.BlendState = BlendState.Additive;
            graphicsDevice.DepthStencilState = DepthStencilState.DepthRead;
            graphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;

            graphicsDevice.SetVertexBuffer(vertexBuffer);

            foreach (var pass in shaderEffect.CurrentTechnique.Passes)
            {
                pass.Apply();
                graphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, 0, numVerts / 3);
            }
        }

        private void InitShockwaveGraphics(GraphicsDevice graphicsDevice, float size, float thickness, int numDivisions, float expandRate, float lifetime)
        {
            this.numDivisions = numDivisions;
            this.size = size;
            this.thickness = thickness;
            this.expandRate = expandRate;
            this.lifetime = lifetime;

            if (numDivisions < 4)
            {
                throw new ArgumentOutOfRangeException();
            }

            // create shockwave
            numVerts = numDivisions * 6;

            // Create vertex buffer for shockwave
            var vertices = new VertexPositionColorTexture[numVerts];

            // Fill vertex buffer

            // calculate number of vertices
            double fStep = 360.0f / numDivisions;
            int index = 0;

            for (double q = 0.0f; q < 360.0f; q += fStep)
            {
                // calculate x1, z1, x2, z2, x3,z3 and x4,z4 points
                float x1 = size * (float)Math.Cos(q.ToRadians());
                float z1 = size * (float)Math.Sin(q.ToRadians());
                float x2 = (size - thickness) * (float)Math.Cos(q.ToRadians());
                float z2 = (size - thickness) * (float)Math.Sin(q.ToRadians());

                float x3 = size * (float)Math.Cos((q + fStep).ToRadians());
                float z3 = size * (float)Math.Sin((q + fStep).ToRadians());
                float x4 = (size - thickness) * (float)Math.Cos((q + fStep).ToRadians());
                float z4 = (size - thickness) * (float)Math.Sin((q + fStep).ToRadians());

                vertices[index].Position.X = x2;
                vertices[index].Position.Y = 0.0f;
                vertices[index].Position.Z = z2;
                vertices[index].Color = new Color(255, 255, 255, 255);
                vertices[index].TextureCoordinate.X = 0.0f;
                vertices[index].TextureCoordinate.Y = 1.0f;
                index++;

                vertices[index].Position.X = x1;
                vertices[index].Position.Y = 0.0f;
                vertices[index].Position.Z = z1;
                vertices[index].Color = new Color(255, 255, 255, 255);
                vertices[index].TextureCoordinate.X = 0.0f;
                vertices[index].TextureCoordinate.Y = 0.0f;
                index++;

                vertices[index].Position.X = x4;
                vertices[index].Position.Y = 0.0f;
                vertices[index].Position.Z = z4;
                vertices[index].Color = new Color(255, 255, 255, 255);
                vertices[index].TextureCoordinate.X = 1.0f;
                vertices[index].TextureCoordinate.Y = 1.0f;
                index++;

                vertices[index].Position.X = x1;
                vertices[index].Position.Y = 0.0f;
                vertices[index].Position.Z = z1;
                vertices[index].Color = new Color(255, 255, 255, 255);
                vertices[index].TextureCoordinate.X = 0.0f;
                vertices[index].TextureCoordinate.Y = 0.0f;
                index++;

                vertices[index].Position.X = x3;
                vertices[index].Position.Y = 0.0f;
                vertices[index].Position.Z = z3;
                vertices[index].Color = new Color(255, 255, 255, 255);
                vertices[index].TextureCoordinate.X = 1.0f;
                vertices[index].TextureCoordinate.Y = 0.0f;
                index++;

                vertices[index].Position.X = x4;
                vertices[index].Position.Y = 0.0f;
                vertices[index].Position.Z = z4;
                vertices[index].Color = new Color(255, 255, 255, 255);
                vertices[index].TextureCoordinate.X = 1.0f;
                vertices[index].TextureCoordinate.Y = 1.0f;
                index++;
            }

            vertexBuffer = new VertexBuffer(graphicsDevice, typeof(VertexPositionColorTexture), vertices.Length, BufferUsage.None);
            vertexBuffer.SetData(vertices);
        }
    }
}