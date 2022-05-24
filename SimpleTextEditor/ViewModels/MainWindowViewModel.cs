using System;
using Microsoft.Win32;
using SimpleTextEditor.Commands;
using SimpleTextEditor.ViewModels.Base;
using System.IO;
using System.Windows.Input;

namespace SimpleTextEditor.ViewModels
{
    internal class MainWindowViewModel : ViewModel
    {
        private string _text;

        public string Text
        {
            get => _text;

            set => Set(ref _text, value);
        }

        private string _filePath;

        public string FilePath
        {
            get => _filePath;

            set
            {
                if (Set(ref _filePath, value))
                {
                    ReadFileAsync(value);
                }
            }
        }

        private async void ReadFileAsync(string filePath)
        {
            if (!File.Exists(filePath))
            {
                return;
            }

            using (StreamReader reader = File.OpenText(filePath))
            {
                Text = await reader.ReadToEndAsync().ConfigureAwait(true);
            }
        }

        #region Команды

        public ICommand CreateFileCommand { get; }

        private void OnCreateFileCommandExecute(object p)
        {
            Text = string.Empty;
            FilePath = null;
        }

        public ICommand SaveFileCommand { get; }

        private bool CanSaveFileCommandExecute(object p) => !string.IsNullOrWhiteSpace(Text);

        private async void OnSaveFileCommandExecuteAsync(object p)
        {
            if (p is string filePath)
            {
                return;
            }

            var dialog = new SaveFileDialog
            {
                Title = "Saving a file...",
                Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*",
                FileName = string.Empty,
                DefaultExt = "*.txt",
                AddExtension = true,
                InitialDirectory = Environment.CurrentDirectory,
                RestoreDirectory = true
            };

            if (dialog.ShowDialog() != true)
            {
                return;
            }

            filePath = dialog.FileName;

            using (var writer = new StreamWriter(new FileStream(filePath, FileMode.Create,
                       FileAccess.Write)))
            {
                await writer.WriteAsync(Text).ConfigureAwait(false);
            }
        }
        
        #endregion

        public MainWindowViewModel()
        {
            CreateFileCommand = new RelayCommand(OnCreateFileCommandExecute);
            SaveFileCommand = new RelayCommand(OnSaveFileCommandExecuteAsync, 
                CanSaveFileCommandExecute);
        }
    }
}
