// <copyright file="PlasmaGun.cs" company="Urs Müller">
// Copyright (c) Urs Müller. All rights reserved.
// </copyright>

namespace FiringRange
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Content;
    using Microsoft.Xna.Framework.Graphics;
    using SpecialEffectsExamplesLibrary.Camera;

    public class PlasmaGun : Gun
    {
        private PlasmaBulletArray bulletArray;
        private SpecialEffectsExamplesLibrary.Helper helper;

        public PlasmaGun()
        {
            helper = new SpecialEffectsExamplesLibrary.Helper();
        }

        public PlasmaBulletArray BulletArray
        {
            get { return bulletArray; }
            set { bulletArray = value; }
        }

        public override void LoadContent(ContentManager content, GraphicsDevice graphicsDevice, string meshFileName)
        {
            bulletArray = new PlasmaBulletArray();
            bulletArray.LoadContent(content, graphicsDevice);

            base.LoadContent(content, graphicsDevice, meshFileName);
        }

        public override bool CanFire()
        {
            return !Timer.IsRunning();
        }

        public override void Fire(Camera camera)
        {
            Matrix matWorld, matInvView;

            Vector3 position = new Vector3(0.0f, 0.0f, 0.0f);
            Vector3 velocity = new Vector3(0.0f, 0.0f, -1.0f);

            Vector3 vPositionOut, vVelocityOut;

            // invert view and remove translation
            matInvView = Matrix.Invert(camera.ViewMatrix);
            matInvView.Translation = Vector3.Zero;

            matWorld = AssembleWorldMatrix(camera, Vector3.Zero);

            vPositionOut = Vector3.Transform(position, matWorld);
            vVelocityOut = Vector3.Transform(velocity, matInvView);

            bulletArray.AddBullet(vPositionOut, vVelocityOut * 50.0f);

            Timer.Begin();
        }

        public override void Update(GameTime gameTime)
        {
            if ((Timer.Time / 1000.0f) > 0.1f)
            {
                Timer.Stop();
            }
        }

        public override void Draw(GraphicsDevice graphicsDevice, Camera camera, Matrix projectionMatrix, GameTime gameTime)
        {
            /*
            // create a light
            {
                D3DLIGHT8 light; ZeroMemory(&light, sizeof(D3DLIGHT8));

                // plasma gun looks metallic & while
                D3DUtil_InitLight(light, D3DLIGHT_DIRECTIONAL, -10.0f, 20.0f, -20.0f);
                light.Ambient.a = 1.0f;
                light.Ambient.g = 0.1f;
                light.Ambient.b = light.Ambient.r = 0.0f;
                light.Diffuse.a = 1.0f;
                light.Diffuse.g = 0.3f;
                light.Diffuse.b = light.Diffuse.r = 0.0f;
                light.Direction = D3DXVECTOR3(1.0f, -1.0f, 1.0f);

                m_pd3dDevice->SetRenderState(D3DRS_LIGHTING, TRUE);
                m_pd3dDevice->SetLight(0, &light);
                m_pd3dDevice->LightEnable(0, true);
            }
            */

            // render gun model
            Vector3 translation;

            float fOffsetZ = -1.0f + ((Timer.Time / 1000.0f) * 5.0f);

            if (fOffsetZ > 0.0f)
            {
                fOffsetZ = 0.0f;
            }

            if (Timer.IsRunning())
            {
                translation = new Vector3(0.0f, 0.0f, fOffsetZ);
            }
            else
            {
                translation = Vector3.Zero;
            }

            var worldMatrix = AssembleWorldMatrix(camera, translation);

            graphicsDevice.BlendState = BlendState.Opaque;
            graphicsDevice.DepthStencilState = DepthStencilState.Default;
            graphicsDevice.RasterizerState = RasterizerState.CullCounterClockwise;

            var center = Mesh.Meshes[0].BoundingSphere.Center;

            foreach (var mesh in Mesh.Meshes)
            {
                // "Effect" refers to a shader. Each mesh may
                // have multiple shaders applied to it for more
                // advanced visuals.
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.LightingEnabled = false;
                    effect.World = worldMatrix;
                    effect.Projection = projectionMatrix;
                    effect.View = camera.ViewMatrix;
                }

                mesh.Draw();
            }
        }
    }
}
