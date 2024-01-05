// <copyright file="MainWindowViewModel.cs" company="Urs Müller">
// Copyright (c) Urs Müller. All rights reserved.
// </copyright>

namespace AdvancedParticleSystemMG38_1.ViewModel
{
    using System;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Input;
    using System.Xml.Linq;
    using AdvancedParticleSystemMG38_1.Scene;
    using Microsoft.Win32;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private bool showGround;
        private string particleSystemDescription;

        private ICommand saveCommand;
        private ICommand loadCommand;
        private ICommand compileCommand;

        public MainWindowViewModel()
        {
            saveCommand = new RelayCommand(SaveFile);
            loadCommand = new RelayCommand(LoadFile);
            compileCommand = new RelayCommand(Compile);
            showGround = true;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public Action ShowGroundAction { get; set; }

        public Action LastErrorAction { get; set; }

        public Action CompileCommandAction { get; set; }

        public bool ShowGround
        {
            get
            {
                return showGround;
            }

            set
            {
                showGround = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ShowGround)));
                ShowGroundAction?.Invoke();
            }
        }

        public string ParticleSystemDescription
        {
            get
            {
                return particleSystemDescription;
            }

            set
            {
                particleSystemDescription = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ParticleSystemDescription)));
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

        public ICommand CompileCommand
        {
            get
            {
                return compileCommand;
            }

            set
            {
                compileCommand = value;
            }
        }

        public void SaveFile(object obj)
        {
            SaveFileDialog saveFileDialog = new ()
            {
                Filter = "TXT file (*.txt)|*.txt",
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                SaveTxtFile(saveFileDialog.FileName);
            }
        }

        public void LoadFile(object obj)
        {
            OpenFileDialog openFileDialog = new ()
            {
                Filter = "TXT file (*.txt)|*.txt",
            };

            if (openFileDialog.ShowDialog() == true)
            {
                LoadTxtFile(openFileDialog.FileName);
            }
        }

        public void Compile(object obj)
        {
            CompileCommandAction?.Invoke();
        }

        private void SaveTxtFile(string fileName)
        {
            var text = ParticleSystemDescription;

            System.IO.File.WriteAllText(fileName, text);
        }

        private void LoadTxtFile(string fileName)
        {
            var text = System.IO.File.ReadAllText(fileName);

            ParticleSystemDescription = text;
        }
    }
}
