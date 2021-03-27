using System.IO;
using System.Text;

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

            return "";
        }

    }
}
