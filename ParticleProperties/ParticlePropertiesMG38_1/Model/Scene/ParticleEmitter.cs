// <copyright file="ParticleEmitter.cs" company="Urs Müller">
// Copyright (c) Urs Müller. All rights reserved.
// </copyright>

namespace ParticlePropertiesMG38_1.Model.Scene
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Content;
    using Microsoft.Xna.Framework.Graphics;
    using SpecialEffectsExamplesLibrary;

    public class ParticleEmitter
    {
        private const int MaxNumParticles = 2000;

        private int numParticles;

        private string textureFilename;
        private Texture2D texture;

        private Vector3 gravity;
        private Vector3 pos;
        private float emitRateMin;
        private float emitRateMax;

        private Color startColor1;
        private Color startColor2;
        private Color startColor;

        private Color endColor1;
        private Color endColor2;
        private Color endColor;

        private float particleSizeMin;
        private float particleSizeMax;
        private float particleLifeTimeMin;
        private float particleLifeTimeMax;

        private Vector3 spawnDir1;
        private Vector3 spawnDir2;

        private Vector3 emitRadius;

        private Blend destBlendMode;
        private Blend srcBlendMode;

        private Helper helper;

        // A vertex buffer holding our particles. This contains the same data as
        // the particles array, but copied across to where the GPU can access it.
        private DynamicVertexBuffer vertexBuffer;

        // Index buffer turns sets of four vertices into particle quads (pairs of triangles).
        private IndexBuffer indexBuffer;

        private float numNewPartsExcess;
        private RecyclingArray<Particle> particles = new RecyclingArray<Particle>(MaxNumParticles);

        public ParticleEmitter(ContentManager contentManager, GraphicsDevice graphicsDevice, string textureFilename)
        {
            this.textureFilename = textureFilename;
            Initialize(contentManager, graphicsDevice);
            LoadContent(contentManager, graphicsDevice);

            // default particle system configuration - boring!
            Gravity = new Vector3(0.0f, 0.0f, 0.0f);
            Position = new Vector3(0.0f, 0.0f, 0.0f);
            EmitRateMin = 10.0f;
            EmitRateMax = 30.0f;
            startColor1 = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            startColor2 = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            endColor1 = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            endColor2 = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            ParticleSizeMin = 1.0f;
            ParticleSizeMax = 1.0f;

            spawnDir1 = new Vector3(-1.0f, -1.0f, -1.0f);
            ////spawnDir1.Normalize();
            ////spawnDir1 /= 100.0f;
            spawnDir2 = new Vector3(1.0f, 1.0f, 1.0f);
            ////spawnDir2.Normalize();
            ////spawnDir2 /= 100.0f;

            emitRadius = new Vector3(1.0f, 1.0f, 1.0f);

            destBlendMode = Blend.One;
            srcBlendMode = Blend.SourceAlpha;

            numParticles = 500;

            particleLifeTimeMin = 0;
            particleLifeTimeMax = 10;

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

        public Vector3 EmissionRadius
        {
            get { return emitRadius; }
            set { emitRadius = value; }
        }

        public Blend SourceBlendMode
        {
            get { return srcBlendMode; }
            set { srcBlendMode = value; }
        }

        public Blend DestBlendMode
        {
            get { return destBlendMode; }
            set { destBlendMode = value; }
        }

        public Color StartColor1
        {
            get { return startColor1; }
            set { startColor1 = value; }
        }

        public Color StartColor2
        {
            get { return startColor2; }
            set { startColor2 = value; }
        }

        public Color EndColor1
        {
            get { return endColor1; }
            set { endColor1 = value; }
        }

        public Color EndColor2
        {
            get { return endColor2; }
            set { endColor2 = value; }
        }

        public Vector3 Position
        {
            get { return pos; }
            set { pos = value; }
        }

        public int NumParticles
        {
            get { return numParticles; }
            set { numParticles = value; }
        }

        public Vector3 Gravity
        {
            get { return gravity; }
            set { gravity = value; }
        }

        public float EmitRateMin
        {
            get { return emitRateMin; }
            set { emitRateMin = value; }
        }

        public float EmitRateMax
        {
            get { return emitRateMax; }
            set { emitRateMax = value; }
        }

        public float ParticleSizeMin
        {
            get { return particleSizeMin; }
            set { particleSizeMin = value; }
        }

        public float ParticleSizeMax
        {
            get { return particleSizeMax; }
            set { particleSizeMax = value; }
        }

        public float ParticleLifeTimeMin
        {
            get { return particleLifeTimeMin; }
            set { particleLifeTimeMin = value; }
        }

        public float ParticleLifeTimeMax
        {
            get { return particleLifeTimeMax; }
            set { particleLifeTimeMax = value; }
        }

        public void Initialize(ContentManager contentManager, GraphicsDevice graphicsDevice)
        {
        }

        public void LoadContent(ContentManager contentManager, GraphicsDevice graphicsDevice)
        {
            texture = contentManager.Load<Texture2D>(textureFilename);

            // Create a dynamic vertex buffer.
            vertexBuffer = new DynamicVertexBuffer(graphicsDevice, VertexPositionColorTexture.VertexDeclaration, MaxNumParticles * 4, BufferUsage.WriteOnly);

            // Create and populate the index buffer.
            ushort[] indices = new ushort[MaxNumParticles * 6];

            for (int i = 0; i < MaxNumParticles; i++)
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

            // update existing particles
            for (int q = 0; q < particles.GetTotalElements(); q++)
            {
                if (particles.IsAlive(q))
                {
                    var part = particles.GetAt(q);

                    if (!part.Update(gameTime))
                    {
                        particles.Delete(part);
                    }
                    else
                    {
                        // apply gravity to this particle.
                        part.Direction += elapsedTime * gravity;
                    }
                }
            }

            // create new particles
            float emitRateThisFrame = helper.RandomNumber(emitRateMin, emitRateMax);
            int numNewParts = (int)(emitRateThisFrame * elapsedTime);
            numNewPartsExcess += (float)(emitRateThisFrame * elapsedTime) - numNewParts;

            if (numNewPartsExcess > 1.0f)
            {
                numNewParts += (int)numNewPartsExcess;
                numNewPartsExcess -= (int)numNewPartsExcess;
            }

            for (int q = 0; q < numNewParts && particles.GetNumUsedElements() < NumParticles; q++)
            {
                try
                {
                    var part = particles.New();
                    part.ResetParticle();
                    part.Alive = true;

                    part.Lifetime = helper.RandomNumber(particleLifeTimeMin, particleLifeTimeMax);
                    part.Size = helper.RandomNumber(particleSizeMin, particleSizeMax);

                    // determine a random vector between dir1 and dir2
                    float fRandX = helper.RandomNumber(spawnDir1.X, spawnDir2.X);
                    float fRandY = helper.RandomNumber(spawnDir1.Y, spawnDir2.Y);
                    float fRandZ = helper.RandomNumber(spawnDir1.Z, spawnDir2.Z);

                    part.Direction = new Vector3(fRandX, fRandY, fRandZ);

                    // TODO: This division should not be necessary. Check about Spawn Dir usage and adaptation.
                    part.Direction /= 4;
                    part.Position = Position;

                    // pick a random vector between +/- emitradius
                    fRandX = helper.RandomNumber(-emitRadius.X, emitRadius.X);
                    fRandY = helper.RandomNumber(-emitRadius.Y, emitRadius.Y);
                    fRandZ = helper.RandomNumber(-emitRadius.Z, emitRadius.Z);
                    part.Position += new Vector3(fRandX, fRandY, fRandZ);

                    float fRandR = helper.RandomNumber((float)startColor1.R, (float)startColor2.R);
                    float fRandG = helper.RandomNumber((float)startColor1.G, (float)startColor2.G);
                    float fRandB = helper.RandomNumber((float)startColor1.B, (float)startColor2.B);
                    float fRandA = helper.RandomNumber((float)startColor1.A, (float)startColor2.A);

                    startColor = new Color((int)fRandR, (int)fRandG, (int)fRandB, (int)fRandA);

                    fRandR = helper.RandomNumber((float)endColor1.R, (float)endColor2.R);
                    fRandG = helper.RandomNumber((float)endColor1.G, (float)endColor2.G);
                    fRandB = helper.RandomNumber((float)endColor1.B, (float)endColor2.B);
                    fRandA = helper.RandomNumber((float)endColor1.A, (float)endColor2.A);

                    endColor = new Color((int)fRandR, (int)fRandG, (int)fRandB, (int)fRandA);

                    part.Color = startColor;
                    part.ColorStep = new Color(
                        (endColor.R - startColor.R) / part.Lifetime,
                        (endColor.G - startColor.G) / part.Lifetime,
                        (endColor.B - startColor.B) / part.Lifetime,
                        (endColor.A - startColor.A) / part.Lifetime);
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
            var state = new BlendState
            {
                ColorSourceBlend = srcBlendMode,
                AlphaSourceBlend = srcBlendMode,
                ColorDestinationBlend = destBlendMode,
                AlphaDestinationBlend = destBlendMode,
            };

            graphicsDevice.BlendState = state;
            graphicsDevice.DepthStencilState = DepthStencilState.DepthRead;

            var shaderEffect = new BasicEffect(graphicsDevice)
            {
                Texture = texture,
                World = Matrix.Identity,
                View = viewMatrix,
                Projection = projectionMatrix,
                TextureEnabled = true,
                VertexColorEnabled = true,
            };

            // Set other Render States
            RasterizerState rasterizerState = new RasterizerState()
            {
                CullMode = CullMode.None,
            };

            graphicsDevice.RasterizerState = rasterizerState;

            // Set up the vertex buffer to be rendered
            var numParticlesToRender = 0;
            var vertices = new List<VertexPositionColorTexture>();

            // Render each particle
            for (int q = 0; q < MaxNumParticles; q++)
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
                            TextureCoordinate = (part.Corners[i] + new Vector2(1, 1)) / 2,
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
