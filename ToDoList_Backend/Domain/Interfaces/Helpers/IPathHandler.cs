using Domain.ENUM;

namespace Domain.Interfaces.Helpers
{
    public interface IPathHandler
    {
        string GetPath(string fileName, FileType fileType);
        void VerifyDirectory(string path);
    }
}
