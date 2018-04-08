// <copyright file="MachineGun.cs" company="Urs Müller">
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
    using SpecialEffectsExamplesLibrary.Animation;
    using SpecialEffectsExamplesLibrary.Camera;

    public class MachineGun : Gun
    {
        private AnimationSequence animation;
        private Sprite muzzleFlare;
        private SpecialEffectsExamplesLibrary.Helper helper;

        public MachineGun()
        {
            helper = new SpecialEffectsExamplesLibrary.Helper();
        }

        public override void LoadContent(ContentManager content, GraphicsDevice graphicsDevice, string meshFileName)
        {
            animation = new AnimationSequence(graphicsDevice);
            animation.AddFrame(content.Load<Texture2D>("Ch21p1_MachineGunFlare"), 0.025f);
            muzzleFlare = new Sprite
            {
                AnimationSequemce = animation
            };

            base.LoadContent(content, graphicsDevice, meshFileName);
        }

        public override bool CanFire()
        {
            return !Timer.IsRunning();
        }

        public override void Fire(Camera camera)
        {
            Timer.Start();
            muzzleFlare.Timer.Begin();
            muzzleFlare.Size = helper.RandomNumber(1.0f, 3.0f);
            muzzleFlare.RotationRoll = helper.RandomNumber(0.0f, 2.0f * (float)Math.PI);
        }

        public override void Update(GameTime gameTime)
        {
            if ((Timer.Time / 1000.0f) > 0.05f)
            {
                Timer.Stop();
            }
        }

        public override void Draw(GraphicsDevice graphicsDevice, Camera camera, Matrix projectionMatrix, GameTime gameTime = null)
        {
            // create a light
            /*
            {
                D3DLIGHT8 light; ZeroMemory(&light, sizeof(D3DLIGHT8));

                // Machine gun looks white
                D3DUtil_InitLight(light, D3DLIGHT_DIRECTIONAL, -10.0f, 20.0f, -20.0f);
                light.Ambient.a = 1.0f;
                light.Ambient.g = 0.1f;
                light.Ambient.b = 0.1f;
                light.Ambient.r = 0.1f;
                light.Diffuse.a = 1.0f;
                light.Diffuse.g = 0.3f;
                light.Diffuse.b = 0.3f;
                light.Diffuse.r = 0.3f;
                light.Direction = D3DXVECTOR3(1.0f, -1.0f, 1.0f);

                m_pd3dDevice->SetRenderState(D3DRS_LIGHTING, TRUE);
                m_pd3dDevice->SetLight(0, &light);
                m_pd3dDevice->LightEnable(0, true);
            }
            */

            // if firing, render machinegun flare
            if (Timer.IsRunning())
            {
                var inverseView = Matrix.Invert(camera.ViewMatrix);
                var translationMatrix = Matrix.CreateTranslation(0.0f, 0.0f, -2.8f);
                var rotationMatrix = Matrix.CreateFromYawPitchRoll((float)Math.PI, 0.0f, 0.0f);
                var gunTranslationMatrix = Matrix.CreateTranslation(Position.X, Position.Y, Position.Z);

                var emitterWorldMatrix = rotationMatrix * gunTranslationMatrix * translationMatrix * inverseView;

                muzzleFlare.Position = emitterWorldMatrix.Translation;

                var state = new BlendState
                {
                    ColorSourceBlend = Blend.One,
                    AlphaSourceBlend = Blend.One,
                    ColorDestinationBlend = Blend.One,
                    AlphaDestinationBlend = Blend.One
                };

                graphicsDevice.BlendState = state;
                graphicsDevice.DepthStencilState = DepthStencilState.None;

                BasicEffect effect = new BasicEffect(graphicsDevice)
                {
                    LightingEnabled = false,
                    World = emitterWorldMatrix,
                    Projection = projectionMatrix,
                    View = camera.ViewMatrix,
                };

                /*
                m_pd3dDevice->SetRenderState(D3DRS_LIGHTING, FALSE);
                m_pd3dDevice->SetRenderState(D3DRS_ZENABLE, FALSE);
                m_pd3dDevice->SetRenderState(D3DRS_ALPHABLENDENABLE, TRUE);
                m_pd3dDevice->SetRenderState(D3DRS_SRCBLEND, D3DBLEND_ONE);
                m_pd3dDevice->SetRenderState(D3DRS_DESTBLEND, D3DBLEND_ONE);

                m_pd3dDevice->SetTextureStageState(0, D3DTSS_COLORARG1, D3DTA_TEXTURE);
                m_pd3dDevice->SetTextureStageState(0, D3DTSS_COLOROP, D3DTOP_SELECTARG1);

                m_pd3dDevice->SetTextureStageState(0, D3DTSS_ALPHAARG1, D3DTA_TEXTURE);
                m_pd3dDevice->SetTextureStageState(0, D3DTSS_ALPHAOP, D3DTOP_SELECTARG1);

                m_pd3dDevice->SetTextureStageState(1, D3DTSS_COLOROP, D3DTOP_DISABLE);
                m_pd3dDevice->SetTextureStageState(1, D3DTSS_ALPHAOP, D3DTOP_DISABLE);
                */

                foreach (EffectPass pass in effect.CurrentTechnique.Passes)
                {
                    pass.Apply();

                    muzzleFlare.Draw(graphicsDevice, camera.ViewMatrix, projectionMatrix);
                }
            }

            // render gun model
            Vector3 translation;

            if (Timer.IsRunning())
            {
                translation = new Vector3(0.0f, 0.0f, helper.RandomNumber(0.0f, -0.5f));
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
