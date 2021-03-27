using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
namespace ResourceStringChecker
{
    public class SpecDocMetaInfoRepository : ISpecDocMetaInfoRepository
    {
        private static readonly string SchemaFolder = @"schema\";
        public List<SpecDoc> GetSpecList()
        {
            var files = Directory.GetFiles(SchemaFolder, "*.json", System.IO.SearchOption.AllDirectories);
            var list = new List<SpecDoc>();
            foreach(var file in files)
            {
                var repo = new JsonRepository();
                var info = repo.Load<SpecDocMetaInfo>(file);
                list.Add(new SpecDoc()
                    {
                        SheetNames = info.Sheets.Select(x => x.SheetName).ToList(),
                        SpecName = Path.GetFileNameWithoutExtension(file),
                    }
                );
            }
            return list;
        }

        public SpecDocMetaInfo GetMetaInfo(string specName)
        {
            var repo = new JsonRepository();
            return repo.Load<SpecDocMetaInfo>(SchemaFolder + specName + ".json");
        }
    }
}
