// <copyright file="GroundPlane.cs" company="Urs Müller">
// Copyright (c) Urs Müller. All rights reserved.
// </copyright>

namespace SimpleParticleSystemMG38_1.Graphics
{
    using System;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Content;
    using Microsoft.Xna.Framework.Graphics;

    public class GroundPlane : IDisposable
    {
        private readonly Texture2D texture = null;

        private float width;
        private float height;
        private int numVerts;
        private int numIndices;
        private int numGridSquares;

        private VertexBuffer vertexBuffer = null;
        private IndexBuffer indexBuffer = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="GroundPlane"/> class.
        /// </summary>
        /// <param name="contentManager">The content manager.</param>
        /// <param name="graphicsDevice">The graphics device.</param>
        /// <param name="textureFilename">The name of the texture file.</param>
        /// <param name="width">The width of the texture file.</param>
        /// <param name="height">The height of the texture file.</param>
        /// <param name="numGridSquares">The number of squares of the texture file.</param>
        public GroundPlane(ContentManager contentManager, GraphicsDevice graphicsDevice, string textureFilename, float width, float height, int numGridSquares)
        {
            this.numGridSquares = numGridSquares;
            this.height = height;
            this.width = width;
            this.texture = contentManager.Load<Texture2D>(textureFilename);
            this.InitTerrainGraphics(graphicsDevice);
        }

        /// <summary>
        /// Draws the specified game time.
        /// </summary>
        /// <param name="viewMatrix">The view matrix.</param>
        /// <param name="projectionMatrix">The projection matrix.</param>
        /// <param name="graphicsDevice">The graphics device.</param>
        public void Draw(Matrix viewMatrix, Matrix projectionMatrix, GraphicsDevice graphicsDevice)
        {
            var shaderEffect = new BasicEffect(graphicsDevice)
            {
                Texture = texture,
                World = Matrix.Identity,
                View = viewMatrix,
                Projection = projectionMatrix,
                TextureEnabled = true,
            };

            // Render States setzen
            RasterizerState rasterizerState = new ()
            {
                CullMode = CullMode.None,
            };

            graphicsDevice.RasterizerState = rasterizerState;
            graphicsDevice.BlendState = BlendState.Opaque;
            graphicsDevice.DepthStencilState = DepthStencilState.Default;
            graphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;

            graphicsDevice.SetVertexBuffer(vertexBuffer);
            graphicsDevice.Indices = indexBuffer;

            foreach (var pass in shaderEffect.CurrentTechnique.Passes)
            {
                pass.Apply();
                graphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, numIndices / 3);
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                vertexBuffer.Dispose();
                vertexBuffer = null;
                indexBuffer.Dispose();
                indexBuffer = null;
            }
        }

        /// <summary>
        /// Inits the terrain graphics.
        /// </summary>
        /// <param name="graphicsDevice">The graphics device.</param>
        private void InitTerrainGraphics(GraphicsDevice graphicsDevice)
        {
            // calculate the size of vertex/index buffers
            numVerts = (numGridSquares + 1) * (numGridSquares + 1);
            numIndices = numGridSquares * numGridSquares * 6;

            var vertices = new VertexPositionTexture[numVerts];

            var i = 0;

            // Fill in vertices
            for (int zz = 0; zz <= numGridSquares; zz++)
            {
                for (int xx = 0; xx <= numGridSquares; xx++)
                {
                    vertices[i].Position.X = width * (((float)xx / (float)numGridSquares) - 0.5f);
                    vertices[i].Position.Y = 0.0f;
                    vertices[i].Position.Z = height * (((float)zz / (float)numGridSquares) - 0.5f);
                    vertices[i].TextureCoordinate.X = (xx % 2) == 0 ? 0.0f : 1.0f;
                    vertices[i].TextureCoordinate.Y = (zz % 2) == 0 ? 0.0f : 1.0f;

                    i++;
                }
            }

            vertexBuffer = new VertexBuffer(graphicsDevice, typeof(VertexPositionTexture), vertices.Length, BufferUsage.None);
            vertexBuffer.SetData(vertices);

            var indices = new short[numIndices];

            i = 0;

            // Fill in indices
            for (int z = 0; z < numGridSquares; z++)
            {
                for (int x = 0; x < numGridSquares; x++)
                {
                    int vtx = x + (z * (numGridSquares + 1));

                    indices[i] = (short)(vtx + 1);
                    indices[i + 1] = (short)(vtx + 0);
                    indices[i + 2] = (short)(vtx + 0 + (numGridSquares + 1));
                    indices[i + 3] = (short)(vtx + 1);
                    indices[i + 4] = (short)(vtx + 0 + (numGridSquares + 1));
                    indices[i + 5] = (short)(vtx + 1 + (numGridSquares + 1));

                    i += 6;
                }
            }

            indexBuffer = new IndexBuffer(graphicsDevice, IndexElementSize.SixteenBits, sizeof(short) * indices.Length, BufferUsage.WriteOnly);
            indexBuffer.SetData(indices);
        }
    }
}