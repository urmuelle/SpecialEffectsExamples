namespace ParticleSceneMG38_1
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Input;
    using SpecialEffectsExamplesLibrary;
    using SpecialEffectsExamplesLibrary.Graphics;
    using SpecialEffectsExamplesLibrary.ParticleSystem.AdvancedParticleSystem;

    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private SpriteFont _font;

        private string[] _particleSystemNames = { "BionicRGB", "BlinkingGreenAndPurpleLights",
                                                 "Blue", "BlueAndRed", "ColorfulSwirl", "DualSwirl",
                                                 "Faeries", "FireJet", "GreenAndPurplePulse", "LightningBugs",
                                                 "ParticleSystem", "PurdyColors", "QuadSwirl", "RandomBouncers",
                                                 "RGBCycleTest", "RGBFade", "RGBTest", "Snow", "Swirl",
                                                 "TripleSwirl" };

        private int _currentState;

        private GroundPlane _groundPlane;
        private ParticleEmitter _particleSystem;

        // Input state.
        private KeyboardState _currentKeyboardState;
        private KeyboardState _lastKeyboardState;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _font = Content.Load<SpriteFont>("font");

            _groundPlane = new GroundPlane(_graphics.GraphicsDevice, Content, "ch19p1_GroundTexture", 256.0f, 256.0f, 8);

            _particleSystem = Content.Load<ParticleEmitter>("ParticleSystems/" + _particleSystemNames[0]);
            _currentState = 0;

            _particleSystem.Start();
        }

        protected override void Update(GameTime gameTime)
        {
            HandleInput();

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                Exit();
            }

            _particleSystem.Update(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// Handles input for quitting the game and cycling
        /// through the different particle effects.
        /// </summary>
        void HandleInput()
        {
            _lastKeyboardState = _currentKeyboardState;

            _currentKeyboardState = Keyboard.GetState();

            // Check for exit.
            if (_currentKeyboardState.IsKeyDown(Keys.Escape))
            {
                Exit();
            }

            // Check for changing the active particle effect.
            if (_currentKeyboardState.IsKeyDown(Keys.Space) &&
                 _lastKeyboardState.IsKeyUp(Keys.Space))
            {
                _currentState++;

                if (_currentState >= _particleSystemNames.Length)
                {
                    _currentState = 0;
                }

                _particleSystem = Content.Load<ParticleEmitter>("ParticleSystems/" + _particleSystemNames[_currentState]);

                _particleSystem.Start();
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            Matrix view = Matrix.CreateLookAt(new Vector3(0, 2, -5), new Vector3(0, 1, 0), new Vector3(0, 1, 0));
            Matrix projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45), 800f / 480f, 0.01f, 100f);

            _groundPlane.Draw(GraphicsDevice, view, projection);

            _particleSystem.Draw(GraphicsDevice, gameTime, view, projection);

            DrawMessage();

            base.Draw(gameTime);
        }

        /// <summary>
        /// Helper for drawing our message text.
        /// </summary>
        void DrawMessage()
        {
            string message = string.Format("Current particle system: {0}\n" +
                                           "Hit space bar to switch.", _particleSystemNames[_currentState]);

            _spriteBatch.Begin();
            _spriteBatch.DrawString(_font, message, new Vector2(10, 10), Color.White);
            _spriteBatch.End();
        }
    }
}
