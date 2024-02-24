// <copyright file="MainWindow.xaml.cs" company="Urs Müller">
// Copyright (c) Urs Müller. All rights reserved.
// </copyright>

namespace ParticlePropertiesMG38_1
{
    using System.Windows;
    using ParticlePropertiesMG38_1.Model.Scene;
    using ParticlePropertiesMG38_1.ViewModel;

    /// <summary>
    /// Interaction logic for MainWindow.xaml.
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            var viewModel = new MainWindowViewModel();
            Loaded += (s, e) => { DataContext = viewModel; };
            viewModel.SetDefaultValues();

            if (viewModel.ShowGroundAction == null)
            {
                viewModel.ShowGroundAction = new Action(() =>
                {
                    ParticleScene.ShowGround = viewModel.ShowGround;
                });
            }

            if (viewModel.SpawnDir1Action == null)
            {
                viewModel.SpawnDir1Action = new Action(() =>
                {
                    ParticleScene.SpawnDir1 = new Microsoft.Xna.Framework.Vector3(
                        viewModel.SpawnDir1X,
                        viewModel.SpawnDir1Y,
                        viewModel.SpawnDir1Z);
                });
            }

            if (viewModel.SpawnDir2Action == null)
            {
                viewModel.SpawnDir2Action = new Action(() =>
                {
                    ParticleScene.SpawnDir2 = new Microsoft.Xna.Framework.Vector3(
                        viewModel.SpawnDir2X,
                        viewModel.SpawnDir2Y,
                        viewModel.SpawnDir2Z);
                });
            }

            if (viewModel.GravityAction == null)
            {
                viewModel.GravityAction = new Action(() =>
                {
                    ParticleScene.Gravity = new Microsoft.Xna.Framework.Vector3(
                        viewModel.GravityX,
                        viewModel.GravityY,
                        viewModel.GravityZ);
                });
            }

            if (viewModel.PositionAction == null)
            {
                viewModel.PositionAction = new Action(() =>
                {
                    ParticleScene.Position = new Microsoft.Xna.Framework.Vector3(
                        viewModel.PositionX,
                        viewModel.PositionY,
                        viewModel.PositionZ);
                });
            }

            if (viewModel.EmissionRadiusAction == null)
            {
                viewModel.EmissionRadiusAction = new Action(() =>
                {
                    ParticleScene.Position = new Microsoft.Xna.Framework.Vector3(
                        viewModel.EmissionRadiusX,
                        viewModel.EmissionRadiusY,
                        viewModel.EmissionRadiusZ);
                });
            }

            if (viewModel.SourceBlendAction == null)
            {
                viewModel.SourceBlendAction = new Action(() =>
                {
                    ParticleScene.SourceBlendMode = (Microsoft.Xna.Framework.Graphics.Blend)viewModel.SourceBlendMode;
                });
            }

            if (viewModel.DestBlendAction == null)
            {
                viewModel.DestBlendAction = new Action(() =>
                {
                    ParticleScene.DestBlendMode = (Microsoft.Xna.Framework.Graphics.Blend)viewModel.DestBlendMode;
                });
            }

            if (viewModel.MaxNumberOfParticlesAction == null)
            {
                viewModel.MaxNumberOfParticlesAction = new Action(() =>
                {
                    ParticleScene.NumParticles = viewModel.NumParticles;
                });
            }

            if (viewModel.StartColor1Action == null)
            {
                viewModel.StartColor1Action = new Action(() =>
                {
                    ParticleScene.StartColor1 = new Microsoft.Xna.Framework.Color(
                        viewModel.StartColor1.R,
                        viewModel.StartColor1.G,
                        viewModel.StartColor1.B,
                        viewModel.StartColor1.A);
                });
            }

            if (viewModel.StartColor2Action == null)
            {
                viewModel.StartColor2Action = new Action(() =>
                {
                    ParticleScene.StartColor2 = new Microsoft.Xna.Framework.Color(
                        viewModel.StartColor2.R,
                        viewModel.StartColor2.G,
                        viewModel.StartColor2.B,
                        viewModel.StartColor2.A);
                });
            }

            if (viewModel.EndColor1Action == null)
            {
                viewModel.EndColor1Action = new Action(() =>
                {
                    ParticleScene.EndColor1 = new Microsoft.Xna.Framework.Color(
                        viewModel.EndColor1.R,
                        viewModel.EndColor1.G,
                        viewModel.EndColor1.B,
                        viewModel.EndColor1.A);
                });
            }

            if (viewModel.EndColor2Action == null)
            {
                viewModel.EndColor2Action = new Action(() =>
                {
                    ParticleScene.EndColor2 = new Microsoft.Xna.Framework.Color(
                        viewModel.EndColor2.R,
                        viewModel.EndColor2.G,
                        viewModel.EndColor2.B,
                        viewModel.EndColor2.A);
                });
            }

            if (viewModel.EmitRateMinAction == null)
            {
                viewModel.EmitRateMinAction = new Action(() =>
                {
                    ParticleScene.EmitRateMin = viewModel.EmitRateMin;
                });
            }

            if (viewModel.EmitRateMaxAction == null)
            {
                viewModel.EmitRateMaxAction = new Action(() =>
                {
                    ParticleScene.EmitRateMax = viewModel.EmitRateMax;
                });
            }

            if (viewModel.ParticleSizeMinAction == null)
            {
                viewModel.ParticleSizeMinAction = new Action(() =>
                {
                    ParticleScene.ParticleSizeMin = (int)viewModel.ParticleSizeMin;
                });
            }

            if (viewModel.ParticleSizeMaxAction == null)
            {
                viewModel.ParticleSizeMaxAction = new Action(() =>
                {
                    ParticleScene.ParticleSizeMax = (int)viewModel.ParticleSizeMax;
                });
            }

            if (viewModel.ParticleLifeTimeMinAction == null)
            {
                viewModel.ParticleLifeTimeMinAction = new Action(() =>
                {
                    ParticleScene.ParticleLifeTimeMin = (int)viewModel.ParticleLifeTimeMin;
                });
            }

            if (viewModel.ParticleLifeTimeMaxAction == null)
            {
                viewModel.ParticleLifeTimeMaxAction = new Action(() =>
                {
                    ParticleScene.ParticleLifeTimeMax = (int)viewModel.ParticleLifeTimeMax;
                });
            }

            if (viewModel.TextureFileNameAction == null)
            {
                viewModel.TextureFileNameAction = new Action(() =>
                {
                    ParticleScene.TextureFileName = viewModel.TextureFileName;
                });
            }
        }
    }
}
