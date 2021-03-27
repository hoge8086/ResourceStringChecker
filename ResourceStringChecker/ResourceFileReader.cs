using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace ResourceStringChecker
{
    public class ResourceFileReaderFactory : IResourceFileReaderFactory
    {
        public IResourceFileReader Create(ResourceFile resourceFile)
        {
            return new ResourceFileReader(resourceFile);
        }
    }
    public class ResourceFileReader : IResourceFileReader
    {
        private string content;
        public ResourceFileReader(ResourceFile resource)
        {
            using (var sr = new StreamReader(resource.FilePath, Encoding.GetEncoding(resource.Encoding)))
            {
                content = sr.ReadToEnd();
            }
        }

        public string FindString(string resourceId)
        {
            var pos = content.IndexOf(resourceId);
            if (pos == -1)
                return null;

            //string targetText = content.Substring(pos);
            string targetText = content.Substring(0, pos);

            var matches = Regex.Matches(targetText, "\"[^\"]*\"");

            if (matches.Count == 0)
                return null;

            var lastMatched = matches[matches.Count - 1].Value;

            if (lastMatched.Length < 2)
                return null;

            return lastMatched.Substring(1, lastMatched.Length - 2); 
        }

    }
}
