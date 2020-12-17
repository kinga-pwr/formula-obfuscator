using FormulaObfuscator.BLL;
using FormulaObfuscator.BLL.Models;
using FormulaObfuscator.BLL.TestGenerator;
using FormulaObfuscator.Commands;
using MahApps.Metro.Controls.Dialogs;
using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using FormulaObfuscator.Views;
using System.Text.Json;

namespace FormulaObfuscator.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private const string DefaultInputFile = "Input.html";
        private readonly IDialogCoordinator _dialogCoordinator;

        public DelegateCommand InputUploadCommand { get; set; }
        public DelegateCommand OutputUploadCommand { get; set; }
        public DelegateCommand ObfuscateCommand { get; set; }
        public DelegateCommand DeobfuscateCommand { get; set; }
        public DelegateCommand InputDownloadCommand { get; set; }
        public DelegateCommand OutputDownloadCommand { get; set; }
        public ParameterCommand<int> LoadSampleCommand { get; set; }
        public DelegateCommand GenerateSampleCommand { get; set; }
        public DelegateCommand OpenSettingsCommand { get; set; }

        public DelegateCommand InputFirefoxCommand { get; set; }
        public DelegateCommand OutputFirefoxCommand { get; set; }

        private Settings Settings;

        public bool HasFirefox { get; }
        public string FirefoxTooltip => HasFirefox 
            ? "Open Firefox with html file from text box" 
            : "No Firefox detected, functionality not available";

        private string _input;
        public string Input
        {
            get => _input;
            set => SetProperty(ref _input, value);
        }

        private string _output;
        public string Output
        {
            get => _output;
            set => SetProperty(ref _output, value);
        }

        public MainWindowViewModel(IDialogCoordinator dialogCoordinator)
        {
            InputUploadCommand = new DelegateCommand(() => UploadFile(true));
            OutputUploadCommand = new DelegateCommand(() => UploadFile(false));
            ObfuscateCommand = new DelegateCommand(() => Obfuscate());
            DeobfuscateCommand = new DelegateCommand(() => Deobfuscate());
            InputDownloadCommand = new DelegateCommand(() => DownloadCode(Input, "Plain html"));
            OutputDownloadCommand = new DelegateCommand(() => DownloadCode(Output, "Obfuscated html"));
            LoadSampleCommand = new ParameterCommand<int>((sampleId) => LoadSample(sampleId));
            GenerateSampleCommand = new DelegateCommand(() => GenerateSample());
            OpenSettingsCommand = new DelegateCommand(() => OpenSettings());
            InputFirefoxCommand = new DelegateCommand(() => OpenInFirefox(Input));
            OutputFirefoxCommand = new DelegateCommand(() => OpenInFirefox(Output));

            Settings = GetSettings();

            _dialogCoordinator = dialogCoordinator;

            HasFirefox = CheckIfFirefoxIsInstalled();
            OnPropertyChanged(nameof(HasFirefox));
            OnPropertyChanged(nameof(FirefoxTooltip));

            LoadInitialFile();
        }

        private Settings GetSettings()
        {
            var settings = new Settings();

            try
            {
                using (StreamReader r = new StreamReader("currSettings"))
                {
                    string json = r.ReadToEnd();
                    settings = JsonSerializer.Deserialize<Settings>(json);
                }
            } catch
            {
                return settings;
            }

            return settings;
        }

        private bool CheckIfFirefoxIsInstalled()
        {
            string keyName = @"HKEY_LOCAL_MACHINE\SOFTWARE\Mozilla\Mozilla Firefox";
            string valueName = "CurrentVersion";
            return Registry.GetValue(keyName, valueName, null) != null;
        }

        private void OpenSettings()
        {
            new SettingsWindow(Settings).ShowDialog();
        }

        /// <summary>
        /// Automatically load default file (Input.html) if exists
        /// </summary>
        private void LoadInitialFile()
        {
            if (File.Exists(DefaultInputFile))
            {
                Input = File.ReadAllText(DefaultInputFile);
            }
        }

        private void UploadFile(bool isInput)
        {
            var openDialog = new OpenFileDialog
            {
                Filter = "HTML Files|*.html",
                FilterIndex = 2,
                RestoreDirectory = true
            };
            if (openDialog.ShowDialog() == true)
            {
                if (isInput) Input = File.ReadAllText(openDialog.FileName);
                else Output = File.ReadAllText(openDialog.FileName);
            }
        }

        private async void Obfuscate()
        {
            var progressController = await _dialogCoordinator.ShowProgressAsync(this, "Obfuscating", "Obfuscating data...");

            progressController.SetIndeterminate();

            await ObfuscateData();
            await progressController.CloseAsync();
        }


        private async Task ObfuscateData()
        {
            await Task.Run(async () =>
            {
                var resultHolder = new ObfuscatorManager(Input).RunObfuscate(Settings);

                if (resultHolder.WasSuccessful)
                    Output = resultHolder.Value;
                else
                {
                    Output = string.Empty;
                    await _dialogCoordinator.ShowMessageAsync(this, "Error", resultHolder.ErrorMsg, 
                        settings: new MetroDialogSettings()
                        {
                            ColorScheme = MetroDialogColorScheme.Accented
                        });
                }
            });
        }

        private async void Deobfuscate()
        {
            var progressController = await _dialogCoordinator.ShowProgressAsync(this, "Deobfuscating", "Deobfuscating data...");

            progressController.SetIndeterminate();

            try
            {
                await DeobfuscateData();
                await progressController.CloseAsync();
            }
            catch
            {
                await progressController.CloseAsync();
                await _dialogCoordinator.ShowMessageAsync(this, "Error", "Error occured!");
            }
        }

        private async Task DeobfuscateData()
        {
            await Task.Run(async () =>
            {
                var resultHolder = new ObfuscatorManager(Output).RunDeobfuscate();

                if (resultHolder.WasSuccessful)
                    Input = resultHolder.Value;
                else
                {
                    Input = string.Empty;
                    await _dialogCoordinator.ShowMessageAsync(this, "Error", resultHolder.ErrorMsg,
                        settings: new MetroDialogSettings()
                        {
                            ColorScheme = MetroDialogColorScheme.Accented
                        });
                }
            });
        }

        private async void DownloadCode(string data, string name)
        {
            var saveDialog = new SaveFileDialog
            {
                FileName = $"{name} {DateTime.Now:dd-MM-yyyy HH_mm}",
                Filter = "HTML Files|*.html",
                FilterIndex = 2,
                RestoreDirectory = true
            };

            if (saveDialog.ShowDialog() == true)
            {
                File.WriteAllText(saveDialog.FileName, data);
                await _dialogCoordinator.ShowMessageAsync(this, "File exported", "Code file successfully exported!");
            }
        }

        private async void LoadSample(int sampleId)
        {
            try
            {
                Input = await File.ReadAllTextAsync($"Samples/sample{sampleId}.html");
            }
            catch (Exception e)
            {
                Input = e.Message;
            }
        }

        private void GenerateSample()
        {
            var samplesGenerator = new SamplesGenerator();
            Input = samplesGenerator.DrawSample(Settings);
        }

        private async void OpenInFirefox(string data)
        {
            string keyName = @"HKEY_LOCAL_MACHINE\SOFTWARE\Mozilla\Mozilla Firefox";
            string valueName = "CurrentVersion";
            var mozillaVersion = Registry.GetValue(keyName, valueName, null);
            keyName = $@"HKEY_LOCAL_MACHINE\SOFTWARE\Mozilla\Mozilla Firefox\{mozillaVersion}\Main";
            valueName = "PathToExe";
            var mozillaExe = Registry.GetValue(keyName, valueName, null).ToString();
            if (mozillaExe != null)
            {
                File.WriteAllText("temp", data);
                Process.Start(mozillaExe, "temp");
            }
            else
            {
                await _dialogCoordinator.ShowMessageAsync(this, "Error", "Error opening Firefox!",
                        settings: new MetroDialogSettings()
                        {
                            ColorScheme = MetroDialogColorScheme.Accented
                        });
            }
        }
    }
}
