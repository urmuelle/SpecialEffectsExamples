namespace ParticleSceneMG38
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Input;
    using SpecialEffectsExamplesLibrary;
    using SpecialEffectsExamplesLibrary.Graphics;
    using SpecialEffectsExamplesLibrary.ParticleSystem.AdvancedParticleSystem;

    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteFont font;

        private string[] particleSystemNames = { "BionicRGB", "BlinkingGreenAndPurpleLights",
                                                 "Blue", "BlueAndRed", "ColorfulSwirl", "DualSwirl",
                                                 "Faeries", "FireJet", "GreenAndPurplePulse", "LightningBugs",
                                                 "ParticleSystem", "PurdyColors", "QuadSwirl", "RandomBouncers",
                                                 "RGBCycleTest", "RGBFade", "RGBTest", "Snow", "Swirl",
                                                 "TripleSwirl" };

        private int currentState;

        private GroundPlane groundPlane;
        private ParticleEmitter particleSystem;

        // Input state.
        KeyboardState currentKeyboardState;
        KeyboardState lastKeyboardState;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
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

            font = Content.Load<SpriteFont>("font");

            groundPlane = new GroundPlane(graphics.GraphicsDevice, Content, "ch19p1_GroundTexture", 256.0f, 256.0f, 8);

            particleSystem = Content.Load<ParticleEmitter>("ParticleSystems/" + particleSystemNames[0]);
            currentState = 0;

            particleSystem.Start();
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
            HandleInput();

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                Exit();
            }

            particleSystem.Update(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// Handles input for quitting the game and cycling
        /// through the different particle effects.
        /// </summary>
        void HandleInput()
        {
            lastKeyboardState = currentKeyboardState;

            currentKeyboardState = Keyboard.GetState();

            // Check for exit.
            if (currentKeyboardState.IsKeyDown(Keys.Escape))
            {
                Exit();
            }

            // Check for changing the active particle effect.
            if (currentKeyboardState.IsKeyDown(Keys.Space) &&
                 lastKeyboardState.IsKeyUp(Keys.Space))
            {
                currentState++;

                if (currentState >= particleSystemNames.Length)
                {
                    currentState = 0;
                }

                particleSystem = Content.Load<ParticleEmitter>("ParticleSystems/" + particleSystemNames[currentState]);

                particleSystem.Start();
            }
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            Matrix view = Matrix.CreateLookAt(new Vector3(0, 2, -5), new Vector3(0, 1, 0), new Vector3(0, 1, 0));
            Matrix projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45), 800f / 480f, 0.01f, 100f);

            groundPlane.Draw(GraphicsDevice, view, projection);

            particleSystem.Draw(GraphicsDevice, gameTime, view, projection);

            DrawMessage();

            base.Draw(gameTime);
        }

        /// <summary>
        /// Helper for drawing our message text.
        /// </summary>
        void DrawMessage()
        {
            string message = string.Format("Current particle system: {0}\n" +
                                           "Hit space bar to switch.", particleSystemNames[currentState]);

            spriteBatch.Begin();
            spriteBatch.DrawString(font, message, new Vector2(10, 10), Color.White);
            spriteBatch.End();
        }
    }
}
