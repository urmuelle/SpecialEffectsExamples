// <copyright file="ParticleEventSequence.cs" company="Urs Müller">
// Copyright (c) Urs Müller. All rights reserved.
// </copyright>

namespace AdvancedParticleSystemMG38_1.ParticleSystem.Parser
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Content;
    using Microsoft.Xna.Framework.Graphics;
    using SpecialEffectsExamplesLibrary;

    public class ParticleEventSequence
    {
        private const int MaxNumParticles = 2000;
        private Blend destBlendMode;
        private Blend srcBlendMode;

        private float numNewPartsExcess;
        private RecyclingArray<Particle>? particles;
        private int totalParticleLives;
        private int loops;
        private int numParticles;
        private MinMax<Vector3> gravity;
        private MinMax<Vector3> emitRadius;
        private MinMax<float> emitRate;

        private Helper helper;

        public List<ParticleEvent> Events;

        public ParticleEventSequence()
        {
            Texture = null;
            particles = null;
            helper = new Helper();
            Events = new List<ParticleEvent>();
            Reset();
        }

        public string Name { get; internal set; }

        public MinMax<float> EmitRate
        {
            get { return emitRate; }
            set { emitRate = value; }
        }

        public MinMax<float> Lifetime { get; internal set; }

        public int NumParticles
        {
            get { return this.numParticles; }
            set { this.numParticles = value; }
        }

        public int Loops
        {
            get { return this.loops; }
            set { this.loops = value; }
        }

        public MinMax<Vector3> SpawnDir { get; internal set; }

        public MinMax<Vector3> EmitRadius
        {
            get { return emitRadius; }
            set { emitRadius = value; }
        }

        public MinMax<Vector3> Gravity
        {
            get { return this.gravity; }
            set { this.gravity = value; }
        }

        public Blend SrcBlendMode
        {
            get { return this.srcBlendMode; }
            set { this.srcBlendMode = value; }
        }

        public Blend DestBlendMode
        {
            get { return this.destBlendMode; }
            set { this.destBlendMode = value; }
        }

        public string TextureFilename { get; internal set; }

        public Texture2D? Texture { get; set; }

        public void LoadContent(ContentManager contentManager, GraphicsDevice graphicsDevice)
        {
            particles = new RecyclingArray<Particle>(MaxNumParticles);
            Texture = contentManager.Load<Texture2D>(TextureFilename);
        }

        public void RunEvents(ref Particle part)
        {
            int i;

            // apply any other events to this particle
            for (i = part.CurEvent; i < Events.Count && Events[i].ActualTime <= part.EventTimer; i++)
            {
                float oldeventtimer = part.EventTimer;

                Events[i].DoItToIt(ref part);

                if (part.EventTimer != oldeventtimer)
                {
                    int recalcIter;

                    // event timer has changed, we need to recalc m_CurEvent.
                    for (recalcIter = 0; recalcIter < Events.Count && Events[recalcIter].ActualTime < part.EventTimer;
                         recalcIter++)
                    {
                    }

                    // set our main iterator to the recalculated iterator
                    // the -1 just compensates for the i++ in the main for loop
                    i = recalcIter - 1;
                }
            }

            part.CurEvent = i;
        }

        public void DeleteAllParticles()
        {
            particles.DeleteAll();
            totalParticleLives = 0;
        }

        public void Reset()
        {
            totalParticleLives = 0;
            loops = -1;

            numParticles = 100;
            numNewPartsExcess = 0.0f;
            gravity = new MinMax<Vector3>(new Vector3(0.0f, 0.0f, 0.0f), new Vector3(0.0f, 0.0f, 0.0f));
            emitRadius = new MinMax<Vector3>(new Vector3(0.0f, 0.0f, 0.0f), new Vector3(0.0f, 0.0f, 0.0f));
            Events.Clear();
            particles = null;
            Texture = null;
        }

        public void CreateNewParticle(Vector3 partSysPos)
        {
            var part = particles.New();
            part.ResetParticle();
            part.Alive = true;

            part.Lifetime = Lifetime.GetRandomNumInRange();
            part.Position = partSysPos + EmitRadius.GetRandomNumInRange();

            var i = 0;

            // process any initial events
            foreach (var ev in Events)
            {
                if (ev.ActualTime == 0)
                {
                    ev.DoItToIt(ref part);
                    i++;
                }
            }

            part.CurEvent = i;
            totalParticleLives++;
        }

        public void CreateFadeLists()
        {
            // for each event,
            for (var i = 0; i < Events.Count; i++)
            {
                // try to find the next fade event of the same type.
                for (var j = i; j < Events.Count; j++)
                {
                    if (j != i && (Events[j].GetType() == Events[i].GetType()) && Events[j].IsFade())
                    {
                        // we've found a fade event further in the future.  make a note that
                        // this event needs to be linked to this future fade event (so that we
                        // can calculate the deltas later).
                        Events[i].NextFadeEvent = Events[j];
                        break;
                    }
                }
            }
        }

        public void SortEvents()
        {
            List<ParticleEvent> sortedList = Events.OrderBy(o => o.ActualTime).ToList();
            Events = sortedList;
        }

        public void NailDownRandomTimes()
        {
            foreach (var ev in Events)
            {
                ev.ActualTime = ev.TimeRange.GetRandomNumInRange();
            }
        }

        public void Update(GameTime gameTime, Vector3 partSysPos)
        {
            var elapsedTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (particles == null)
            {
                return;
            }

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
                        part.Direction += elapsedTime * gravity.GetRandomNumInRange();
                        RunEvents(ref part);
                    }
                }
            }

            // create new particles
            float emitRateThisFrame = emitRate.GetRandomNumInRange();
            int numNewParts = (int)(emitRateThisFrame * elapsedTime);
            numNewPartsExcess += (float)(emitRateThisFrame * elapsedTime) - numNewParts;

            if (numNewPartsExcess > 1.0f)
            {
                numNewParts += (int)numNewPartsExcess;
                numNewPartsExcess -= (int)numNewPartsExcess;
            }

            if (loops > 0 && totalParticleLives + numNewParts > loops * numParticles)
            {
                numNewParts = (loops * numParticles) - totalParticleLives;

                if (numNewParts <= 0)
                {
                    numNewParts = 0;
                }
            }

            for (int q = 0; q < numNewParts && particles.GetNumFreeElements() > 0; q++)
            {
                try
                {
                    CreateNewParticle(partSysPos);
                }
                catch (Exception ex)
                {
                    q = numNewParts;
                }
            }
        }

        public void Draw(GameTime gameTime, Matrix viewMatrix, Matrix projectionMatrix, GraphicsDevice graphicsDevice, DynamicVertexBuffer vertexBuffer, IndexBuffer indexBuffer)
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
                Texture = Texture,
                World = Matrix.Identity,
                View = viewMatrix,
                Projection = projectionMatrix,
                TextureEnabled = true,
                VertexColorEnabled = true,
            };

            // Set other Render States
            RasterizerState rasterizerState = new ()
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
