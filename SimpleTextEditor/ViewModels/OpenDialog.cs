using System;
using System.IO;
using Microsoft.Win32;
using SimpleTextEditor.Commands;
using System.Windows;
using System.Windows.Input;

namespace SimpleTextEditor.ViewModels
{
    internal class OpenDialog : Freezable
    {
        public string Title
        {
            get => (string)GetValue(TitleProperty);

            set => SetValue(TitleProperty, value);
        }

        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register(
            nameof(Title),
            typeof(string),
            typeof(OpenDialog),
            new PropertyMetadata(default(string)));

        public string Filter
        {
            get => (string)GetValue(FilterProperty);

            set => SetValue(FilterProperty, value);
        }

        public static readonly DependencyProperty FilterProperty = DependencyProperty.Register(
            nameof(Filter),
            typeof(string),
            typeof(OpenDialog),
            new PropertyMetadata("Text Files (*.txt)|*.txt|All Files (*.*)|*.*"));

        public string SelectedFileName
        {
            get => (string)GetValue(SelectedFileNameProperty);

            set => SetValue(SelectedFileNameProperty, value);
        }

        public static readonly DependencyProperty SelectedFileNameProperty = DependencyProperty
            .Register(
                nameof(SelectedFileName), 
                typeof(string), 
                typeof(OpenDialog), 
                new PropertyMetadata(default(string)));

        public ICommand OpenFileCommand { get; }

        public OpenDialog()
        {
            OpenFileCommand = new RelayCommand(OnOpenFileCommandExecute);
        }

        private void OnOpenFileCommandExecute(object p)
        {
            var dialog = new OpenFileDialog
            {
                Title = Title,
                Filter = Filter,
                InitialDirectory = Environment.CurrentDirectory,
                RestoreDirectory = true
            };

            if (dialog.ShowDialog() != true)
            {
                return;
            }

            SelectedFileName = dialog.FileName;
        }

        protected override Freezable CreateInstanceCore()
        {
            return new OpenDialog();
        }
    }
}
