using Domain.ENUM;
using Domain.Interfaces;

namespace DAL.PathHandler
{
    public class PersistencePathHandler
    {
        public string GetPersistenceFilePath(string fileName, FileType fileType)
        {
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string dataDirectory = Directory.GetParent(baseDirectory)!
                .Parent!.Parent!.Parent!.FullName;
            string fullPath = Path.Combine(dataDirectory, 
                "Persistence", 
                fileType.ToString(),
                $"{fileName}.{fileType}");
            return fullPath;
        }

        public void VerifyDirectoryExists(string filePath)
        {
            string directoryPath = Path.GetDirectoryName(filePath)!;
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
        }
    }
}
