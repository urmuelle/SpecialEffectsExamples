// <copyright file="ParticleScene.cs" company="Urs Müller">
// </copyright>

namespace AdvancedParticleSystem.Scene
{
    using System;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using MonoGame.Framework.WpfInterop;
    using MonoGame.Framework.WpfInterop.Input;
    using ParticleSystem;
    using SpecialEffectsExamplesLibrary;
    using SpecialEffectsExamplesLibrary.Graphics;

    public class ParticleScene : WpfGame
    {
        private IGraphicsDeviceService graphicsDeviceManager;
        private WpfKeyboard keyboard;
        private WpfMouse mouse;

        private GroundPlane groundPlane;
        private ParticleEmitter particleSystem;
        private bool showGround;
        private string particleSystemDescription;

        public bool ShowGround
        {
            get { return showGround; }
            set { showGround = value; }
        }

        public int NumParticles
        {
            get { return particleSystem.NumParticles; }
            set { particleSystem.NumParticles = value; }
        }

        public string TextureFileName { get; internal set; }

        public string ParticleSystemDescription
        {
            get
            {
                return particleSystemDescription;
            }

            set
            {
                this.particleSystemDescription = value;
                Compile();
            }
        }

        public void Compile()
        {
            particleSystem.Stop();
            particleSystem.Compile(particleSystemDescription);
            particleSystem.LoadContent(Content, graphicsDeviceManager.GraphicsDevice);
            particleSystem.Start();
        }

        protected override void Initialize()
        {
            // must be initialized. required by Content loading and rendering (will add itself to the Services)
            graphicsDeviceManager = new WpfGraphicsDeviceService(this);

            // wpf and keyboard need reference to the host control in order to receive input
            // this means every WpfGame control will have it's own keyboard & mouse manager which will only react if the mouse is in the control
            keyboard = new WpfKeyboard(this);
            mouse = new WpfMouse(this);
            showGround = false;

            // must be called after the WpfGraphicsDeviceService instance was created
            base.Initialize();

            // content loading now possible
        }

        protected override void LoadContent()
        {
            groundPlane = new GroundPlane(graphicsDeviceManager.GraphicsDevice, Content, "ch18p1_GroundTexture", 256.0f, 256.0f, 8);

            ShowGround = true;

            particleSystem = new ParticleEmitter(Content, graphicsDeviceManager.GraphicsDevice)
            {
                Position = new Vector3(0.0f, 1.0f, 0.0f)
            };

            particleSystem.Compile(particleSystemDescription);
            particleSystem.LoadContent(Content, graphicsDeviceManager.GraphicsDevice);
            particleSystem.Start();

            base.LoadContent();
        }

        protected override void Update(GameTime time)
        {
            // every update we can now query the keyboard & mouse for our WpfGame
            var mouseState = mouse.GetState();
            var keyboardState = keyboard.GetState();

            particleSystem.Update(time);
        }

        protected override void Draw(GameTime time)
        {
            graphicsDeviceManager.GraphicsDevice.Clear(Color.Black);

            Matrix view = Matrix.CreateLookAt(new Vector3(0, 2, -5), new Vector3(0, 1, 0), new Vector3(0, 1, 0));
            Matrix projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45), 800f / 480f, 0.01f, 100f);

            if (showGround)
            {
                groundPlane.Draw(graphicsDeviceManager.GraphicsDevice, view, projection);
            }

            particleSystem.Draw(time, view, projection, graphicsDeviceManager.GraphicsDevice);
        }
    }
}