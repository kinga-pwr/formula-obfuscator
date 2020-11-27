using FormulaObfuscator.BLL.Models;
using FormulaObfuscator.Commands;
using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace FormulaObfuscator.Views
{
    /// <summary>
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : MetroWindow
    {
        public Settings Settings { get; set; }

        public DelegateCommand ObfuscateLevelsCommand { get; set; }

        public SettingsWindow(Settings settings)
        {
            Settings = settings;

            ObfuscateLevelsCommand = new DelegateCommand(() => GetAllObfuscateLevels());

            InitializeComponent();

            InitialFields();
        }

        private List<string> GetAllObfuscateLevels()
        {
            return new List<string> { "Full formula", "Fractions", "Variables"};
        }

        private void InitialFields()
        {
            TextBoxLetters.Text = Settings.Letters;
            DropDownButtonObfuscatedLevels.ItemsSource = GetAllObfuscateLevels();
        }
    }
}
