using FormulaObfuscator.BLL;
using FormulaObfuscator.BLL.Models;
using FormulaObfuscator.Commands;
using MahApps.Metro.Controls.Dialogs;
using Microsoft.Win32;
using System;
using System.IO;
using System.Threading.Tasks;
using FormulaObfuscator.BLL.TestGenerator;

namespace FormulaObfuscator.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private const string DefaultInputFile = "Input.html";
        private readonly IDialogCoordinator _dialogCoordinator;

        public DelegateCommand UploadCommand { get; set; }
        public DelegateCommand ObfuscateCommand { get; set; }
        public DelegateCommand DownloadCommand { get; set; }
        public ParameterCommand<int> LoadSampleCommand { get; set; }
        public DelegateCommand GenerateSampleCommand { get; set; }

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
            UploadCommand = new DelegateCommand(() => UploadFile());
            ObfuscateCommand = new DelegateCommand(() => Obfuscate());
            DownloadCommand = new DelegateCommand(() => DownloadResult());
            LoadSampleCommand = new ParameterCommand<int>((sampleId) => LoadSample(sampleId));
            GenerateSampleCommand = new DelegateCommand(() => GenerateSample());

            _dialogCoordinator = dialogCoordinator;

            LoadInitialFile();
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

        private void UploadFile()
        {
            var openDialog = new OpenFileDialog
            {
                Filter = "HTML Files|*.html",
                FilterIndex = 2,
                RestoreDirectory = true
            };
            if (openDialog.ShowDialog() == true)
            {
                Input = File.ReadAllText(openDialog.FileName);
            }
        }

        private async void Obfuscate()
        {
            var progressController = await _dialogCoordinator.ShowProgressAsync(this, "Obfuscating", "Obfuscating data...");

            progressController.SetIndeterminate();

            try
            {
                await ObfuscateData();
                await progressController.CloseAsync();
            }
            catch
            {
                await progressController.CloseAsync();
                await _dialogCoordinator.ShowMessageAsync(this, "Error", "Error occured, please try again");
            }
        }

        private async Task ObfuscateData()
        {
            await Task.Run(async () =>
            {
                var settings = new Settings()
                {
                    RecursionDepth = 4
                };

                var resultHolder = new ObfuscatorManager(Input).RunObfuscate(settings);

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

        private async void DownloadResult()
        {
            var saveDialog = new SaveFileDialog
            {
                FileName = $"Obfuscated result {DateTime.Now:dd-MM-yyyy HH_mm}",
                Filter = "HTML Files|*.html",
                FilterIndex = 2,
                RestoreDirectory = true
            };

            if (saveDialog.ShowDialog() == true)
            {
                File.WriteAllText(saveDialog.FileName, Output);
                await _dialogCoordinator.ShowMessageAsync(this, "File exported", "Obfuscated file successfully exported!");
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
            SamplesGenerator samplesGenerator = new SamplesGenerator();
            Input = samplesGenerator.DrawSample();
        }
    }
}
