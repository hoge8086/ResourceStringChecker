using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace ResourceStringChecker
{
    public enum Language
    {
        Japanease,
        English,
    }
    [DataContract]
    public class SpecDocMetaInfo
    {
        [DataMember]
        public string SpecDocPath { get; set; }
        [DataMember]
        public List<SpecDocSheet> Sheets { get; set; } 
    }

    [DataContract]
    public class ResourceFile
    {
        [DataMember]
        public string FilePath;
        [DataMember]
        public Language Language;
        [DataMember]
        public string Encoding;
    }

    [DataContract]
    public class LanguageColumn
    {
        [DataMember]
        public Language Language;
        [DataMember]
        public string Column;
    }

    [DataContract]
    public class TableHeader
    {
        [DataMember]
        public List<LanguageColumn> LanguageColumns;
        [DataMember]
        public string ResourceIdColumn;
    }

    [DataContract]
    public class SpecDocSheet
    {
        [DataMember]
        public string SheetName;
        [DataMember]
        public List<TableHeader> TableHeaders;
        [DataMember]
        public List<ResourceFile> ResourceFiles;
    }
}
