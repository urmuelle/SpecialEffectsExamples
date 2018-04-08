// <copyright file="D3DHelperFuncs.cs" company="Urs Müller">
// Copyright (c) Urs Müller. All rights reserved.
// </copyright>

namespace SpecialEffectsExamplesLibrary
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Content;
    using Microsoft.Xna.Framework.Graphics;

    public class D3DHelperFuncs
    {
        public static void CreateQuad(
            GraphicsDevice graphicsDevice,
            ref VertexBuffer vertexBuffer,
            float size,
            Color color,
            float texTileX = 1.0f,
            float texTileY = 1.0f)
        {
            // Create a dynamic vertex buffer.
            vertexBuffer = new VertexBuffer(graphicsDevice, VertexPositionColorTexture.VertexDeclaration, 6, BufferUsage.WriteOnly);

            var vertices = new VertexPositionColorTexture[6];

            var sizeDiv2 = size / 2;

            // first triangle
            vertices[0].Position = new Vector3(-sizeDiv2, sizeDiv2, 0.0f);
            vertices[0].Color = color;
            vertices[0].TextureCoordinate.X = 0.0f;
            vertices[0].TextureCoordinate.Y = 0.0f;

            vertices[1].Position = new Vector3(sizeDiv2, sizeDiv2, 0.0f);
            vertices[1].Color = color;
            vertices[1].TextureCoordinate.X = texTileX;
            vertices[1].TextureCoordinate.Y = 0.0f;

            vertices[2].Position = new Vector3(sizeDiv2, -sizeDiv2, 0.0f);
            vertices[2].Color = color;
            vertices[2].TextureCoordinate.X = texTileX;
            vertices[2].TextureCoordinate.Y = texTileY;

            // second triangle
            vertices[3].Position = new Vector3(-sizeDiv2, sizeDiv2, 0.0f);
            vertices[3].Color = color;
            vertices[3].TextureCoordinate.X = 0.0f;
            vertices[3].TextureCoordinate.Y = 0.0f;

            vertices[4].Position = new Vector3(sizeDiv2, -sizeDiv2, 0.0f);
            vertices[4].Color = color;
            vertices[4].TextureCoordinate.X = texTileX;
            vertices[4].TextureCoordinate.Y = texTileY;

            vertices[5].Position = new Vector3(-sizeDiv2, -sizeDiv2, 0.0f);
            vertices[5].Color = color;
            vertices[5].TextureCoordinate.X = 0.0f;
            vertices[5].TextureCoordinate.Y = texTileY;

            vertexBuffer.SetData(vertices.ToArray());
        }
    }
}
