// <copyright file="PlasmaBulletArray.cs" company="Urs Müller">
// </copyright>

namespace FiringRangeMG38
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Content;
    using Microsoft.Xna.Framework.Graphics;
    using SpecialEffectsExamplesLibrary;
    using SpecialEffectsExamplesLibrary.Animation;
    using SpecialEffectsExamplesLibrary.Camera;

    public class PlasmaBulletArray
    {
        private static int NumBullets = 100;
        private RecyclingArray<PlasmaBullet> bullets;
        private AnimationSequence animation;
        private VertexBuffer vertexBuffer;
        private SpecialEffectsExamplesLibrary.Helper helper;

        public PlasmaBulletArray()
        {
            helper = new Helper();
        }

        public void LoadContent(ContentManager content, GraphicsDevice graphicsDevice)
        {
            bullets = new RecyclingArray<PlasmaBullet>(NumBullets);
            vertexBuffer = new DynamicVertexBuffer(graphicsDevice, VertexPositionColorTexture.VertexDeclaration, NumBullets * 6, BufferUsage.WriteOnly);

            animation = new AnimationSequence(graphicsDevice);
            animation.AddFrame(content.Load<Texture2D>("Ch21p1_PlasmaBullet_01"), 20.0f);
        }

        public void UpdateAll(GameTime gameTime)
        {
            var elapsedTime = gameTime.ElapsedGameTime.Milliseconds / 1000.0f;

            for (int q = 0; q < NumBullets; q++)
            {
                if (bullets.IsAlive(q))
                {
                    PlasmaBullet bullet = bullets.GetAt(q);
                    bullet.Position += bullet.Velocity * elapsedTime;

                    if (bullet.Sprite.Timer.IsRunning() &&
                      bullet.Sprite.Timer.Time / 1000.0f > 5.0f)
                    {
                        bullets.Delete(q);
                    }
                }
            }
        }

        public void RenderAll(GraphicsDevice graphicsDevice, Matrix worldMatrix, Matrix projectionMatrix, Camera camera)
        {
            // Set other Render States
            RasterizerState rasterizerState = new RasterizerState()
            {
                CullMode = CullMode.None,
            };

            graphicsDevice.RasterizerState = rasterizerState;
            graphicsDevice.DepthStencilState = DepthStencilState.Default;
            graphicsDevice.BlendState = BlendState.AlphaBlend;
            graphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;

            // render all the bullets via one call to DrawPrimitive
            for (int q = 0; q < NumBullets; q++)
            {
                if (bullets.IsAlive(q))
                {
                    PlasmaBullet bullet = bullets.GetAt(q);

                    bullet.Sprite.Position = bullet.Position;
                    bullet.Sprite.Draw(graphicsDevice, camera.ViewMatrix, projectionMatrix);
                }
            }
        }

        public void AddBullet(Vector3 position, Vector3 velocity)
        {
            PlasmaBullet newbullet = bullets.New();

            newbullet.Position = position;
            newbullet.Velocity = velocity;

            newbullet.Sprite.AnimationSequemce = animation;
            newbullet.Sprite.Timer.Begin();
            newbullet.Sprite.Size = helper.RandomNumber(5.0f, 10.0f);
        }
    }
}
