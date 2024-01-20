// <copyright file="ComplexExplosion.cs" company="Urs Müller">
// Copyright (c) Urs Müller. All rights reserved.
// </copyright>

namespace ComplexExplosionMG38_1
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Input;
    using SpecialEffectsExamplesLibrary;
    using SpecialEffectsExamplesLibrary.Animation;
    using SpecialEffectsExamplesLibrary.Camera;
    using SpecialEffectsExamplesLibrary.Graphics;
    using SpecialEffectsExamplesLibrary.ParticleSystem.AdvancedParticleSystem;

    public class ComplexExplosion : Game
    {
        private const int NumSprites = 5;

        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        // Fonts
        private SpriteFont font;
        private SpriteFont fontSmall;

        // Scene
        private GroundPlane groundPlane;
        private UserControlledCamera camera;
        private MouseState originalMouseState;

        // Explosion Object
        private Model explosionObject;
        private Timer objectRebirthTimer;

        // Explosion Animation and Sprite
        private AnimationSequence explosionAnimation;
        private Sprite[] explosionSprite = new Sprite[NumSprites];
        private ParticleEmitter particleEmitter;
        private Shockwave shockwave;

        // skybox
        private SkyBox skyBox;

        private SpecialEffectsExamplesLibrary.Helper helper;

        public ComplexExplosion()
        {
            graphics = new GraphicsDeviceManager(this)
            {
                PreferredBackBufferWidth = 640,
                PreferredBackBufferHeight = 480,
            };

            Content.RootDirectory = "Content";
            helper = new SpecialEffectsExamplesLibrary.Helper();
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            font = Content.Load<SpriteFont>("gamefont");
            fontSmall = Content.Load<SpriteFont>("gamefontSmall");

            groundPlane = new GroundPlane(graphics.GraphicsDevice, Content, "ch20p3_GroundTexture", 256.0f, 256.0f, 8);
            explosionObject = Content.Load<Model>("Ch20p3_Object");
            objectRebirthTimer = new Timer();

            explosionAnimation = new AnimationSequence(GraphicsDevice);
            explosionAnimation.AddFrame(Content.Load<Texture2D>("Ch20p3_Explosion_Frame01"), 0.03f);
            explosionAnimation.AddFrame(Content.Load<Texture2D>("Ch20p3_Explosion_Frame02"), 0.03f);
            explosionAnimation.AddFrame(Content.Load<Texture2D>("Ch20p3_Explosion_Frame03"), 0.03f);
            explosionAnimation.AddFrame(Content.Load<Texture2D>("Ch20p3_Explosion_Frame04"), 0.03f);
            explosionAnimation.AddFrame(Content.Load<Texture2D>("Ch20p3_Explosion_Frame05"), 0.03f);
            explosionAnimation.AddFrame(Content.Load<Texture2D>("Ch20p3_Explosion_Frame06"), 0.03f);
            explosionAnimation.AddFrame(Content.Load<Texture2D>("Ch20p3_Explosion_Frame07"), 0.03f);
            explosionAnimation.AddFrame(Content.Load<Texture2D>("Ch20p3_Explosion_Frame08"), 0.03f);

            for (int q = 0; q < NumSprites; q++)
            {
                explosionSprite[q] = new Sprite
                {
                    AnimationSequemce = explosionAnimation,
                    Size = 15.0f,
                };
            }

            skyBox = new SkyBox(
                GraphicsDevice,
                Content,
                "Ch20p3_SkyBox_Top",
                "Ch20p3_SkyBox_Bottom",
                "Ch20p3_SkyBox_Front",
                "Ch20p3_SkyBox_Back",
                "Ch20p3_SkyBox_Left",
                "Ch20p3_SkyBox_Right")
            {
                Size = 100.0f,
            };

            particleEmitter = Content.Load<ParticleEmitter>("Ch20p3_ParticleScript");

            // vary shockwave thickness, speed, and lifetime to taste
            shockwave = new Shockwave(GraphicsDevice, Content, "Ch20p3_Shockwave", 10.0f, 4.0f, 16, 6.0f, 1.0f)
            {
                Position = new Vector3(0.0f, 4.0f, 0.0f),
            };

            Mouse.SetPosition(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2);
            originalMouseState = Mouse.GetState();

            camera = new UserControlledCamera
            {
                Position = new Vector3(0.0f, 7.0f, 20.0f),
            };
        }

        protected override void Update(GameTime gameTime)
        {
            ProcessInput(gameTime);

            Timer.UpdateAll(gameTime.ElapsedGameTime.Milliseconds);

            particleEmitter.Update(gameTime);
            shockwave.Update(gameTime);

            if (objectRebirthTimer.Time / 1000 > 2.0f)
            {
                objectRebirthTimer.Stop();
            }

            camera.Update(gameTime.ElapsedGameTime.Milliseconds / 1000);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            GraphicsDevice.DepthStencilState = DepthStencilState.Default;

            Matrix projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45), 800f / 480f, 0.01f, 100f);
            Matrix world = Matrix.Identity;

            Matrix view = camera.ViewMatrix;

            GraphicsDevice.BlendState = BlendState.Opaque;
            GraphicsDevice.DepthStencilState = DepthStencilState.None;
            GraphicsDevice.SamplerStates[1] = SamplerState.PointClamp;

            // draw skybox
            skyBox.Draw(GraphicsDevice, world, view, projection);

            // Draw explosion
            if (objectRebirthTimer.IsRunning())
            {
                for (int q = 0; q < NumSprites; q++)
                {
                    explosionSprite[q].Draw(GraphicsDevice, view, projection);
                }

                shockwave.Draw(GraphicsDevice, view, projection);
                particleEmitter.Draw(GraphicsDevice, gameTime, view, projection);
            }

            // Write information
            spriteBatch.Begin();
            spriteBatch.DrawString(fontSmall, "Position: (" + camera.Position.X + "," + camera.Position.Y + "," + camera.Position.Z + ")", new Vector2(0, 0), Color.White);

            if (objectRebirthTimer.IsRunning())
            {
                spriteBatch.DrawString(fontSmall, "Kablammo!", new Vector2(0, 20), Color.Yellow);
            }
            else
            {
                spriteBatch.DrawString(fontSmall, "Press B to blow up this poor object.", new Vector2(0, 20), Color.Yellow);
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }

        private void ProcessInput(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                Exit();
            }

            if (Keyboard.GetState().IsKeyDown(Keys.B))
            {
                // one big explosion in the middle
                if (!objectRebirthTimer.IsRunning())
                {
                    // one big explosion in the middle
                    CreateExplosion(
                      ref explosionSprite[0],
                      0.0f, 4.0f, 0.0f, // position
                      0.0f, 0.0f, 0.0f, // radius
                      15.0f, 5.0f,      // size
                      0.0f, 0.0f);      // time

                    // several smaller explosions around it
                    for (int q = 1; q < NumSprites; q++)
                    {
                        CreateExplosion(
                          ref explosionSprite[q],
                          0.0f, 4.0f, 0.0f, // position
                          1.0f, 1.0f, 1.0f, // radius
                          10.0f, 5.0f,      // size
                          0.0f, 0.25f);     // time
                    }

                    objectRebirthTimer.Begin();

                    // start particle system
                    particleEmitter.Position = new Vector3(0.0f, 4.0f, 0.0f);
                    particleEmitter.Stop();
                    particleEmitter.Start();
                    shockwave.Stop();
                    shockwave.Begin();
                }
            }

            const float speed = 0.5f;

            // Process keyboard input
            if (Keyboard.GetState().IsKeyDown(Keys.D))
            {
                camera.AddToVelocity(new Vector3(speed, 0.0f, 0.0f)); // Slide Right
            }

            if (Keyboard.GetState().IsKeyDown(Keys.A))
            {
                camera.AddToVelocity(new Vector3(-speed, 0.0f, 0.0f)); // Slide Left
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Q))
            {
                camera.AddToVelocity(new Vector3(0.0f, speed, 0.0f)); // Slide Up
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Z))
            {
                camera.AddToVelocity(new Vector3(0.0f, -speed, 0.0f)); // Slide Down
            }

            if (Keyboard.GetState().IsKeyDown(Keys.W))
            {
                camera.AddToVelocity(new Vector3(0.0f, 0.0f, -speed)); // Slide Foward
            }

            if (Keyboard.GetState().IsKeyDown(Keys.S))
            {
                camera.AddToVelocity(new Vector3(0.0f, 0.0f, speed)); // Slide Back
            }

            if (Keyboard.GetState().IsKeyDown(Keys.L))
            {
                camera.AddToYawPitchRoll(speed, 0.0f, 0.0f); // Turn Right
            }

            if (Keyboard.GetState().IsKeyDown(Keys.J))
            {
                camera.AddToYawPitchRoll(-speed, 0.0f, 0.0f); // Turn Left
            }

            if (Keyboard.GetState().IsKeyDown(Keys.K))
            {
                camera.AddToYawPitchRoll(0.0f, speed, 0.0f); // Turn Down
            }

            if (Keyboard.GetState().IsKeyDown(Keys.I))
            {
                camera.AddToYawPitchRoll(0.0f, -speed, 0.0f); // Turn Up
            }

            MouseState currentMouseState = Mouse.GetState();

            float xDifference = 0.0f;
            float yDifference = 0.0f;

            if (currentMouseState != originalMouseState)
            {
                xDifference = currentMouseState.X - originalMouseState.X;
                yDifference = currentMouseState.Y - originalMouseState.Y;
                Mouse.SetPosition(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2);
            }

            // mouse look
            MouseState dims2 = Mouse.GetState();

            // play with the divisor constants to change the mouselook sensitivity.
            // I've found that these values most accurately simulate my beloved Q3A setup. :)
            camera.AddToYawPitchRoll(-xDifference / 3.8f, -yDifference / 3.8f, 0.0f);

            if (camera.Position.Y < 1.0f)
            {
                camera.Position = new Vector3(camera.Position.X, 1.0f, camera.Position.Z);
            }
        }

        private void CreateExplosion(
            ref Sprite sprite,
            float posX, float posY, float posZ,
            float radiusX, float radiusY, float radiusZ,
            float size, float sizeDelta,
            float time, float timeDelta)
        {
            sprite.Position = new Vector3(posX, posY, posZ) + helper.RandomNumber(
                new Vector3(-radiusX, -radiusY, -radiusZ),
                new Vector3(radiusX, radiusY, radiusZ));

            sprite.Size = size + helper.RandomNumber(-sizeDelta, sizeDelta);

            sprite.Timer.BeginWithDelay(time + helper.RandomNumber(-timeDelta, timeDelta));
        }
    }
}