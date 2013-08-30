using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Moriyama.Umbraco.Password.Classes.Interfaces;

namespace Moriyama.Umbraco.Password.Classes.Application
{
    public class FilePathService : IFilePathService
    {

        private static readonly FilePathService InternalInstance = new FilePathService();

        private FilePathService() { }

        public static FilePathService Instance
         {
            get 
            {
                return InternalInstance; 
            }
        }

        public string SanitisePath(string path, bool trailingSlash = true)
        {
            // replace back slash with forward
            path = path.Replace(@"\", "/");

            // replace double slash
            path = Regex.Replace(path, "/+", "/");

            var components = path.Split('/');

            // miss first element containing drive letter
            for(var i = 1; i<components.Length; i++)
            {
                components[i] = RemoveIllegalCharactersFromFileName(components[i]);
            }

            path = string.Join("/", components);

            if (trailingSlash && !path.EndsWith("/"))
            {
                path += "/";
            }

            return path;
        }

        public string DatePath(string basePath)
        {
            return DatePath(basePath, DateTime.Now);
        }

        public string DatePath(string basePath, DateTime date)
        {
            basePath = SanitisePath(basePath);
            var path = Path.Combine(basePath, date.Year.ToString(CultureInfo.InvariantCulture), date.Month.ToString(CultureInfo.InvariantCulture), date.Day.ToString(CultureInfo.InvariantCulture));
            path = SanitisePath(path);

            if (!Directory.Exists(path)) Directory.CreateDirectory(path);

            return path;
        }

        public string DeepFilePathFromFileName(string basePath, string filename, int depth)
        {
            basePath = SanitisePath(basePath);

            filename = filename.ToLower();
            filename = RemoveIllegalCharactersFromFileName(filename);

            var components = filename.ToCharArray();

            var a = 0;
            while (a <= depth && a < components.Length)
            {
                var letter = components[a];

                if (letter == '.') break;

                if (letter >= 'a' && letter <= 'z')
                {
                    basePath = Path.Combine(basePath, letter.ToString(CultureInfo.InvariantCulture));
                }
                a++;
            }

            basePath = SanitisePath(basePath);

            if (!Directory.Exists(basePath)) Directory.CreateDirectory(basePath);

            return basePath;
        }

        public string RemoveIllegalCharactersFromFileName(string filename)
        {
            return Path.GetInvalidFileNameChars().Aggregate(filename, (current, c) => current.Replace(c.ToString(CultureInfo.InvariantCulture), string.Empty));
        }

        public void EmptyDirectory(string path, bool deleteIfEmpty)
        {
            if (!Directory.Exists(path)) return;

            foreach (var file in Directory.GetFiles(path))
            {
                File.Delete(file);
            }

            if(deleteIfEmpty) Directory.Delete(path);
        }
    }
}
