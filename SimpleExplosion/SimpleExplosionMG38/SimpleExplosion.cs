// <copyright file="SimpleExplosion.cs" company="Urs Müller">
// </copyright>

namespace SimpleExplosionMG38
{
    using System;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Input;
    using SpecialEffectsExamplesLibrary;
    using SpecialEffectsExamplesLibrary.Animation;
    using SpecialEffectsExamplesLibrary.Camera;
    using SpecialEffectsExamplesLibrary.Graphics;

    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class SimpleExplosion : Game
    {
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
        private Sprite explosionSprite;

        private SpecialEffectsExamplesLibrary.Helper helper;

        /// <summary>
        /// Initializes a new instance of the <see cref="SimpleExplosion"/> class.
        /// </summary>
        public SimpleExplosion()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            helper = new SpecialEffectsExamplesLibrary.Helper();
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            font = Content.Load<SpriteFont>("gamefont");
            fontSmall = Content.Load<SpriteFont>("gamefontSmall");

            groundPlane = new GroundPlane(graphics.GraphicsDevice, Content, "ch20p1_GroundTexture", 256.0f, 256.0f, 8);
            explosionObject = Content.Load<Model>("Ch20p1_Object");
            objectRebirthTimer = new Timer();

            explosionAnimation = new AnimationSequence(GraphicsDevice);
            explosionAnimation.AddFrame(Content.Load<Texture2D>("Ch20p1_Explosion_Frame01"), 0.03f);
            explosionAnimation.AddFrame(Content.Load<Texture2D>("Ch20p1_Explosion_Frame02"), 0.03f);
            explosionAnimation.AddFrame(Content.Load<Texture2D>("Ch20p1_Explosion_Frame03"), 0.03f);
            explosionAnimation.AddFrame(Content.Load<Texture2D>("Ch20p1_Explosion_Frame04"), 0.03f);
            explosionAnimation.AddFrame(Content.Load<Texture2D>("Ch20p1_Explosion_Frame05"), 0.03f);
            explosionAnimation.AddFrame(Content.Load<Texture2D>("Ch20p1_Explosion_Frame06"), 0.03f);
            explosionAnimation.AddFrame(Content.Load<Texture2D>("Ch20p1_Explosion_Frame07"), 0.03f);
            explosionAnimation.AddFrame(Content.Load<Texture2D>("Ch20p1_Explosion_Frame08"), 0.03f);

            explosionSprite = new Sprite
            {
                AnimationSequemce = explosionAnimation,
                Size = 15.0f,
            };

            Mouse.SetPosition(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2);
            originalMouseState = Mouse.GetState();

            camera = new UserControlledCamera
            {
                Position = new Vector3(0.0f, 4.0f, 15.0f),
            };
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            ProcessInput(gameTime);

            Timer.UpdateAll(gameTime.ElapsedGameTime.Milliseconds);

            if (objectRebirthTimer.Time / 1000 > 2.0f)
            {
                objectRebirthTimer.Stop();
            }

            camera.Update(gameTime.ElapsedGameTime.Milliseconds / 1000);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            Matrix projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45), 800f / 480f, 0.01f, 100f);
            Matrix world = Matrix.Identity;

            Matrix view = camera.ViewMatrix;

            groundPlane.Draw(GraphicsDevice, view, projection);

            world = Matrix.CreateTranslation(0, 4, 0);
            if (!objectRebirthTimer.IsRunning())
            {
                explosionObject.Draw(world, view, projection);
            }

            // Draw explosion
            if (objectRebirthTimer.IsRunning())
            {
                explosionSprite.Draw(GraphicsDevice, view, projection);
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
                if (!objectRebirthTimer.IsRunning())
                {
                    CreateExplosion(
                        ref explosionSprite,
                        0.0f, 4.0f, 0.0f, // position
                        0.0f, 0.0f, 0.0f, // radius
                        15.0f, 5.0f,      // size
                        0.0f, 0.0f);      // time
                    objectRebirthTimer.Begin();
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
