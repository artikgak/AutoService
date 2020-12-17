using System;
using System.IO;

namespace AutoService.Tools
{
    internal static class FileFolderHelper
    {
        private static readonly string AppDataPath =
                   Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

        internal static readonly string AppFolderPath =
            Path.Combine(AppDataPath, "AutoService");

        //   internal static readonly string StorageFilePath =
        //        Path.Combine(AppFolderPath, "Storage.cskma");

        internal static string CreateFile(string fileName)
        {
            string filePath = Path.Combine(AppFolderPath, fileName);
            if (!CreateFolderAndCheckFileExistance(filePath))
                File.Create(filePath);

            return filePath;
        }


        internal static bool CreateFolderAndCheckFileExistance(string filePath)
        {
            var file = new FileInfo(filePath);
            return file.CreateFolderAndCheckFileExistance();
        }

        internal static bool CreateFolderAndCheckFileExistance(this FileInfo file)
        {
            if (!file.Directory.Exists)
            {
                file.Directory.Create();
            }
            return file.Exists;
        }
    }
}
