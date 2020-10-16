﻿using FormulaObfuscator.Commands;
using MahApps.Metro.Controls.Dialogs;
using Microsoft.Win32;
using System;
using System.IO;
using System.Threading.Tasks;

namespace FormulaObfuscator.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private const string DefaultInputFile = "Input.html";
        private readonly IDialogCoordinator _dialogCoordinator;

        public DelegateCommand UploadCommand { get; set; }
        public DelegateCommand ObfuscateCommand { get; set; }
        public DelegateCommand DownloadCommand { get; set; }

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

            await ObfuscateData();   

            await progressController.CloseAsync();
        }

        private async Task ObfuscateData()
        {
            await Task.Run(() =>
            {
                // todo: use algorithm
                Output = Input;
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
    }
}
