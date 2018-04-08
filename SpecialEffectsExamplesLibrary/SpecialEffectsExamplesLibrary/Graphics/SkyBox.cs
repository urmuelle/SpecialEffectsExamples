// <copyright file="SkyBox.cs" company="Urs Müller">
// Copyright (c) Urs Müller. All rights reserved.
// </copyright>

namespace SpecialEffectsExamplesLibrary.Graphics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Content;
    using Microsoft.Xna.Framework.Graphics;

    public enum BoxFace
    {
        Top = 0,
        Bottom,
        Left,
        Right,
        Front,
        Back
    }

    public class SkyBox
    {
        private const int NumFaces = 6;

        private Texture2D[] textures = new Texture2D[6];
        private float size;

        public SkyBox(
            GraphicsDevice graphicsDevice,
            ContentManager contentManager,
            string topTextureFilename,
            string bottomTextureFilename,
            string frontTextureFilename,
            string backTextureFilename,
            string leftTextureFilename,
            string rightTextureFilename)
        {
            size = 10.0f;

            // Texturen einlesen
            textures[(int)BoxFace.Top] = contentManager.Load<Texture2D>(topTextureFilename);
            textures[(int)BoxFace.Bottom] = contentManager.Load<Texture2D>(bottomTextureFilename);
            textures[(int)BoxFace.Left] = contentManager.Load<Texture2D>(leftTextureFilename);
            textures[(int)BoxFace.Right] = contentManager.Load<Texture2D>(rightTextureFilename);
            textures[(int)BoxFace.Front] = contentManager.Load<Texture2D>(frontTextureFilename);
            textures[(int)BoxFace.Back] = contentManager.Load<Texture2D>(backTextureFilename);
        }

        public float Size
        {
            get { return size; }
            set { size = value; }
        }

        public void Draw(GraphicsDevice graphicsDevice, Matrix worldMatrix, Matrix viewMatrix, Matrix projectionMatrix)
        {
            VertexPositionTexture[] vertices = new VertexPositionTexture[4];

            var f = 0.5f / (float)textures[(int)BoxFace.Front].Width;

            vertices[0].TextureCoordinate.X = 0.0f + f;
            vertices[0].TextureCoordinate.Y = 0.0f + f;
            vertices[1].TextureCoordinate.X = 0.0f + f;
            vertices[1].TextureCoordinate.Y = 1.0f - f;
            vertices[2].TextureCoordinate.X = 1.0f - f;
            vertices[2].TextureCoordinate.Y = 0.0f + f;
            vertices[3].TextureCoordinate.X = 1.0f - f;
            vertices[3].TextureCoordinate.Y = 1.0f - f;

            f = size * 0.5f;

            // Kopie der Sichtmatrix erstellen und die Translation
            // unberücksichtigt lassen, da der Himmel unendlich erscheinen soll
            // unabhängig davon, wo wir uns im Level befinden
            Matrix view = viewMatrix;
            view.Translation = Vector3.Zero;

            // Graphics Device zum Rendern der Primitive vorbereiten

            // Tiefentest deaktivieren und Schreibschutz des Z-Buffers aktivieren
            ////Beim Rendern der Sky Box sollen keine Tiefenwerte in den Z-Buffer geschrieben
            ////werden, denn der Himmel liegt immer hinter allen anderen Objekten
            ////this.GraphicsDevice.RenderState.DepthBufferEnable = false;
            ////this.GraphicsDevice.RenderState.DepthBufferWriteEnable = false;
            ////this.GraphicsDevice.DepthStencilState.DepthBufferEnable = false;
            ////this.GraphicsDevice.DepthStencilState.DepthBufferWriteEnable = false;

            var shaderEffect = new BasicEffect(graphicsDevice)
            {
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
            graphicsDevice.BlendState = BlendState.Opaque;
            graphicsDevice.DepthStencilState = DepthStencilState.None;
            graphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;

            // left (negative x)
            vertices[0].Position = new Vector3(-f, f, f);
            vertices[1].Position = new Vector3(-f, -f, f);
            vertices[2].Position = new Vector3(-f, f, -f);
            vertices[3].Position = new Vector3(-f, -f, -f);

            shaderEffect.Texture = textures[(int)BoxFace.Left];

            foreach (var pass in shaderEffect.CurrentTechnique.Passes)
            {
                pass.Apply();
                graphicsDevice.DrawUserPrimitives<VertexPositionTexture>(PrimitiveType.TriangleStrip, vertices, 0, 2);
            }

            // right (positive x)
            vertices[0].Position = new Vector3(f, f, -f);
            vertices[1].Position = new Vector3(f, -f, -f);
            vertices[2].Position = new Vector3(f, f, f);
            vertices[3].Position = new Vector3(f, -f, f);

            shaderEffect.Texture = textures[(int)BoxFace.Right];

            foreach (var pass in shaderEffect.CurrentTechnique.Passes)
            {
                pass.Apply();
                graphicsDevice.DrawUserPrimitives<VertexPositionTexture>(PrimitiveType.TriangleStrip, vertices, 0, 2);
            }

            // down (negative y)
            vertices[0].Position = new Vector3(-f, -f, -f);
            vertices[1].Position = new Vector3(-f, -f, f);
            vertices[2].Position = new Vector3(f, -f, -f);
            vertices[3].Position = new Vector3(f, -f, f);

            shaderEffect.Texture = textures[(int)BoxFace.Bottom];

            foreach (var pass in shaderEffect.CurrentTechnique.Passes)
            {
                pass.Apply();
                graphicsDevice.DrawUserPrimitives<VertexPositionTexture>(PrimitiveType.TriangleStrip, vertices, 0, 2);
            }

            // up (positive y)
            vertices[0].Position = new Vector3(-f, f, f);
            vertices[1].Position = new Vector3(-f, f, -f);
            vertices[2].Position = new Vector3(f, f, f);
            vertices[3].Position = new Vector3(f, f, -f);

            shaderEffect.Texture = textures[(int)BoxFace.Top];

            foreach (var pass in shaderEffect.CurrentTechnique.Passes)
            {
                pass.Apply();
                graphicsDevice.DrawUserPrimitives<VertexPositionTexture>(PrimitiveType.TriangleStrip, vertices, 0, 2);
            }

            // back (negative z)
            vertices[0].Position = new Vector3(-f, f, -f);
            vertices[1].Position = new Vector3(-f, -f, -f);
            vertices[2].Position = new Vector3(f, f, -f);
            vertices[3].Position = new Vector3(f, -f, -f);

            shaderEffect.Texture = textures[(int)BoxFace.Back];

            foreach (var pass in shaderEffect.CurrentTechnique.Passes)
            {
                pass.Apply();
                graphicsDevice.DrawUserPrimitives<VertexPositionTexture>(PrimitiveType.TriangleStrip, vertices, 0, 2);
            }

            // front (positive z)
            vertices[0].Position = new Vector3(f, f, f);
            vertices[1].Position = new Vector3(f, -f, f);
            vertices[2].Position = new Vector3(-f, f, f);
            vertices[3].Position = new Vector3(-f, -f, f);

            shaderEffect.Texture = textures[(int)BoxFace.Front];

            foreach (var pass in shaderEffect.CurrentTechnique.Passes)
            {
                pass.Apply();
                graphicsDevice.DrawUserPrimitives<VertexPositionTexture>(PrimitiveType.TriangleStrip, vertices, 0, 2);
            }
        }
    }
}
