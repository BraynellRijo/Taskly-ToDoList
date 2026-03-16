using Domain.ENUM;
using Domain.Interfaces.Helpers;

namespace DAL.PathHandler
{
    public class PersistencePathHandler : IPathHandler
    {
        public string GetPath(string fileName, FileType fileType)
        {
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string solutionDirectory = Directory.GetParent(baseDirectory)!
                .Parent!
                .Parent!
                .Parent!
                .Parent!
                .FullName;
            string fullPath = Path.Combine(solutionDirectory, 
                "DAL",
                "Persistence", 
                fileType.ToString(),
                $"{fileName}.{fileType}");
            return fullPath;
        }

        public void VerifyDirectory(string filePath)
        {
            string directoryPath = Path.GetDirectoryName(filePath)!;
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
        }
    }
}
