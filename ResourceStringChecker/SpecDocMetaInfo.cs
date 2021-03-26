using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResourceStringChecker
{
    public enum Language
    {
        Japanease,
        English,
    }
    public class SpecDocMetaInfo
    {
        public string SpecDocPath;
        public List<SpecDocSheet> Sheets; 
    }

    public class ResourceFile
    {
        public string FilePath;
        public Language Language;
        public string Encoding;
    }

    public class LanguageColumn
    {
        public Language Language;
        public string Column;
    }

    public class TableHeader
    {
        public List<LanguageColumn> LanguageColumns;
        public string ResourceIdColumn;
    }

    public class SpecDocSheet
    {
        public string SheetName;
        public List<TableHeader> TableHeaders;
        public List<ResourceFile> ResourceFiles;
    }
}
