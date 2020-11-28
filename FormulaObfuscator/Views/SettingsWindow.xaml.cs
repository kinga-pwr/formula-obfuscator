using FormulaObfuscator.BLL.Models;
using FormulaObfuscator.BLL.TestSamplesGenerator;
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
        private Settings Settings { get; set; }
        private List<CheckBox> SimpleMethods = new List<CheckBox> { };
        private List<CheckBox> ComplexMethods = new List<CheckBox> { };
        private Dictionary<TypeOfMethod, CheckBox> EqualOneMethods = new Dictionary<TypeOfMethod, CheckBox> { };
        private Dictionary<TypeOfMethod, CheckBox> EqualZeroMethods = new Dictionary<TypeOfMethod, CheckBox> { };
        private List<CheckBox> SamplesMethods = new List<CheckBox> { };

        public DelegateCommand ObfuscateLevelsCommand { get; set; }

        public SettingsWindow(Settings settings)
        {
            Settings = settings;

            ObfuscateLevelsCommand = new DelegateCommand(() => GetAllObfuscateLevels());

            InitializeComponent();

            MakeCheckBoxGroups();

            InitialFields();
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

        private void InitialFields()
        {
            TextBoxLetters.Text = Settings.Letters;
            TextBoxGreekLetters.Text = Settings.GreekLetters;
            DropDownButtonObfuscatedLevels.ItemsSource = GetAllObfuscateLevels();
            DropDownButtonObfuscatedLevels.SelectedIndex = (int) Settings.ObfuscateLevel;
            UpDownMin.Value = Settings.MinNumber;
            UpDownMax.Value = Settings.MaxNumber;
            UpDownRecursionDepth.Value = Settings.RecursionDepth;
            UpDownTreeWalks.Value = Settings.ObfucateCount;
            UpDownProbability.Value = Settings.ObfucateProbability;

            CheckSelectedCheckboxes(SimpleMethods, Settings.SimpleMethods);
            CheckSelectedCheckboxes(ComplexMethods, Settings.ComplexMethods);
            CheckSelectedCheckboxes(EqualOneMethods, Settings.MethodsForOneGenerator);
            CheckSelectedCheckboxes(EqualZeroMethods, Settings.MethodsForZeroGenerator);
            CheckSelectedCheckboxes(SamplesMethods, Settings.MethodsForSamplesGenerator);
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
            foreach (var method in methods)
            {
                methodsCB.TryGetValue(method, out CheckBox toCheck);
                toCheck.IsChecked = true;
            }
        }

        private void ButtonSave_Clicked(object sender, RoutedEventArgs e)
        {
            // TODO validation
            Settings.RecursionDepth = (int) UpDownRecursionDepth.Value;
            Settings.ObfuscateLevel = (ObfuscateLevel) DropDownButtonObfuscatedLevels.SelectedIndex;
            Settings.ObfucateCount = (int) UpDownTreeWalks.Value;
            Settings.Letters = TextBoxLetters.Text;
            Settings.GreekLetters = TextBoxGreekLetters.Text;
            Settings.MinNumber = (int) UpDownMin.Value;
            Settings.MaxNumber = (int) UpDownMax.Value;
            Settings.SimpleMethods = GetAllCheckedMethods<SimpleGeneratorMethod>(SimpleMethods);
            Settings.ComplexMethods = GetAllCheckedMethods<ComplexGeneratorMethod>(ComplexMethods);
            Settings.MethodsForOneGenerator = GetAllCheckedMethods(EqualOneMethods);
            Settings.MethodsForZeroGenerator = GetAllCheckedMethods(EqualZeroMethods);
            Settings.MethodsForSamplesGenerator = GetAllCheckedMethods<SamplesGeneratorMethod>(SamplesMethods);

            this.Close();
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

        private void ButtonUpload_Clicked(object sender, RoutedEventArgs e)
        {

        }

        private void ButtonSaveAs_Clicked(object sender, RoutedEventArgs e)
        {

        }
    }
}
