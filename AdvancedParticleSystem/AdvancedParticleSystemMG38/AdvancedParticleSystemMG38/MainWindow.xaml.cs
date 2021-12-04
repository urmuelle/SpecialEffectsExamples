using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using AdvancedParticleSystem.ViewModel;

namespace AdvancedParticleSystemMG38
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {            
            InitializeComponent();
            var viewModel = new MainWindowViewModel();
            Loaded += (s, e) => { DataContext = viewModel; };

            if (viewModel.ShowGroundAction == null)
            {
                viewModel.ShowGroundAction = new Action(() =>
                {
                    ParticleScene.ShowGround = viewModel.ShowGround;
                });
            }

            if (viewModel.CompileCommandAction == null)
            {
                viewModel.CompileCommandAction = new Action(() =>
                {
                    ParticleScene.ParticleSystemDescription = viewModel.ParticleSystemDescription;
                });
            }
        }
    }
}
