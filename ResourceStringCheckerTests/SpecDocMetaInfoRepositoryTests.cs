using Microsoft.VisualStudio.TestTools.UnitTesting;
using ResourceStringChecker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResourceStringChecker.Tests
{
    [TestClass()]
    public class SpecDocMetaInfoRepositoryTests
    {
        [TestMethod()]
        public void GetMetaInfoTest()
        {

            var info = new SpecDocMetaInfo()
            {
                SpecDocPath = "test.xlsx",
                Sheets = new List<SpecDocSheet>()
                {
                    new SpecDocSheet()
                    {
                        TableHeaders = new List<TableHeader>()
                        {
                            new TableHeader()
                            {
                                LanguageColumns = new List<LanguageColumn>()
                                {
                                    new LanguageColumn() { Column = "A", Language = Language.English },
                                    new LanguageColumn() { Column = "B", Language = Language.Japanease},
                                },
                                ResourceIdColumn = "C",
                            },
                        },
                        ResourceFiles = new List<ResourceFile> {
                            new ResourceFile { Language = Language.Japanease , FilePath="resource_jp.txt", Encoding="shift-jis"},
                            new ResourceFile { Language = Language.English, FilePath="resource_en.txt", Encoding="shift-jis"},
                        },
                        SheetName = "sheet",
                    },
                },
            };

            var json = new JsonRepository();
            json.Save<SpecDocMetaInfo>(@"schema\abc.json", info);

            var repo = new SpecDocMetaInfoRepository();
            var loadInfo = repo.GetMetaInfo("abc.json");
        }
    }
}