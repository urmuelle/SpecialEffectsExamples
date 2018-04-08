// <copyright file="Triangle.cs" company="Urs Müller">
// Copyright (c) Urs Müller. All rights reserved.
// </copyright>

namespace SimpleParticleSystem.Graphics
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Content;
    using Microsoft.Xna.Framework.Graphics;

    public class Triangle
    {
        private readonly Texture2D texture = null;

        private string textureFilename;

        private VertexBuffer vertexBuffer = null;
        private IndexBuffer indexBuffer = null;

        public Triangle(ContentManager contentManager, GraphicsDevice graphicsDevice, string textureFilename)
        {
            this.textureFilename = textureFilename;
            texture = contentManager.Load<Texture2D>(textureFilename);
            InitGraphics(graphicsDevice);
        }

        public void Draw(GraphicsDevice graphicsDevice, Matrix viewMatrix, Matrix projectionMatrix)
        {
            Matrix world = Matrix.CreateTranslation(0, 0, 0);

            var basicEffect = new BasicEffect(graphicsDevice)
            {
                Texture = texture,
                World = world,
                View = viewMatrix,
                Projection = projectionMatrix,
                VertexColorEnabled = true,
            };

            graphicsDevice.SetVertexBuffer(vertexBuffer);
            graphicsDevice.Indices = indexBuffer;

            RasterizerState rasterizerState = new RasterizerState()
            {
                CullMode = CullMode.None
            };

            graphicsDevice.RasterizerState = rasterizerState;

            foreach (EffectPass pass in basicEffect.CurrentTechnique.Passes)
            {
                pass.Apply();
                graphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, 2);
            }
        }

        private void InitGraphics(GraphicsDevice graphicsDevice)
        {
            VertexPositionColorTexture[] vertices = new VertexPositionColorTexture[4];
            vertices[0] = new VertexPositionColorTexture(new Vector3(-0.5f, 0, -0.5f), Color.Red, new Vector2(0.0f, 1.0f));
            vertices[1] = new VertexPositionColorTexture(new Vector3(+0.5f, 0, -0.5f), Color.Green, new Vector2(1.0f, 0.0f));
            vertices[2] = new VertexPositionColorTexture(new Vector3(-0.5f, 0, 0.5f), Color.Blue, new Vector2(0.0f, 0.0f));
            vertices[3] = new VertexPositionColorTexture(new Vector3(0.5f, 0, 0.5f), Color.Blue, new Vector2(0.0f, 0.0f));

            vertexBuffer = new VertexBuffer(graphicsDevice, typeof(VertexPositionColorTexture), 4, BufferUsage.WriteOnly);
            vertexBuffer.SetData(vertices);

            var indices = new short[6];
            indices[0] = 0;
            indices[1] = 1;
            indices[2] = 2;
            indices[3] = 1;
            indices[4] = 2;
            indices[5] = 3;

            indexBuffer = new IndexBuffer(graphicsDevice, IndexElementSize.SixteenBits, sizeof(short) * indices.Length, BufferUsage.WriteOnly);
            indexBuffer.SetData(indices);
        }
    }
}
