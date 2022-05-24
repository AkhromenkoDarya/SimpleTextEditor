using SimpleTextEditor.ViewModels.Base;
using System.IO;

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

        private string _fileName;

        public string FileName
        {
            get => _fileName;

            set
            {
                if (Set(ref _fileName, value))
                {
                    ReadFileAsync(value);
                }
            }
        }

        private async void ReadFileAsync(string filePath)
        {
            Text = string.Empty;

            if (!File.Exists(filePath))
            {
                return;
            }

            using (StreamReader reader = File.OpenText(filePath))
            {
                Text = await reader.ReadToEndAsync().ConfigureAwait(true);
            }
        }
    }
}
