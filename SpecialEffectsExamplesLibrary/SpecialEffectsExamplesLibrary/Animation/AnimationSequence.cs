// <copyright file="AnimationSequence.cs" company="Urs Müller">
// Copyright (c) Urs Müller. All rights reserved.
// </copyright>
// <author>Urs Müller</author>

namespace SpecialEffectsExamplesLibrary.Animation
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    /// <summary>
    /// Class used for texture Animation Sequences
    /// </summary>
    public class AnimationSequence
    {
        private List<AnimationFrame> frames;
        private VertexBuffer vertexBuffer;

        /// <summary>
        /// Initializes a new instance of the <see cref="AnimationSequence"/> class.
        /// </summary>
        /// <param name="origin">The origin.</param>
        /// <param name="rotation">The rotation.</param>
        /// <param name="scale">The scale.</param>
        /// <param name="depth">The depth.</param>
        /// <param name="loop">if set to <c>true</c> [loop].</param>
        public AnimationSequence(GraphicsDevice graphicsDevice)
        {
            frames = new List<AnimationFrame>();
            D3DHelperFuncs.CreateQuad(graphicsDevice, ref vertexBuffer, 1.0f, new Color(255, 255, 255, 255));
        }

        /// <summary>
        /// Adds the frame.
        /// </summary>
        /// <param name="texture">The texture.</param>
        /// <param name="time">The time passed.</param>
        public void AddFrame(Texture2D texture, float time)
        {
            frames.Add(new AnimationFrame(texture, time));
        }

        public void DeleteFrame(int index)
        {
            frames.RemoveAt(index);
        }

        public void ClearAllFrames()
        {
            frames.Clear();
        }

        public int GetCurFrame(Timer timer)
        {
            int curFrame = 0;
            float timeCount = 0.0f;

            foreach (var frame in frames)
            {
                timeCount += frame.Time;
                if (timer.Time / 1000 <= timeCount)
                {
                    break;
                }

                curFrame++;
            }

            return curFrame;
        }

        public Texture2D GetCurFrameTexture(Timer timer)
        {
            try
            {
                int curFrame = GetCurFrame(timer);
                return frames[curFrame].Texture;
            }
            catch (Exception ex)
            {
            }

            return null;
        }

        /// <summary>
        /// Draws the frame.
        /// </summary>
        /// <param name="batch">The batch.</param>
        /// <param name="screenPos">The screen pos.</param>
        public void Draw(GraphicsDevice graphicsDevice, Timer timer, Matrix worldMatrix, Matrix viewMatrix, Matrix projectionMatrix)
        {
            if (timer.Time < 0)
            {
                return;
            }

            var curFrame = GetCurFrame(timer);

            if (curFrame >= frames.Count)
            {
                return;
            }

            var frame = frames[curFrame];

            var shaderEffect = new BasicEffect(graphicsDevice)
            {
                Texture = frame.Texture,
                World = worldMatrix,
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
            graphicsDevice.DepthStencilState = DepthStencilState.Default;
            graphicsDevice.DepthStencilState = DepthStencilState.DepthRead;
            graphicsDevice.SamplerStates[0] = SamplerState.LinearClamp;

            graphicsDevice.SetVertexBuffer(vertexBuffer);

            foreach (var pass in shaderEffect.CurrentTechnique.Passes)
            {
                pass.Apply();
                graphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, 0, 2);
            }
        }
    }
}
