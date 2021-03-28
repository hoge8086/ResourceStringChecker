using System.Collections.Generic;
using System.IO;
using System.Linq;
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

        class ResourceString
        {
            public int Index;
            public string String;
        }
        List<ResourceString> resourceStrings;
        public ResourceFileReader(ResourceFile resource)
        {
            using (var sr = new StreamReader(resource.FilePath, Encoding.GetEncoding(resource.Encoding)))
            {
                content = sr.ReadToEnd();
            }

            resourceStrings = new List<ResourceString>();
            var matches = Regex.Matches(content, "\"(?<str>.*?)\"(?!\")");
            foreach(Match  m in matches)
            {
                resourceStrings.Add(new ResourceString()
                {
                    Index = m.Index,
                    String = m.Groups["str"].Value,
                });
            }

            resourceStrings.Sort((l, r) => l.Index - r.Index);

        }

        public string FindString(string resourceId)
        {
            var idList = resourceId.Split(new char[] { '.' });

            int pos = 0;
            for(int i=0; i<idList.Length; i++)
            {
                pos = content.IndexOf(idList[i], pos);
                if (pos == -1)
                    return null;
            }

            var found = resourceStrings.FindIndex(x => x.Index > pos);

            if(idList.Last().StartsWith("IDC"))
            {
                if (found <= 0)
                    return null;

                return resourceStrings[found - 1].String;

            }else
            {
                if (found == -1)
                    return null;

                return resourceStrings[found].String;
            }

            ////string targetText = content.Substring(pos);
            //string targetText = content.Substring(0, pos);

            //var matches = Regex.Matches(targetText, "\".*\"(?!\")");

            //if (matches.Count == 0)
            //    return null;

            //var lastMatched = matches[matches.Count - 1].Value;

            //if (lastMatched.Length < 2)
            //    return null;

            //return lastMatched.Substring(1, lastMatched.Length - 2); 
        }

    }
}
