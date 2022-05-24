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

            set => Set(ref _filePath, value);
        }

        #region Команды

        public ICommand CreateFileCommand { get; }

        private void OnCreateFileCommandExecute(object p)
        {
            Text = string.Empty;
            FilePath = null;
        }

        public ICommand SaveFileAsCommand { get; }

        private bool CanSaveFileAsCommandExecute(object p) => !string.IsNullOrWhiteSpace(Text);

        private async void OnSaveFileAsCommandExecuteAsync(object p)
        {
            var dialog = new SaveFileDialog
            {
                Title = "Saving a file...",
                Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*",
                FileName = "NewTextFile",
                DefaultExt = "*.txt",
                AddExtension = true,
                InitialDirectory = FilePath,
                RestoreDirectory = true
            };

            if (dialog.ShowDialog() != true)
            {
                return;
            }

            FilePath = dialog.FileName;

            using (var writer = new StreamWriter(new FileStream(FilePath, FileMode.Create, 
                       FileAccess.Write)))
            {
                await writer.WriteAsync(Text).ConfigureAwait(false);
            }
        }
        
        #endregion

        public MainWindowViewModel()
        {
            CreateFileCommand = new RelayCommand(OnCreateFileCommandExecute);
            SaveFileAsCommand = new RelayCommand(OnSaveFileAsCommandExecuteAsync, 
                CanSaveFileAsCommandExecute);
        }
    }
}
