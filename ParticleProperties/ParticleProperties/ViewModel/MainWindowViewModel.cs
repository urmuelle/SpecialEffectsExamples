// <copyright file="MainWindowViewModel.cs" company="Urs Müller">
// Copyright (c) Urs Müller. All rights reserved.
// </copyright>

namespace ParticleProperties.ViewModel
{
    using System;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Input;
    using System.Xml.Linq;
    using Microsoft.Win32;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using ParticleProperties.Model.Scene;

    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private bool showGround;
        private Vector3 spawnDir1;
        private Vector3 spawnDir2;
        private Vector3 gravity;
        private Vector3 position;
        private Vector3 emissionRadius;
        private Blend sourceBlend;
        private Blend destBlend;
        private int maxNumberOfParticles;
        private System.Windows.Media.Color startColor1;
        private System.Windows.Media.Color startColor2;
        private System.Windows.Media.Color endColor1;
        private System.Windows.Media.Color endColor2;
        private float emitRateMin;
        private float emitRateMax;
        private float particleSizeMin;
        private float particleSizeMax;
        private float particleLifeTimeMin;
        private float particleLifeTimeMax;
        private ObservableCollection<string> textureFileNames = new ObservableCollection<string>();
        private string textureFileName;

        private ICommand saveCommand;
        private ICommand loadCommand;
        private ICommand resetCommand;

        public MainWindowViewModel()
        {
            saveCommand = new RelayCommand(SaveFile);
            loadCommand = new RelayCommand(LoadFile);
            resetCommand = new RelayCommand(SetDefaultValues);
            showGround = true;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public Action ShowGroundAction { get; set; }

        public Action GravityAction { get; set; }

        public Action SpawnDir1Action { get; set; }

        public Action SpawnDir2Action { get; set; }

        public Action PositionAction { get; set; }

        public Action EmissionRadiusAction { get; set; }

        public Action SourceBlendAction { get; set; }

        public Action DestBlendAction { get; set; }

        public Action MaxNumberOfParticlesAction { get; set; }

        public Action StartColor1Action { get; set; }

        public Action StartColor2Action { get; set; }

        public Action EndColor1Action { get; set; }

        public Action EndColor2Action { get; set; }

        public Action EmitRateMinAction { get; set; }

        public Action EmitRateMaxAction { get; set; }

        public Action ParticleSizeMinAction { get; set; }

        public Action ParticleSizeMaxAction { get; set; }

        public Action ParticleLifeTimeMinAction { get; set; }

        public Action ParticleLifeTimeMaxAction { get; set; }

        public Action TextureFileNameAction { get; set; }

        public bool ShowGround
        {
            get
            {
                return showGround;
            }

            set
            {
                showGround = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ShowGround"));
                ShowGroundAction?.Invoke();
            }
        }

        public float SpawnDir1X
        {
            get
            {
                return spawnDir1.X;
            }

            set
            {
                spawnDir1.X = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SpawnDir1X"));
                SpawnDir1Action?.Invoke();
            }
        }

        public float SpawnDir1Y
        {
            get
            {
                return spawnDir1.Y;
            }

            set
            {
                spawnDir1.Y = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SpawnDir1Y"));
                SpawnDir1Action?.Invoke();
            }
        }

        public float SpawnDir1Z
        {
            get
            {
                return spawnDir1.Z;
            }

            set
            {
                spawnDir1.Z = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SpawnDir1Z"));
                SpawnDir1Action?.Invoke();
            }
        }

        public float SpawnDir2X
        {
            get
            {
                return spawnDir2.X;
            }

            set
            {
                spawnDir2.X = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SpawnDir2X"));
                SpawnDir2Action?.Invoke();
            }
        }

        public float SpawnDir2Y
        {
            get
            {
                return spawnDir2.Y;
            }

            set
            {
                spawnDir2.Y = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SpawnDir2Y"));
                SpawnDir2Action?.Invoke();
            }
        }

        public float SpawnDir2Z
        {
            get
            {
                return spawnDir2.Z;
            }

            set
            {
                spawnDir2.Z = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SpawnDir2Z"));
                SpawnDir2Action?.Invoke();
            }
        }

        public float GravityX
        {
            get
            {
                return gravity.X;
            }

            set
            {
                gravity.X = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("GravityX"));
                GravityAction?.Invoke();
            }
        }

        public float GravityY
        {
            get
            {
                return gravity.Y;
            }

            set
            {
                gravity.Y = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("GravityY"));
                GravityAction?.Invoke();
            }
        }

        public float GravityZ
        {
            get
            {
                return gravity.Z;
            }

            set
            {
                gravity.Z = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("GravityZ"));
                GravityAction?.Invoke();
            }
        }

        public float PositionX
        {
            get
            {
                return position.X;
            }

            set
            {
                position.X = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("PositionX"));
                PositionAction?.Invoke();
            }
        }

        public float PositionY
        {
            get
            {
                return position.Y;
            }

            set
            {
                position.Y = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("PositionY"));
                PositionAction?.Invoke();
            }
        }

        public float PositionZ
        {
            get
            {
                return position.Z;
            }

            set
            {
                position.Z = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("PositionZ"));
                PositionAction?.Invoke();
            }
        }

        public float EmissionRadiusX
        {
            get
            {
                return emissionRadius.X;
            }

            set
            {
                emissionRadius.X = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("EmissionRadiusX"));
                EmissionRadiusAction?.Invoke();
            }
        }

        public float EmissionRadiusY
        {
            get
            {
                return emissionRadius.Y;
            }

            set
            {
                emissionRadius.Y = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("EmissionRadiusY"));
                EmissionRadiusAction?.Invoke();
            }
        }

        public float EmissionRadiusZ
        {
            get
            {
                return emissionRadius.Z;
            }

            set
            {
                emissionRadius.Z = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("EmissionRadiusZ"));
                EmissionRadiusAction?.Invoke();
            }
        }

        public LocalBlendState SourceBlendMode
        {
            get
            {
                var val = (int)sourceBlend;
                var retVal = (LocalBlendState)val;
                return retVal;
            }

            set
            {
                var val = (int)value;
                sourceBlend = (Blend)val;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SourceBlendMode"));
                SourceBlendAction?.Invoke();
            }
        }

        public LocalBlendState DestBlendMode
        {
            get
            {
                var val = (int)destBlend;
                var retVal = (LocalBlendState)val;
                return retVal;
            }

            set
            {
                var val = (int)value;
                destBlend = (Blend)val;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("DestBlendMode"));
                DestBlendAction?.Invoke();
            }
        }

        public int NumParticles
        {
            get
            {
                return maxNumberOfParticles;
            }

            set
            {
                maxNumberOfParticles = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("NumParticles"));
                MaxNumberOfParticlesAction?.Invoke();
            }
        }

        public System.Windows.Media.Color StartColor1
        {
            get
            {
                return startColor1;
            }

            set
            {
                startColor1 = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("StartColor1"));
                StartColor1Action?.Invoke();
            }
        }

        public System.Windows.Media.Color StartColor2
        {
            get
            {
                return startColor2;
            }

            set
            {
                startColor2 = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("StartColor2"));
                StartColor2Action?.Invoke();
            }
        }

        public System.Windows.Media.Color EndColor1
        {
            get
            {
                return endColor1;
            }

            set
            {
                endColor1 = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("EndColor1"));
                EndColor1Action?.Invoke();
            }
        }

        public System.Windows.Media.Color EndColor2
        {
            get
            {
                return endColor2;
            }

            set
            {
                endColor2 = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("EndColor2"));
                EndColor2Action?.Invoke();
            }
        }

        public float EmitRateMin
        {
            get
            {
                return emitRateMin;
            }

            set
            {
                emitRateMin = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("EmitRateMin"));
                EmitRateMinAction?.Invoke();
            }
        }

        public float EmitRateMax
        {
            get
            {
                return emitRateMax;
            }

            set
            {
                emitRateMax = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("EmitRateMax"));
                EmitRateMaxAction?.Invoke();
            }
        }

        public float ParticleSizeMin
        {
            get
            {
                return particleSizeMin;
            }

            set
            {
                particleSizeMin = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ParticleSizeMin"));
                ParticleSizeMinAction?.Invoke();
            }
        }

        public float ParticleSizeMax
        {
            get
            {
                return particleSizeMax;
            }

            set
            {
                particleSizeMax = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ParticleSizeMax"));
                ParticleSizeMaxAction?.Invoke();
            }
        }

        public float ParticleLifeTimeMin
        {
            get
            {
                return particleLifeTimeMin;
            }

            set
            {
                particleLifeTimeMin = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ParticleLifeTimeMin"));
                ParticleLifeTimeMinAction?.Invoke();
            }
        }

        public float ParticleLifeTimeMax
        {
            get
            {
                return particleLifeTimeMax;
            }

            set
            {
                particleLifeTimeMax = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ParticleLifeTimeMax"));
                ParticleLifeTimeMaxAction?.Invoke();
            }
        }

        public ObservableCollection<string> TextureFileNames
        {
            get
            {
                return textureFileNames;
            }

            set
            {
                textureFileNames = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("TextureFileNames"));
            }
        }

        public string TextureFileName
        {
            get
            {
                return textureFileName;
            }

            set
            {
                textureFileName = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("TextureFileName"));
                TextureFileNameAction?.Invoke();
            }
        }

        public ICommand SaveCommand
        {
            get
            {
                return saveCommand;
            }

            set
            {
                saveCommand = value;
            }
        }

        public ICommand LoadCommand
        {
            get
            {
                return loadCommand;
            }

            set
            {
                loadCommand = value;
            }
        }

        public ICommand ResetCommand
        {
            get
            {
                return resetCommand;
            }

            set
            {
                resetCommand = value;
            }
        }

        public void SaveFile(object obj)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "XML file (*.xml)|*.xml"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                SaveXML(saveFileDialog.FileName);
            }
        }

        public void LoadFile(object obj)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "XML file (*.xml)|*.xml"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                LoadXML(openFileDialog.FileName);
            }
        }

        public void SetDefaultValues(object obj)
        {
            SetDefaultValues();
        }

        public void SetDefaultValues()
        {
            ShowGround = true;

            SpawnDir1X = -10.0f;
            SpawnDir1Y = -10.0f;
            SpawnDir1Z = -10.0f;

            SpawnDir2X = 10.0f;
            SpawnDir2Y = 10.0f;
            SpawnDir2Z = 10.0f;

            PositionX = 0.0f;
            PositionY = 1.0f;
            PositionZ = 0.0f;

            EmissionRadiusX = 1.0f;
            EmissionRadiusY = 1.0f;
            EmissionRadiusZ = 1.0f;

            SourceBlendMode = LocalBlendState.SourceAlpha;
            DestBlendMode = LocalBlendState.One;

            NumParticles = 500;

            StartColor1 = System.Windows.Media.Color.FromArgb(0, 0, 0, 0);
            StartColor2 = System.Windows.Media.Color.FromArgb(255, 255, 255, 255);
            EndColor1 = System.Windows.Media.Color.FromArgb(0, 0, 0, 0);
            EndColor2 = System.Windows.Media.Color.FromArgb(255, 255, 255, 255);
            EmitRateMin = 10.0f;
            EmitRateMax = 20.0f;

            ParticleSizeMin = 1.0f;
            ParticleSizeMax = 1.0f;

            ParticleLifeTimeMin = 0.0f;
            ParticleLifeTimeMax = 10.0f;

            if (!TextureFileNames.Contains("Ch18p1_ParticleTexture"))
            {
                TextureFileNames.Add("Ch18p1_ParticleTexture");
            }

            TextureFileName = "Ch18p1_ParticleTexture";
        }

        private void SaveXML(string fileName)
        {
            XElement particleSystem =
                new XElement("ParticleSystem",
                    new XElement("Position",
                        new XAttribute("x", PositionX),
                        new XAttribute("y", PositionY),
                        new XAttribute("z", PositionZ)),
                    new XElement("Gravity",
                        new XAttribute("x", GravityX),
                        new XAttribute("y", GravityY),
                        new XAttribute("z", GravityZ)),
                    new XElement("SpawnDir1",
                        new XAttribute("x", SpawnDir1X),
                        new XAttribute("y", SpawnDir1Y),
                        new XAttribute("z", SpawnDir1Z)),
                    new XElement("SpawnDir2",
                        new XAttribute("x", SpawnDir2X),
                        new XAttribute("y", SpawnDir2Y),
                        new XAttribute("z", SpawnDir2Z)),
                    new XElement("StartColor1",
                        new XAttribute("r", StartColor1.R),
                        new XAttribute("g", StartColor1.G),
                        new XAttribute("b", StartColor1.B),
                        new XAttribute("a", StartColor1.A)),
                    new XElement("StartColor2",
                        new XAttribute("r", StartColor2.R),
                        new XAttribute("g", StartColor2.G),
                        new XAttribute("b", StartColor2.B),
                        new XAttribute("a", StartColor2.A)),
                    new XElement("EndColor1",
                        new XAttribute("r", EndColor1.R),
                        new XAttribute("g", EndColor1.G),
                        new XAttribute("b", EndColor1.B),
                        new XAttribute("a", EndColor1.A)),
                    new XElement("EndColor2",
                        new XAttribute("r", EndColor2.R),
                        new XAttribute("g", EndColor2.G),
                        new XAttribute("b", EndColor2.B),
                        new XAttribute("a", EndColor2.A)),
                    new XElement("EmitRate",
                        new XAttribute("min", EmitRateMin),
                        new XAttribute("max", EmitRateMax)),
                    new XElement("EmitRadius",
                        new XAttribute("x", EmissionRadiusX),
                        new XAttribute("y", EmissionRadiusY),
                        new XAttribute("z", EmissionRadiusZ)),
                    new XElement("LifeTime",
                        new XAttribute("min", ParticleLifeTimeMin),
                        new XAttribute("max", ParticleLifeTimeMax)),
                    new XElement("Size",
                        new XAttribute("min", ParticleSizeMin),
                        new XAttribute("max", ParticleSizeMax)),
                    new XElement("SrcBlend", SourceBlendMode.ToString()),
                    new XElement("DestBlend", DestBlendMode.ToString()),
                    new XElement("MaxParticles", maxNumberOfParticles.ToString()),
                    new XElement("Texture", "Ch18p1_ParticleTexture"));

            particleSystem.Save(fileName);
        }

        private void LoadXML(string fileName)
        {
            XElement particleSystem = XElement.Load(fileName);
            var position = particleSystem.Element("Position");
            PositionX = float.Parse(position.Attribute("x").Value, CultureInfo.InvariantCulture.NumberFormat);
            PositionY = float.Parse(position.Attribute("y").Value, CultureInfo.InvariantCulture.NumberFormat);
            PositionZ = float.Parse(position.Attribute("z").Value, CultureInfo.InvariantCulture.NumberFormat);

            var gravity = particleSystem.Element("Gravity");
            GravityX = float.Parse(gravity.Attribute("x").Value, CultureInfo.InvariantCulture.NumberFormat);
            GravityY = float.Parse(gravity.Attribute("y").Value, CultureInfo.InvariantCulture.NumberFormat);
            GravityZ = float.Parse(gravity.Attribute("z").Value, CultureInfo.InvariantCulture.NumberFormat);

            var spawnDir1 = particleSystem.Element("SpawnDir1");
            SpawnDir1X = float.Parse(spawnDir1.Attribute("x").Value, CultureInfo.InvariantCulture.NumberFormat);
            SpawnDir1Y = float.Parse(spawnDir1.Attribute("y").Value, CultureInfo.InvariantCulture.NumberFormat);
            SpawnDir1Z = float.Parse(spawnDir1.Attribute("z").Value, CultureInfo.InvariantCulture.NumberFormat);

            var spawnDir2 = particleSystem.Element("SpawnDir2");
            SpawnDir2X = float.Parse(spawnDir2.Attribute("x").Value, CultureInfo.InvariantCulture.NumberFormat);
            SpawnDir2Y = float.Parse(spawnDir2.Attribute("y").Value, CultureInfo.InvariantCulture.NumberFormat);
            SpawnDir2Z = float.Parse(spawnDir2.Attribute("z").Value, CultureInfo.InvariantCulture.NumberFormat);

            var startColor1 = particleSystem.Element("StartColor1");
            System.Windows.Media.Color col = new System.Windows.Media.Color
            {
                R = byte.Parse(startColor1.Attribute("r").Value, CultureInfo.InvariantCulture.NumberFormat),
                G = byte.Parse(startColor1.Attribute("g").Value, CultureInfo.InvariantCulture.NumberFormat),
                B = byte.Parse(startColor1.Attribute("b").Value, CultureInfo.InvariantCulture.NumberFormat),
                A = byte.Parse(startColor1.Attribute("a").Value, CultureInfo.InvariantCulture.NumberFormat)
            };
            StartColor1 = col;

            var startColor2 = particleSystem.Element("StartColor2");
            col.R = byte.Parse(startColor2.Attribute("r").Value, CultureInfo.InvariantCulture.NumberFormat);
            col.G = byte.Parse(startColor2.Attribute("g").Value, CultureInfo.InvariantCulture.NumberFormat);
            col.B = byte.Parse(startColor2.Attribute("b").Value, CultureInfo.InvariantCulture.NumberFormat);
            col.A = byte.Parse(startColor2.Attribute("a").Value, CultureInfo.InvariantCulture.NumberFormat);
            StartColor2 = col;

            var endColor1 = particleSystem.Element("EndColor1");
            col.R = byte.Parse(endColor1.Attribute("r").Value, CultureInfo.InvariantCulture.NumberFormat);
            col.G = byte.Parse(endColor1.Attribute("g").Value, CultureInfo.InvariantCulture.NumberFormat);
            col.B = byte.Parse(endColor1.Attribute("b").Value, CultureInfo.InvariantCulture.NumberFormat);
            col.A = byte.Parse(endColor1.Attribute("a").Value, CultureInfo.InvariantCulture.NumberFormat);
            EndColor1 = col;

            var endColor2 = particleSystem.Element("EndColor2");
            col.R = byte.Parse(endColor2.Attribute("r").Value, CultureInfo.InvariantCulture.NumberFormat);
            col.G = byte.Parse(endColor2.Attribute("g").Value, CultureInfo.InvariantCulture.NumberFormat);
            col.B = byte.Parse(endColor2.Attribute("b").Value, CultureInfo.InvariantCulture.NumberFormat);
            col.A = byte.Parse(endColor2.Attribute("a").Value, CultureInfo.InvariantCulture.NumberFormat);
            EndColor2 = col;

            var emitRate = particleSystem.Element("EmitRate");
            EmitRateMin = float.Parse(emitRate.Attribute("min").Value, CultureInfo.InvariantCulture.NumberFormat);
            EmitRateMax = float.Parse(emitRate.Attribute("max").Value, CultureInfo.InvariantCulture.NumberFormat);

            var emitRadius = particleSystem.Element("EmitRadius");
            EmissionRadiusX = float.Parse(emitRadius.Attribute("x").Value, CultureInfo.InvariantCulture.NumberFormat);
            EmissionRadiusY = float.Parse(emitRadius.Attribute("y").Value, CultureInfo.InvariantCulture.NumberFormat);
            EmissionRadiusZ = float.Parse(emitRadius.Attribute("z").Value, CultureInfo.InvariantCulture.NumberFormat);

            var lifeTime = particleSystem.Element("LifeTime");
            ParticleLifeTimeMin = float.Parse(lifeTime.Attribute("min").Value, CultureInfo.InvariantCulture.NumberFormat);
            ParticleLifeTimeMax = float.Parse(lifeTime.Attribute("max").Value, CultureInfo.InvariantCulture.NumberFormat);

            var size = particleSystem.Element("Size");
            ParticleSizeMin = float.Parse(size.Attribute("min").Value, CultureInfo.InvariantCulture.NumberFormat);
            ParticleSizeMax = float.Parse(size.Attribute("max").Value, CultureInfo.InvariantCulture.NumberFormat);

            var srcBlend = particleSystem.Element("SrcBlend").Value;
            SourceBlendMode = (LocalBlendState)Enum.Parse(typeof(LocalBlendState), srcBlend);

            var destBlend = particleSystem.Element("DestBlend").Value;
            DestBlendMode = (LocalBlendState)Enum.Parse(typeof(LocalBlendState), destBlend);

            var maxParticles = particleSystem.Element("MaxParticles");
            NumParticles = int.Parse(maxParticles.Value, CultureInfo.InvariantCulture.NumberFormat);

            // TODO: Texture is not used yet.
            var texture = particleSystem.Element("Texture");
            if (!TextureFileNames.Contains(texture.Value))
            {
                TextureFileNames.Add(texture.Value);
            }

            TextureFileName = texture.Value;
        }
    }
}