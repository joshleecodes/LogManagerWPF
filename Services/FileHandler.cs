using LogManagerWPF.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogManagerWPF.Services
{
    class FileHandler : IFileHandler
    {
        public List<string> GetFileDirectoryList(string directoryInput)
        {
            return Directory.EnumerateFiles(directoryInput).ToList();
        }

        public List<string> GetFileNameList(string directoryInput)
        {
            List<string> FileDirectoryList = GetFileDirectoryList(directoryInput);
            List<string> FileNameList = new List<string>();
            foreach (var file in FileDirectoryList)
            {
                FileNameList.Add(Path.GetFileNameWithoutExtension(file));
            }
            return FileNameList;
        }

        public List<string> FindFileByName(List<string> fileNameList, string[] fileNameInput)
        {
            List<string> NameMatchList = new List<string>();

            foreach (var fileName in fileNameList)
            {
                for (int i = 0; i < fileNameInput.Length; i++)
                {
                    if (fileName == fileNameInput[i])
                    {
                        NameMatchList.Add($"{fileNameInput[i]} found.");
                    }
                }
            }
            return NameMatchList;
        }

        public List<string> FindFilesContaining(List<string> fileList, string[] keywords)
        {
            List<string> KeywordMatchesList = new List<string>();
            foreach (var file in fileList) //loop through all fileList
            {
                string[] lines = File.ReadAllLines(file);
                for (int i = 0; i < lines.Length; i++) //For each line
                {
                    for (int j = 0; j < keywords.Length; j++) //change keyword on each iteration
                    {
                        if (lines[i].Contains(keywords[j])) //search through document for keyword
                        {
                            KeywordMatchesList.Add($"Keyword ({keywords[j]}) found in {Path.GetFileName(file)} at line {i}. Line content: {lines[i]}");
                        }
                    }
                }
            }
            return KeywordMatchesList;
        }

        public List<string> FindFileDirectory(List<string> fileList, string[] fileNames)
        {
            List<string> fileDirectories = new List<string>();
            foreach (var file in fileList)
            {
                for (int i = 0; i < fileNames.Length; i++)
                {
                    Console.WriteLine(Path.GetFileName(file));
                    if (fileNames[i] == Path.GetFileName(file))
                    {
                        fileDirectories.Add(Path.GetFullPath(file));
                    }
                }
            }

            return fileDirectories;
        }

        public List<string> AddTimeStamp(List<string> fileDirectoryList)
        {
            List<string> filesTimeStampedList = new List<string>();

            string currentTimeStamp = DateTime.Now.ToString();
            foreach (var fileDirectory in fileDirectoryList)
            {
                using (StreamWriter sw = File.AppendText(fileDirectory))
                {
                    sw.WriteLine("\nLast time stamped at " + currentTimeStamp);
                }
                filesTimeStampedList.Add($"The file found at {fileDirectory} has been time-stamped.");
            }
            return filesTimeStampedList;
        }

        public List<string> RetrieveEmptyFiles(List<string> fileList)
        {
            List<string> emptyFilesDirectoryList = new List<string>();
            foreach (var file in fileList) //Loop through fileList
            {
                string fileContent = File.ReadAllText(file);
                if (String.IsNullOrWhiteSpace(fileContent)) //Check if file is empty
                {
                    emptyFilesDirectoryList.Add(Path.GetFullPath(file));
                }
            }

            return emptyFilesDirectoryList;
        }

        public void RemoveEmptyFiles(List<string> fileList)
        {
            foreach (var file in fileList) //Loop through fileList
            {
                string fileContent = File.ReadAllText(file);
                if (String.IsNullOrWhiteSpace(fileContent)) //Check if file is empty
                {
                    //delete file if empty
                    Console.WriteLine($"Empty file found: {Path.GetFileName(file)} File has been deleted");
                    File.Delete(file);
                }
            }
        }

        public string CreateOutput(List<string> inputList)
        {
            string output = "";
            if (inputList.Any())
            {
                foreach (var line in inputList)
                {
                    output += "\n" + line;
                }
            }
            else
            {
                output += "\n" + "No fileList found.";
            }
            return output;
        }

        public void ExportOutput(string output, string directory)
        {
            System.IO.File.WriteAllText((directory + "\\OutputLog.txt"), output);
        }

        public string ClearOutput()
        {
            return "";
        }
    }
}
