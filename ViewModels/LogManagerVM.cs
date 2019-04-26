using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using LogManagerWPF.Annotations;
using LogManagerWPF.Services;
using Prism.Commands;

namespace LogManagerWPF.ViewModels
{
    public class LogManagerVM : INotifyPropertyChanged
    {        
        private SystemUtility SystemUtility = new SystemUtility();
        private FileHandler  FileHandler = new FileHandler();

        private string _validDirectory;
        private string _directory;
        private string _output;

        public string DirectoryLabel { get; set; }
        public string DirectoryInput
        {
            get => _directory;
            set
            {
                if (SystemUtility.IsValidDirectory(value))
                {
                    _validDirectory = value;
                    OnPropertyChanged(nameof(DirectoryInput));
                    DirectoryLabel = "Directory is valid";
                    OnPropertyChanged(nameof(DirectoryLabel));
                }
                else
                {
                    DirectoryLabel = "Directory is not valid";
                    OnPropertyChanged(nameof(DirectoryLabel));
                }
                _directory = value;
            }
        }
        public string Output
        {
            get => _output;
            set
            {
                _output = value;
                OnPropertyChanged(nameof(Output));
            }
        }
        public string FindFilesNamedInput { get; set; }
        public string FindFilesContainingInput { get; set; }
        public string TimeStampFilesInput { get; set; }

        public LogManagerVM()
        {
            SystemUtility = new SystemUtility();
        }

        private DelegateCommand _displayFilesCommand;
        private DelegateCommand _findFilesNamedCommand;
        private DelegateCommand _findFilesContainingCommand;
        private DelegateCommand _timeStampFilesCommand;
        private DelegateCommand _searchEmptyFilesCommand;
        private DelegateCommand _deleteEmptyFilesCommand;
        private DelegateCommand _exportOutputCommand;
        private DelegateCommand _clearOutputCommand;

        public DelegateCommand DisplayFilesCommand =>
            _displayFilesCommand ?? (_displayFilesCommand = new DelegateCommand(DoDisplayFiles));
        private void DoDisplayFiles()
        {
            Output = FileHandler.CreateOutput(FileHandler.GetFileNameList(_validDirectory));
        }

        public DelegateCommand FindFilesNamedCommand =>
            _findFilesNamedCommand ?? (_findFilesNamedCommand = new DelegateCommand(DoFindFilesNamed));
        private void DoFindFilesNamed()
        {
            Output = FileHandler.CreateOutput(FileHandler.FindFileByName(FileHandler.GetFileNameList(_validDirectory), SystemUtility.MultipleInputCheck(FindFilesNamedInput)));
        }

        public DelegateCommand FindFilesContainingCommand =>
            _findFilesContainingCommand ?? (_findFilesContainingCommand = new DelegateCommand(DoFindFilesContaining));
        private void DoFindFilesContaining()
        {
            Output = FileHandler.CreateOutput(FileHandler.FindFilesContaining(FileHandler.GetFileNameList(_validDirectory), SystemUtility.MultipleInputCheck(FindFilesContainingInput)));
        }

        public DelegateCommand TimeStampFilesCommand =>
            _timeStampFilesCommand ?? (_timeStampFilesCommand = new DelegateCommand(DoTimeStampFiles));
        private void DoTimeStampFiles()
        {
            Output = FileHandler.CreateOutput(FileHandler.AddTimeStamp(FileHandler.FindFileDirectory(FileHandler.GetFileDirectoryList(_validDirectory), SystemUtility.MultipleInputCheck(TimeStampFilesInput))));
        }

        public DelegateCommand SearchEmptyFilesCommand =>
            _searchEmptyFilesCommand ?? (_searchEmptyFilesCommand = new DelegateCommand(DoSearchEmptyFiles));
        private void DoSearchEmptyFiles()
        {
            Output = FileHandler.CreateOutput(FileHandler.RetrieveEmptyFiles(FileHandler.GetFileDirectoryList(_validDirectory)));
        }

        public DelegateCommand DeleteEmptyFilesCommand =>
            _deleteEmptyFilesCommand ?? (_deleteEmptyFilesCommand = new DelegateCommand(DoDeleteEmptyFiles));
        private void DoDeleteEmptyFiles()
        {
            List<string> emptyFileList = FileHandler.RetrieveEmptyFiles(FileHandler.GetFileDirectoryList(_validDirectory));
            List<string> emptyFilesRemovedList = new List<string>();
            foreach (var emptyFile in emptyFileList)
            {
                emptyFilesRemovedList.Add($"File Deleted: {Path.GetFileName(emptyFile)}");
            }
            if(emptyFilesRemovedList.Count == 0)
            {
                emptyFilesRemovedList.Add("No empty files to remove.");
            }
            FileHandler.RemoveEmptyFiles(emptyFileList);
            Output = FileHandler.CreateOutput(emptyFilesRemovedList);
        }

        public DelegateCommand ExportOutputCommand =>
            _exportOutputCommand ?? (_exportOutputCommand = new DelegateCommand(DoExportOutput));
        private void DoExportOutput()
        {
            FileHandler.ExportOutput(Output, _validDirectory);
        }

        public DelegateCommand ClearOutputCommand =>
            _clearOutputCommand ?? (_clearOutputCommand = new DelegateCommand(DoClearOutput));
        private void DoClearOutput()
        {
            FileHandler.ClearOutput();
        }


        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}