// <copyright file="ParticleEmitter.cs" company="Urs Müller">
// Copyright (c) Urs Müller. All rights reserved.
// </copyright>

namespace AdvancedParticleSystem.ParticleSystem
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using AdvancedParticleSystem.ParticleSystem.Parser;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Content;
    using Microsoft.Xna.Framework.Graphics;
    using SpecialEffectsExamplesLibrary;

    public class ParticleEmitter
    {
        private const int MaxNumParticles = 2000;

        private string lastError;
        private int numParticles;

        private Vector3 pos;
        private MinMax<Vector3> posRange;
        private string name;

        // A vertex buffer holding our particles. This contains the same data as
        // the particles array, but copied across to where the GPU can access it.
        private DynamicVertexBuffer vertexBuffer;

        // Index buffer turns sets of four vertices into particle quads (pairs of triangles).
        private IndexBuffer indexBuffer;

        private List<ParticleEventSequence> sequences;

        private bool isRunning;

        public ParticleEmitter(ContentManager contentManager, GraphicsDevice graphicsDevice)
        {
            sequences = new List<ParticleEventSequence>();
        }

        public Vector3 Position
        {
            get { return pos; }
            set { pos = value; }
        }

        public MinMax<Vector3> PositionRange
        {
            get { return posRange; }
            set { posRange = value; }
        }

        public int NumParticles
        {
            get { return numParticles; }
            set { numParticles = value; }
        }

        public ParticleEvent EventFactory(string eventName)
        {
            if (eventName.Contains("SIZE"))
            {
                return new ParticleEventSize();
            }
            else if (eventName.Contains("EVENTTIMER"))
            {
                return new ParticleEventTimer();
            }
            else if (eventName.Contains("REDCOLOR"))
            {
                return new ParticleEventRedColor();
            }
            else if (eventName.Contains("GREENCOLOR"))
            {
                return new ParticleEventGreenColor();
            }
            else if (eventName.Contains("BLUECOLOR"))
            {
                return new ParticleEventBlueColor();
            }
            else if (eventName.Contains("ALPHA"))
            {
                return new ParticleEventAlpha();
            }
            else if (eventName.Contains("COLOR"))
            {
                return new ParticleEventColor();
            }
            else if (eventName.Contains("VELOCITYX"))
            {
                return new ParticleEventVelocityX();
            }
            else if (eventName.Contains("VELOCITYY"))
            {
                return new ParticleEventVelocityY();
            }
            else if (eventName.Contains("VELOCITYZ"))
            {
                return new ParticleEventVelocityZ();
            }
            else if (eventName.Contains("VELOCITY"))
            {
                return new ParticleEventVelocity();
            }

            return null;
        }

        public void Compile(string script)
        {
            lastError = "0 error(s), you're good to go!";

            try
            {
                ParticleEmitterTokenizer tokenizer = new ParticleEmitterTokenizer();

                Init();

                // parse the character string into tokens.
                tokenizer.Tokenize(script);

                var tokenIter = tokenizer.TokenVector.GetEnumerator();

                ParticleEmitterTokenizer.NextToken(ref tokenIter);

                if (tokenizer.TokenVector.Count < 2)
                {
                    throw new Exception("This script is too small to be valid.");
                }

                // the first three tokens out of the gate should be
                // ParticleSystem, followed by a name and version number, then
                // an open brace.
                if (tokenIter.Current.Type != TokenType.KeywordParticleSystem)
                {
                    throw new Exception("First word must be ParticleSystem");
                }

                ParticleEmitterTokenizer.NextToken(ref tokenIter);

                if (tokenIter.Current.Type != TokenType.Quote)
                {
                    throw new Exception("Must name particle system");
                }

                name = tokenIter.Current.StringValue.RemoveQuotes();
                ParticleEmitterTokenizer.NextToken(ref tokenIter);

                if (tokenIter.Current.Type != TokenType.RealNumber)
                {
                    throw new Exception("Must have version number");
                }

                ParticleEmitterTokenizer.NextToken(ref tokenIter);

                if (tokenIter.Current.Type != TokenType.OpenBrace)
                {
                    throw new Exception("Missing opening brace for ParticleSystem block");
                }

                ProcessParticleSystemBlock(ref tokenIter);
            }
            catch (Exception ex)
            {
                lastError = ex.Message;
                Init();
                Debug.WriteLine(ex);
            }

            // do misc. processing and calcuations
            pos = posRange.GetRandomNumInRange();
        }

        private ParticleEventSequence ProcessEventSequenceBlock(ref List<ParticleEmitterToken>.Enumerator tokenIter)
        {
            bool startedProcessingEvents = false;
            ParticleEventSequence seq = new ParticleEventSequence();
            seq.Reset();

            // move past the event sequence keyword...
            ParticleEmitterTokenizer.NextToken(ref tokenIter);

            // next token should be the name of the sequence...
            if (tokenIter.Current.Type != TokenType.Quote)
            {
                throw new Exception("Must name particle sequence block!");
            }

            seq.Name = tokenIter.Current.StringValue;

            ParticleEmitterTokenizer.NextToken(ref tokenIter);

            // next token should be opening brace...
            if (tokenIter.Current.Type != TokenType.OpenBrace)
            {
                throw new Exception("Expected opening brace for particle sequence block!");
            }

            ParticleEmitterTokenizer.NextToken(ref tokenIter);

            while (tokenIter.Current.Type != TokenType.CloseBrace)
            {
                ParticleEmitterToken savedtoken;
                savedtoken = tokenIter.Current;

                // the first token here can be a SysNumberProperty, SysAlphaBlendModeProperty, SysVectorProperty,
                // or an EventSequence.
                switch (tokenIter.Current.Type)
                {
                    case TokenType.SeqNumericProp:
                        {
                            if (startedProcessingEvents)
                            {
                                throw new Exception("Cannot specify any sequence properties after specifying events.");
                            }

                            MinMax<float> number;

                            // the next 2 tokens should be an equals, and a number.
                            ParticleEmitterTokenizer.NextToken(ref tokenIter);

                            if (tokenIter.Current.Type != TokenType.Equals)
                            {
                                throw new Exception("Expected equals sign!");
                            }

                            ParticleEmitterTokenizer.NextToken(ref tokenIter);

                            number = ParticleEmitterTokenizer.ProcessNumber(ref tokenIter);

                            if (savedtoken.IsEmitRate())
                            {
                                seq.EmitRate = number;
                            }
                            else if (savedtoken.IsLifeTime())
                            {
                                seq.Lifetime = number;
                            }
                            else if (savedtoken.IsNumParticles())
                            {
                                seq.NumParticles = (int)number.Max;
                            }
                            else if (savedtoken.IsLoops())
                            {
                                seq.Loops = (int)number.Max;
                            }
                            else
                            {
                                throw new Exception("Unknown sequence numeric property!");
                            }
                        }

                        break;

                    case TokenType.SeqVectorProp:
                        {
                            if (startedProcessingEvents)
                            {
                                throw new Exception("Cannot specify any sequence properties after specifying events.");
                            }

                            MinMax<Vector3> v;

                            ParticleEmitterTokenizer.NextToken(ref tokenIter);

                            if (tokenIter.Current.Type != TokenType.Equals)
                            {
                                throw new Exception("Expected equals sign!");
                            }

                            ParticleEmitterTokenizer.NextToken(ref tokenIter);

                            v = ParticleEmitterTokenizer.ProcessVector(ref tokenIter);

                            if (savedtoken.IsSpawnDir())
                            {
                                seq.SpawnDir = v;
                            }
                            else if (savedtoken.IsEmitRadius())
                            {
                                seq.EmitRadius = v;
                            }
                            else if (savedtoken.IsGravity())
                            {
                                seq.Gravity = v;
                            }
                            else
                            {
                                throw new Exception("Unknown sequence vector property!");
                            }
                        }

                        break;

                    case TokenType.SeqAlphaBlendModeProp:
                        {
                            if (startedProcessingEvents)
                            {
                                throw new Exception("Cannot specify any sequence properties after specifying events.");
                            }

                            LocalBlendState alphablendmode;
                            ParticleEmitterTokenizer.NextToken(ref tokenIter);

                            if (tokenIter.Current.Type != TokenType.Equals)
                            {
                                throw new Exception("Expected equals sign!");
                            }

                            ParticleEmitterTokenizer.NextToken(ref tokenIter);

                            alphablendmode = ParticleEmitterTokenizer.ProcessAlphaBlendMode(ref tokenIter);

                            if (savedtoken.IsSrcBlendMode())
                            {
                                seq.SrcBlendMode = (Blend)alphablendmode;
                            }
                            else if (savedtoken.IsDestBlendMode())
                            {
                                seq.DestBlendMode = (Blend)alphablendmode;
                            }
                            else
                            {
                                throw new Exception("Unknown sequence alpha blending mode property!");
                            }
                        }

                        break;

                    case TokenType.KeywordTexture:
                        {
                            // TODO
                            if (startedProcessingEvents)
                            {
                                throw new Exception("Cannot specify any sequence properties after specifying events.");
                            }

                            ParticleEmitterTokenizer.NextToken(ref tokenIter);

                            if (tokenIter.Current.Type != TokenType.Equals)
                            {
                                throw new Exception("Expected equals sign!");
                            }

                            ParticleEmitterTokenizer.NextToken(ref tokenIter);

                            if (tokenIter.Current.Type != TokenType.Quote)
                            {
                                throw new Exception("Expected filename after texture sequence property.");
                            }

                            seq.TextureFilename = tokenIter.Current.StringValue.RemoveQuotes();
                            ParticleEmitterTokenizer.NextToken(ref tokenIter);
                        }

                        break;

                    case TokenType.KeywordFade:
                    case TokenType.KeywordAt:
                    case TokenType.KeywordInitial:
                    case TokenType.KeywordFinal:
                        {
                            startedProcessingEvents = true;

                            bool isFade = false;
                            MinMax<float> timeRange = new MinMax<float>();

                            // parse the time range section of the event line
                            ParticleEmitterTokenizer.ProcessTime(ref timeRange, ref isFade, 0, seq.Lifetime.Max, ref tokenIter);

                            if (tokenIter.Current.Type != TokenType.ParticleNumericProp && tokenIter.Current.Type != TokenType.ParticleVectorProp &&
                                tokenIter.Current.Type != TokenType.ParticleColorProp)
                            {
                                throw new Exception("Expecting particle property after time specifier!");
                            }

                            ParticleEvent newEvent = null;

                            try
                            {
                                // create the appropriate event
                                newEvent = EventFactory(tokenIter.Current.StringValue);

                                if (newEvent == null)
                                {
                                    throw new Exception("Unknown event type, or there was an error creating this event.");
                                }

                                // let the event parse the rest of its properties from the token stream.
                                if (isFade && !newEvent.FadeAllowed())
                                {
                                    throw new Exception("Fading is not supported on this event.");
                                }

                                newEvent.ProcessTokenStream(ref tokenIter);
                                newEvent.TimeRange = timeRange;
                                newEvent.SetFade(isFade);
                                seq.Events.Add(newEvent);
                            }
                            catch (Exception ex)
                            {
                                throw ex;
                            }
                        }

                        break;

                    default:
                        {
                            throw new Exception("Unexpected " + tokenIter.Current.StringValue + " in Sequence Block!");
                        }
                    }
                }

            seq.NailDownRandomTimes();
            seq.SortEvents();
            seq.CreateFadeLists();
            ParticleEmitterTokenizer.NextToken(ref tokenIter);

            return seq;
        }

        private void ProcessParticleSystemBlock(ref List<ParticleEmitterToken>.Enumerator tokenIter)
        {
            var startedProcessingSequences = false;

            // move past the opening brace...
            ParticleEmitterTokenizer.NextToken(ref tokenIter);

            while (tokenIter.Current.Type != TokenType.CloseBrace)
            {
                ParticleEmitterToken savedtoken;
                savedtoken = tokenIter.Current;

                // the first token here can be a SysNumberProperty, SysAlphaBlendModeProperty, SysVectorProperty,
                // or an EventSequence.
                switch (tokenIter.Current.Type)
                {
                    case TokenType.SysVectorProp:
                        {
                            if (startedProcessingSequences)
                            {
                                throw new Exception("Cannot specify any particle system properties after specifying sequences.");
                            }

                            ParticleEmitterTokenizer.NextToken(ref tokenIter);

                            if (tokenIter.Current.Type != TokenType.Equals)
                            {
                                throw new Exception("Expected equals sign!");
                            }

                            ParticleEmitterTokenizer.NextToken(ref tokenIter);

                            MinMax<Vector3> v = ParticleEmitterTokenizer.ProcessVector(ref tokenIter);

                            if (savedtoken.IsPosition())
                            {
                                PositionRange = v;
                            }
                            else
                            {
                                throw new Exception("Unknown particle system property!");
                            }
                        }

                        break;

                    case TokenType.KeywordEventSequence:
                        {
                            startedProcessingSequences = true;

                            ParticleEventSequence newseq = ProcessEventSequenceBlock(ref tokenIter);
                            sequences.Add(newseq);
                        }

                        break;

                    default:
                        {
                            throw new Exception("Unexpected " + tokenIter.Current.StringValue + " in Particle System Block!");
                        }
                }
            }

            var ended = tokenIter.MoveNext();

            if (ended)
            {
                throw new Exception("end - of - file expected");
            }
        }
        
        public void Init()
        {
            name = string.Empty;

            sequences.Clear();

            PositionRange = new MinMax<Vector3>(new Vector3(0.0f, 0.0f, 0.0f), new Vector3(0.0f, 0.0f, 0.0f));

            Stop();
        }

        public void Start()
        {
            isRunning = true;
        }

        public void Pause()
        {
            isRunning = false;
        }

        public void Stop()
        {
            Pause();
            DeleteAllParticles();
        }

        public void DeleteAllParticles()
        {
            foreach (var seq in sequences)
            {
                seq.DeleteAllParticles();
            }
        }

        public bool IsRunning()
        {
            return isRunning;
        }

        public void LoadContent(ContentManager contentManager, GraphicsDevice graphicsDevice)
        {
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

            foreach (var seq in sequences)
            {
                seq.LoadContent(contentManager, graphicsDevice);
            }
        }

        public void Update(GameTime gameTime)
        {
            if (isRunning)
            {
                foreach (var seq in sequences)
                {
                    seq.Update(gameTime, Position);
                }
            }
        }

        public void Draw(GameTime gameTime, Matrix viewMatrix, Matrix projectionMatrix, GraphicsDevice graphicsDevice)
        {
            if (IsRunning())
            {
                foreach (var seq in sequences)
                {
                    seq.Draw(gameTime, viewMatrix, projectionMatrix, graphicsDevice, vertexBuffer, indexBuffer);
                }
            }
        }
    }
}
