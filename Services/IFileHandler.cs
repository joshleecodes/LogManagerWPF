using System.Collections.Generic;

namespace LogManagerWPF.Services
{
    public interface IFileHandler
    {
        List<string> GetFileDirectoryList(string directoryInput);

        List<string> GetFileNameList(string directoryInput);

        List<string> FindFileByName(List<string> fileList, string[] fileNameInput);

        List<string> FindFilesContaining(List<string> fileList, string[] keywordsInput);

        List<string> FindFileDirectory(List<string> fileList, string[] fileNamesInput);

        List<string> AddTimeStamp(List<string> fileDirectoryList);

        List<string> RetrieveEmptyFiles(List<string> fileList);

        void RemoveEmptyFiles(List<string> fileList);
    }
}
