// <copyright file="ParticleEmmiter.cs" company="Urs Müller">
// Copyright (c) Urs Müller. All rights reserved.
// </copyright>

namespace SimpleParticleSystem_MG38.Particle_System
{
    using System;
    using System.Collections.Generic;
    using Helper;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Content;
    using Microsoft.Xna.Framework.Graphics;

    public class ParticleEmmiter
    {
        private const int NumParticles = 500;

        private string textureFilename;
        private Texture2D texture;

        private Vector3 gravity;
        private Vector3 pos;
        private float minEmitRate;
        private float maxEmitRate;

        private Color color1;
        private Color color2;

        private float minSize;
        private float maxSize;
        private Vector3 spawnDir1;
        private Vector3 spawnDir2;

        private Helper helper;

        // A vertex buffer holding our particles. This contains the same data as
        // the particles array, but copied across to where the GPU can access it.
        private DynamicVertexBuffer vertexBuffer;

        // Index buffer turns sets of four vertices into particle quads (pairs of triangles).
        private IndexBuffer indexBuffer;

        private float numNewPartsExcess;
        private RecyclingArray<Particle> particles = new RecyclingArray<Particle>(NumParticles);

        public ParticleEmmiter(ContentManager contentManager, GraphicsDevice graphicsDevice, string textureFilename)
        {
            this.textureFilename = textureFilename;
            Initialize(contentManager, graphicsDevice);
            LoadContent(contentManager, graphicsDevice);

            // default particle system configuration - boring!
            Gravity = new Vector3(0.0f, 0.0f, 0.0f);
            Position = new Vector3(0.0f, 0.0f, 0.0f);
            MinEmitRate = 30.0f;
            MaxEmitRate = 30.0f;
            Color1 = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            Color2 = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            MinSize = 1.0f;
            MaxSize = 1.0f;

            spawnDir1 = new Vector3(-1.0f, -1.0f, -1.0f);
            spawnDir1.Normalize();
            spawnDir1 /= 100.0f;
            spawnDir2 = new Vector3(1.0f, 1.0f, 1.0f);
            spawnDir2.Normalize();
            spawnDir2 /= 100.0f;

            // initialize misc. other things
            numNewPartsExcess = 0.0f;

            helper = new Helper();
        }

        public Vector3 SpawnDir1
        {
            get { return spawnDir1; }
            set { spawnDir1 = value; }
        }

        public Vector3 SpawnDir2
        {
            get { return spawnDir2; }
            set { spawnDir2 = value; }
        }

        public Color Color1
        {
            get { return color1; }
            set { color1 = value; }
        }

        public Color Color2
        {
            get { return color2; }
            set { color2 = value; }
        }

        public Vector3 Position
        {
            get { return pos; }
            set { pos = value; }
        }

        public Vector3 Gravity
        {
            get { return gravity; }
            set { gravity = value; }
        }

        public float MinEmitRate
        {
            get { return minEmitRate; }
            set { minEmitRate = value; }
        }

        public float MaxEmitRate
        {
            get { return maxEmitRate; }
            set { maxEmitRate = value; }
        }

        public float MinSize
        {
            get { return minSize; }
            set { minSize = value; }
        }

        public float MaxSize
        {
            get { return maxSize; }
            set { maxSize = value; }
        }

        public void Initialize(ContentManager contentManager, GraphicsDevice graphicsDevice)
        {
            particles = new RecyclingArray<Particle>(NumParticles);
        }

        public void LoadContent(ContentManager contentManager, GraphicsDevice graphicsDevice)
        {
            texture = contentManager.Load<Texture2D>(textureFilename);

            // Create a dynamic vertex buffer.
            vertexBuffer = new DynamicVertexBuffer(graphicsDevice, VertexPositionColorTexture.VertexDeclaration, NumParticles * 4, BufferUsage.WriteOnly);

            // Create and populate the index buffer.
            ushort[] indices = new ushort[NumParticles * 6];

            for (int i = 0; i < NumParticles; i++)
            {
                indices[(i * 6) + 0] = (ushort)((i * 4) + 0);
                indices[(i * 6) + 1] = (ushort)((i * 4) + 1);
                indices[(i * 6) + 2] = (ushort)((i * 4) + 2);

                indices[(i * 6) + 3] = (ushort)((i * 4) + 0);
                indices[(i * 6) + 4] = (ushort)((i * 4) + 2);
                indices[(i * 6) + 5] = (ushort)((i * 4) + 3);
            }

            indexBuffer = new IndexBuffer(graphicsDevice, typeof(ushort), indices.Length, BufferUsage.WriteOnly);

            indexBuffer.SetData(indices);
        }

        public void Update(GameTime gameTime)
        {
            var elapsedTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            for (int q = 0; q < particles.GetTotalElements(); q++)
            {
                if (particles.IsAlive(q))
                {
                    var part = particles.GetAt(q);

                    if (!part.Update(gameTime))
                    {
                        particles.Delete(part);
                    }
                }
            }

            // create new particles
            float emitRateThisFrame = helper.RandomNumber(minEmitRate, maxEmitRate);
            int numNewParts = (int)(emitRateThisFrame * elapsedTime);
            numNewPartsExcess += (float)(emitRateThisFrame * elapsedTime) - numNewParts;

            if (numNewPartsExcess > 1.0f)
            {
                numNewParts += (int)numNewPartsExcess;
                numNewPartsExcess -= (int)numNewPartsExcess;
            }

            for (int q = 0; q < numNewParts; q++)
            {
                try
                {
                    var part = particles.New();
                    part.ResetParticle();
                    part.Alive = true;

                    // determine a random vector between dir1 and dir2
                    float fRandX = helper.RandomNumber(spawnDir1.X, spawnDir2.X);
                    float fRandY = helper.RandomNumber(spawnDir1.Y, spawnDir2.Y);
                    float fRandZ = helper.RandomNumber(spawnDir1.Z, spawnDir2.Z);

                    part.Direction = new Vector3(fRandX, fRandY, fRandZ);

                    // TODO: This division should not be necessary. Check about Spawn Dir usage and adaptation.
                    part.Direction /= 4;
                    part.Position = Position;

                    float fRandR = helper.RandomNumber((float)color1.R, (float)color2.R);
                    float fRandG = helper.RandomNumber((float)color1.G, (float)color2.G);
                    float fRandB = helper.RandomNumber((float)color1.B, (float)color2.B);
                    float fRandA = helper.RandomNumber((float)color1.A, (float)color2.A);

                    part.Color = new Color((int)fRandR, (int)fRandG, (int)fRandB, (int)fRandA);
                }
                catch (InvalidOperationException ex)
                {
                    Console.WriteLine("FreeParticles.Dequeue threw exception: " + ex.Message);
                }
            }
        }

        public void Draw(GameTime gameTime, Matrix viewMatrix, Matrix projectionMatrix, GraphicsDevice graphicsDevice)
        {
            // setup alpha blending states
            graphicsDevice.BlendState = BlendState.Additive;
            graphicsDevice.DepthStencilState = DepthStencilState.DepthRead;

            var shaderEffect = new BasicEffect(graphicsDevice)
            {
                Texture = texture,
                World = Matrix.Identity,
                View = viewMatrix,
                Projection = projectionMatrix,
                TextureEnabled = true,
                VertexColorEnabled = true
            };

            // Set other Render States
            RasterizerState rasterizerState = new RasterizerState()
            {
                CullMode = CullMode.None
            };

            graphicsDevice.RasterizerState = rasterizerState;

            // Set up the vertex buffer to be rendered
            var numParticlesToRender = 0;
            var vertices = new List<VertexPositionColorTexture>();

            // Render each particle
            for (int q = 0; q < NumParticles; q++)
            {
                // Render each particle a bunch of times to get a blurring effect
                if (particles.IsAlive(q) == true)
                {
                    var part = particles.GetAt(q);

                    for (int i = 0; i < 4; i++)
                    {
                        var v = new VertexPositionColorTexture()
                        {
                            Position = part.VertexPositions[i],
                            Color = part.Color,
                            TextureCoordinate = (part.Corners[i] + new Vector2(1, 1)) / 2
                        };

                        vertices.Add(v);
                    }

                    numParticlesToRender++;
                }
            }

            if (numParticlesToRender > 0)
            {
                vertexBuffer.SetData(vertices.ToArray());

                // Set the particle vertex and index buffer.
                graphicsDevice.SetVertexBuffer(vertexBuffer);
                graphicsDevice.Indices = indexBuffer;

                // Activate the particle effect.
                foreach (EffectPass pass in shaderEffect.CurrentTechnique.Passes)
                {
                    pass.Apply();

                    graphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, numParticlesToRender * 2);
                }

                numParticlesToRender = 0;
            }

            // Reset some of the renderstates that we changed,
            // so as not to mess up any other subsequent drawing.
            graphicsDevice.DepthStencilState = DepthStencilState.Default;
        }
    }
}
