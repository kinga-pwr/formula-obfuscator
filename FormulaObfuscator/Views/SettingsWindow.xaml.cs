using FormulaObfuscator.BLL.Models;
using FormulaObfuscator.BLL.TestSamplesGenerator;
using FormulaObfuscator.Commands;
using MahApps.Metro.Controls;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
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
        private Settings Settings { get; set; }
        private List<CheckBox> SimpleMethods = new List<CheckBox> { };
        private List<CheckBox> ComplexMethods = new List<CheckBox> { };
        private Dictionary<TypeOfMethod, CheckBox> EqualOneMethods = new Dictionary<TypeOfMethod, CheckBox> { };
        private Dictionary<TypeOfMethod, CheckBox> EqualZeroMethods = new Dictionary<TypeOfMethod, CheckBox> { };
        private List<CheckBox> SamplesMethods = new List<CheckBox> { };
        private Brush DefaultValue;

        public SettingsWindow(Settings settings)
        {
            Settings = settings;

            InitializeComponent();

            DefaultValue = ComplexLabel.Foreground;

            MakeCheckBoxGroups();

            InitialFields(Settings);
        }

        private void MakeCheckBoxGroups()
        {
            SimpleMethods.Add(CBSimpleFraction);
            SimpleMethods.Add(CBSimplePower);
            SimpleMethods.Add(CBSimpleRoot);

            ComplexMethods.Add(CBComplexFraction);
            ComplexMethods.Add(CBComplexRoot);

            EqualOneMethods.Add(TypeOfMethod.Polynomial, CBEqualsOnePolynomial);
            EqualOneMethods.Add(TypeOfMethod.Fraction, CBEqualsOneFraction);
            EqualOneMethods.Add(TypeOfMethod.Trigonometry, CBEqualsOneTrigonometry);
            EqualOneMethods.Add(TypeOfMethod.Root, CBEqualsOneRoot);
            EqualOneMethods.Add(TypeOfMethod.TrigonometryRedundancy, CBEqualsOneTrigonometryRed);
            EqualOneMethods.Add(TypeOfMethod.Equation, CBEqualsOneEquation);

            EqualZeroMethods.Add(TypeOfMethod.Polynomial, CBEqualsZeroPolynomial);
            EqualZeroMethods.Add(TypeOfMethod.Fraction, CBEqualsZeroFraction);
            EqualZeroMethods.Add(TypeOfMethod.Trigonometry, CBEqualsZeroTrigonometry);
            EqualZeroMethods.Add(TypeOfMethod.Integral, CBEqualsZeroIntegral);

            SamplesMethods.Add(CBSamplesDetSumPi);
            SamplesMethods.Add(CBSamplesFi);
            SamplesMethods.Add(CBSamplesFuncBracket);
            SamplesMethods.Add(CBSamplesIntegral);
            SamplesMethods.Add(CBSamplesLimit);
            SamplesMethods.Add(CBSamplesSqrtRecursive);
            SamplesMethods.Add(CBSamplesSum);
        }

        private List<string> GetAllObfuscateLevels()
        {
            return new List<string> { "Full formula", "Fractions", "Variables"};
        }

        private void InitialFields(Settings toFill)
        {
            TextBoxLetters.Text = toFill.Letters;
            TextBoxGreekLetters.Text = toFill.GreekLetters;
            DropDownButtonObfuscatedLevels.ItemsSource = GetAllObfuscateLevels();
            DropDownButtonObfuscatedLevels.SelectedIndex = (int)toFill.ObfuscateLevel;
            UpDownMin.Value = toFill.MinNumber;
            UpDownMax.Value = toFill.MaxNumber;
            UpDownRecursionDepth.Value = toFill.RecursionDepth;
            UpDownTreeWalks.Value = toFill.ObfucateCount;
            UpDownProbability.Value = toFill.ObfucateProbability;

            CheckSelectedCheckboxes(SimpleMethods, toFill.SimpleMethods);
            CheckSelectedCheckboxes(ComplexMethods, toFill.ComplexMethods);
            CheckSelectedCheckboxes(EqualOneMethods, toFill.MethodsForOneGenerator);
            CheckSelectedCheckboxes(EqualZeroMethods, toFill.MethodsForZeroGenerator);
            CheckSelectedCheckboxes(SamplesMethods, toFill.MethodsForSamplesGenerator);
        }

        private static void CheckSelectedCheckboxes<EnumType>(List<CheckBox> methodsCB, List<EnumType> methods)
        {
            for (int idx = 0; idx < methodsCB.Count; idx++)
            {
                methodsCB[idx].IsChecked = methods.Contains((EnumType)(object) idx);
            }
        }

        private static void CheckSelectedCheckboxes(Dictionary<TypeOfMethod, CheckBox> methodsCB, List<TypeOfMethod> methods)
        {
            foreach (var method in methodsCB.Keys)
            {
                methodsCB[method].IsChecked = methods.Contains(method);
            }
        }

        private void ButtonSave_Clicked(object sender, RoutedEventArgs e)
        {
            GetConfigurationFromUI(Settings);

            SaveCurrentSetting();

            this.Close();
        }

        private void SaveCurrentSetting()
        {
            var serializerOptions = new JsonSerializerOptions();
            serializerOptions.WriteIndented = true;
            var serializedSettings = JsonSerializer.Serialize<Settings>(Settings, serializerOptions);
            File.WriteAllText("currSettings", serializedSettings.ToString());
        }

        private void GetConfigurationFromUI(Settings toStore)
        {
            toStore.RecursionDepth = (int)UpDownRecursionDepth.Value;
            toStore.ObfuscateLevel = (ObfuscateLevel)DropDownButtonObfuscatedLevels.SelectedIndex;
            toStore.ObfucateCount = (int)UpDownTreeWalks.Value;
            toStore.ObfucateProbability = (int)UpDownProbability.Value;
            toStore.Letters = TextBoxLetters.Text;
            toStore.GreekLetters = TextBoxGreekLetters.Text;
            toStore.MinNumber = (int)UpDownMin.Value;
            toStore.MaxNumber = (int)UpDownMax.Value;
            toStore.SimpleMethods = GetAllCheckedMethods<SimpleGeneratorMethod>(SimpleMethods);
            toStore.ComplexMethods = GetAllCheckedMethods<ComplexGeneratorMethod>(ComplexMethods);
            toStore.MethodsForOneGenerator = GetAllCheckedMethods(EqualOneMethods);
            toStore.MethodsForZeroGenerator = GetAllCheckedMethods(EqualZeroMethods);
            toStore.MethodsForSamplesGenerator = GetAllCheckedMethods<SamplesGeneratorMethod>(SamplesMethods);
        }

        private List<TypeOfMethod> GetAllCheckedMethods(Dictionary<TypeOfMethod, CheckBox> methods)
        {
            List<TypeOfMethod> result = new List<TypeOfMethod>();

            foreach (var method in methods.Keys)
            {
                if ((bool) methods[method].IsChecked)
                {
                    result.Add(method);
                }
            }

            return result;
        }

        private List<EnumType> GetAllCheckedMethods<EnumType>(List<CheckBox> methods)
        {
            List<EnumType> result = new List<EnumType>();

            for (int idx = 0; idx < methods.Count; idx++)
            {
               if ((bool) methods[idx].IsChecked)
                    result.Add((EnumType)(object)idx);
            }

            return result;
        }

        private void ButtonExport_Clicked(object sender, RoutedEventArgs e)
        {
            var saveDialog = new SaveFileDialog
            {
                FileName = $"Configuration {DateTime.Now:dd-MM-yyyy HH_mm}",
                Filter = "Json Files|*.json",
                FilterIndex = 2,
                RestoreDirectory = true
            };

            if (saveDialog.ShowDialog() == true)
            {
                var serializerOptions = new JsonSerializerOptions();
                serializerOptions.WriteIndented = true;
                Settings tmpSettings = new Settings();
                GetConfigurationFromUI(tmpSettings);
                var serializedSettings = JsonSerializer.Serialize<Settings>(tmpSettings, serializerOptions);
                File.WriteAllText(saveDialog.FileName, serializedSettings.ToString());
            }
        }

        private void ButtonUpload_Clicked(object sender, RoutedEventArgs e)
        {
            var openDialog = new OpenFileDialog
            {
                Filter = "Json Files|*.json",
                FilterIndex = 2,
                RestoreDirectory = true
            };

            if (openDialog.ShowDialog() == true)
            {
                using (StreamReader r = new StreamReader(openDialog.FileName))
                {
                    string json = r.ReadToEnd();
                    var uploadedSettings = JsonSerializer.Deserialize<Settings>(json);
                    InitialFields(uploadedSettings);
                }
            }
        }

        private void ButtonReset_Clicked(object sender, RoutedEventArgs e)
        {
            InitialFields(new Settings());
        }

        private void ValidateAll(object sender, RoutedEventArgs e)
        {
            var areComplexValid = !AreAllCheckboxesUnchecked(ComplexMethods, ComplexLabel);
            var areSimpleValid = !AreAllCheckboxesUnchecked(SimpleMethods, SimpleLabel);
            var areSamplesValid = !AreAllCheckboxesUnchecked(SamplesMethods, SamplesLabel);

            var areAllValid = BindingOperations.GetBindingExpression(TextBoxLetters, TextBox.TextProperty).ValidateWithoutUpdate()
                && BindingOperations.GetBindingExpression(TextBoxGreekLetters, TextBox.TextProperty).ValidateWithoutUpdate()
                && areComplexValid
                && areSimpleValid
                && areSamplesValid;

            SaveButton.IsEnabled = areAllValid;
            ExportButton.IsEnabled = areAllValid;
        }

        private bool AreAllCheckboxesUnchecked(List<CheckBox> checkBoxes, Label label)
        {
            var isValid = checkBoxes.TrueForAll(cb => !((bool)cb.IsChecked));

            if (isValid)
                label.Foreground = Brushes.Red;
            else
                label.Foreground = DefaultValue;

            return isValid;
        }
    }
}
