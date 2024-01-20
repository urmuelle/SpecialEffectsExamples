// <copyright file="FiringRange.cs" company="Urs Müller">
// Copyright (c) Urs Müller. All rights reserved.
// </copyright>

namespace FiringRangeMG38_1
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Input;
    using SpecialEffectsExamplesLibrary;
    using SpecialEffectsExamplesLibrary.Camera;
    using SpecialEffectsExamplesLibrary.Graphics;

    public class FiringRange : Game
    {
        private GraphicsDeviceManager graphics;

        // Scene
        private GroundPlane groundPlane;
        private Camera camera;
        private MouseState originalMouseState;

        // skybox
        private SkyBox skyBox;

        private Timer transitionTimer;

        // da weaponery
        private PlasmaGun plasmaGun;
        private PlasmaBulletArray plasmaBulletArray;

        private LaserGun laserGun;
        private Gun curGun;
        private Gun nextGun;

        private MachineGun machineGun;

        public FiringRange()
        {
            graphics = new GraphicsDeviceManager(this)
            {
                PreferredBackBufferWidth = 400,
                PreferredBackBufferHeight = 300,
            };

            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            skyBox = new SkyBox(
                GraphicsDevice,
                Content,
                "Ch21p1_DesertSkyboxTop",
                "Ch21p1_DesertSkyboxBottom",
                "Ch21p1_DesertSkyboxFront",
                "Ch21p1_DesertSkyboxBack",
                "Ch21p1_DesertSkyboxLeft",
                "Ch21p1_DesertSkyboxRight")
            {
                Size = 100.0f,
            };

            groundPlane = new GroundPlane(graphics.GraphicsDevice, Content, "ch21p1_GroundTexture", 256.0f, 256.0f, 8);

            plasmaGun = new PlasmaGun();
            plasmaGun.LoadContent(Content, GraphicsDevice, "Ch21p1_Gun");

            plasmaBulletArray = new PlasmaBulletArray();
            plasmaBulletArray.LoadContent(Content, GraphicsDevice);
            plasmaGun.BulletArray = plasmaBulletArray;

            laserGun = new LaserGun();
            laserGun.LoadContent(Content, GraphicsDevice, "Ch21p1_Gun");

            machineGun = new MachineGun();
            machineGun.LoadContent(Content, GraphicsDevice, "Ch21p1_Gun");

            plasmaGun.Position = new Vector3(0.5f, -0.6f, -1.0f);
            laserGun.Position = new Vector3(0.5f, -0.6f, -1.0f);
            machineGun.Position = new Vector3(0.5f, -0.6f, -1.0f);

            curGun = machineGun;

            Mouse.SetPosition(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2);
            originalMouseState = Mouse.GetState();

            camera = new UserControlledCamera
            {
                Position = new Vector3(0.0f, 7.0f, -20.0f),
            };

            transitionTimer = new Timer();
        }

        protected override void Update(GameTime gameTime)
        {
            ProcessInput();

            Timer.UpdateAll(gameTime.ElapsedGameTime.Milliseconds);

            camera.Update(gameTime.ElapsedGameTime.Milliseconds / 1000);

            plasmaBulletArray.UpdateAll(gameTime);

            curGun.Update(gameTime);

            if (transitionTimer.IsRunning())
            {
                if (curGun != null)
                {
                    if (transitionTimer.Time / 1000 < 0.25f)
                    {
                        curGun.Position = new Vector3(0.5f, -0.6f - ((transitionTimer.Time / 1000) * 4), -1.0f);
                    }
                    else
                    {
                        curGun = nextGun;
                        curGun.Position = new Vector3(0.5f, -1.6f + ((transitionTimer.Time / 1000) * 4) - 1.0f, -1.0f);
                    }

                    if (transitionTimer.Time / 1000 > 0.5f)
                    {
                        transitionTimer.Stop();
                        curGun.Position = new Vector3(0.5f, -0.6f, -1.0f);
                        nextGun = null;
                    }
                }
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            GraphicsDevice.DepthStencilState = DepthStencilState.Default;

            Matrix projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45), 400f / 300f, 0.01f, 100f);
            Matrix world = Matrix.Identity;
            Matrix view = camera.ViewMatrix;

            GraphicsDevice.BlendState = BlendState.Opaque;
            GraphicsDevice.DepthStencilState = DepthStencilState.None;
            GraphicsDevice.SamplerStates[1] = SamplerState.PointClamp;

            // draw skybox
            skyBox.Draw(GraphicsDevice, world, view, projection);

            // draw groundplane
            groundPlane.Draw(GraphicsDevice, view, projection);

            // draw gun
            curGun.Draw(GraphicsDevice, camera, projection);

            // draw plasma bullets
            plasmaBulletArray.RenderAll(GraphicsDevice, world, projection, camera);

            base.Draw(gameTime);
        }

        private void ProcessInput()
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                Exit();
            }

            const float speed = 0.5f;

            if (camera is UserControlledCamera)
            {
                // Process keyboard input
                if (Keyboard.GetState().IsKeyDown(Keys.D))
                {
                    ((UserControlledCamera)camera).AddToVelocity(new Vector3(speed, 0.0f, 0.0f)); // Slide Right
                }

                if (Keyboard.GetState().IsKeyDown(Keys.A))
                {
                    ((UserControlledCamera)camera).AddToVelocity(new Vector3(-speed, 0.0f, 0.0f)); // Slide Left
                }

                if (Keyboard.GetState().IsKeyDown(Keys.Q))
                {
                    ((UserControlledCamera)camera).AddToVelocity(new Vector3(0.0f, speed, 0.0f)); // Slide Up
                }

                if (Keyboard.GetState().IsKeyDown(Keys.Z))
                {
                    ((UserControlledCamera)camera).AddToVelocity(new Vector3(0.0f, -speed, 0.0f)); // Slide Down
                }

                if (Keyboard.GetState().IsKeyDown(Keys.W))
                {
                    ((UserControlledCamera)camera).AddToVelocity(new Vector3(0.0f, 0.0f, -speed)); // Slide Foward
                }

                if (Keyboard.GetState().IsKeyDown(Keys.S))
                {
                    ((UserControlledCamera)camera).AddToVelocity(new Vector3(0.0f, 0.0f, speed)); // Slide Back
                }

                if (Keyboard.GetState().IsKeyDown(Keys.L))
                {
                    ((UserControlledCamera)camera).AddToYawPitchRoll(speed, 0.0f, 0.0f); // Turn Right
                }

                if (Keyboard.GetState().IsKeyDown(Keys.J))
                {
                    ((UserControlledCamera)camera).AddToYawPitchRoll(-speed, 0.0f, 0.0f); // Turn Left
                }

                if (Keyboard.GetState().IsKeyDown(Keys.K))
                {
                    ((UserControlledCamera)camera).AddToYawPitchRoll(0.0f, speed, 0.0f); // Turn Down
                }

                if (Keyboard.GetState().IsKeyDown(Keys.I))
                {
                    ((UserControlledCamera)camera).AddToYawPitchRoll(0.0f, -speed, 0.0f); // Turn Up
                }

                if (Keyboard.GetState().IsKeyDown(Keys.D1))
                {
                    BeginGunTransition(machineGun);
                }

                if (Keyboard.GetState().IsKeyDown(Keys.D2))
                {
                    BeginGunTransition(plasmaGun);
                }

                if (Keyboard.GetState().IsKeyDown(Keys.D3))
                {
                    BeginGunTransition(laserGun);
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
                ((UserControlledCamera)camera).AddToYawPitchRoll(-xDifference / 3.8f, -yDifference / 3.8f, 0.0f);

                // prevent camera from moving up or down.
                camera.Position = new Vector3(camera.Position.X, 2.60f, camera.Position.Z);

                // Do Firing
                if (dims2.LeftButton == ButtonState.Pressed)
                {
                    if ((curGun != null) && curGun.CanFire())
                    {
                        curGun.Fire(camera);
                    }
                }
            }
        }

        private void BeginGunTransition(Gun nextGun)
        {
            if (nextGun.GetType() == curGun.GetType())
            {
                return;
            }

            if (transitionTimer.IsRunning())
            {
                return;
            }

            this.nextGun = nextGun;
            transitionTimer.Begin();
        }
    }
}
