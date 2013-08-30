using System;

namespace Moriyama.Umbraco.Password.Classes.Interfaces
{
    public interface IFilePathService
    {
        string SanitisePath(string path, bool trailingSlash);

        string DatePath(string basePath);
        string DatePath(string basePath, DateTime date);

        string DeepFilePathFromFileName(string basePath, string path, int depth);

        string RemoveIllegalCharactersFromFileName(string filename);

        void EmptyDirectory(string path, bool deleteIfEmpty);

    }
}
