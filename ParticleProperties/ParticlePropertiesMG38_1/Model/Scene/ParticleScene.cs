// <copyright file="ParticleScene.cs" company="Urs Müller">
// Copyright (c) Urs Müller. All rights reserved.
// </copyright>

namespace ParticlePropertiesMG38_1.Model.Scene
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using MonoGame.Framework.WpfInterop;
    using MonoGame.Framework.WpfInterop.Input;
    using SpecialEffectsExamplesLibrary;
    using SpecialEffectsExamplesLibrary.Graphics;

    public enum LocalBlendState
    {
        One,
        Zero,
        SourceColor,
        InverseSourceColor,
        SourceAlpha,
        InverseSourceAlpha,
        DestinationColor,
        InverseDestinationColor,
        DestinationAlpha,
        InverseDestinationAlpha,
        BlendFactor,
        InverseBlendFactor,
        SourceAlphaSaturation,
    }

    public class ParticleScene : WpfGame
    {
        private IGraphicsDeviceService graphicsDeviceManager;
        private WpfKeyboard keyboard;
        private WpfMouse mouse;

        private GroundPlane groundPlane;
        private ParticleEmitter particleSystem;
        private bool showGround;

        public bool ShowGround
        {
            get { return showGround; }
            set { showGround = value; }
        }

        public Vector3 SpawnDir1
        {
            get { return particleSystem.SpawnDir1; }
            set { particleSystem.SpawnDir1 = value; }
        }

        public Vector3 SpawnDir2
        {
            get { return particleSystem.SpawnDir2; }
            set { particleSystem.SpawnDir2 = value; }
        }

        public Vector3 Gravity
        {
            get { return particleSystem.Gravity; }
            set { particleSystem.Gravity = value; }
        }

        public Vector3 Position
        {
            get { return particleSystem.Position; }
            set { particleSystem.Position = value; }
        }

        public Vector3 EmissionRadius
        {
            get { return particleSystem.EmissionRadius; }
            set { particleSystem.EmissionRadius = value; }
        }

        public Blend SourceBlendMode
        {
            get { return particleSystem.SourceBlendMode; }
            set { particleSystem.SourceBlendMode = value; }
        }

        public Blend DestBlendMode
        {
            get { return particleSystem.DestBlendMode; }
            set { particleSystem.DestBlendMode = value; }
        }

        public int NumParticles
        {
            get { return particleSystem.NumParticles; }
            set { particleSystem.NumParticles = value; }
        }

        public Color StartColor1
        {
            get { return particleSystem.StartColor1; }
            set { particleSystem.StartColor1 = value; }
        }

        public Color StartColor2
        {
            get { return particleSystem.StartColor2; }
            set { particleSystem.StartColor2 = value; }
        }

        public Color EndColor1
        {
            get { return particleSystem.EndColor1; }
            set { particleSystem.EndColor1 = value; }
        }

        public Color EndColor2
        {
            get { return particleSystem.EndColor2; }
            set { particleSystem.EndColor2 = value; }
        }

        public float EmitRateMin
        {
            get { return particleSystem.EmitRateMin; }
            set { particleSystem.EmitRateMin = value; }
        }

        public float EmitRateMax
        {
            get { return particleSystem.EmitRateMax; }
            set { particleSystem.EmitRateMax = value; }
        }

        public float ParticleSizeMin
        {
            get { return particleSystem.ParticleSizeMin; }
            set { particleSystem.ParticleSizeMin = value; }
        }

        public float ParticleSizeMax
        {
            get { return particleSystem.ParticleSizeMax; }
            set { particleSystem.ParticleSizeMax = value; }
        }

        public float ParticleLifeTimeMin
        {
            get { return particleSystem.ParticleLifeTimeMin; }
            set { particleSystem.ParticleLifeTimeMin = value; }
        }

        public float ParticleLifeTimeMax
        {
            get { return particleSystem.ParticleLifeTimeMax; }
            set { particleSystem.ParticleLifeTimeMax = value; }
        }

        public string TextureFileName { get; internal set; }

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

            particleSystem = new ParticleEmitter(Content, graphicsDeviceManager.GraphicsDevice, "Ch18p1_ParticleTexture")
            {
                SpawnDir1 = new Vector3(-10.0f, -10.0f, -10.0f),
                SpawnDir2 = new Vector3(10.0f, 10.0f, 10.0f),
                StartColor1 = new Color(1.0f, 0.0f, 0.0f, 1.0f),
                StartColor2 = new Color(0.0f, 1.0f, 0.0f, 0.0f),
                Position = new Vector3(0.0f, 1.0f, 0.0f)
            };

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
