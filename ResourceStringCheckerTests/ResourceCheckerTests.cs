using Microsoft.VisualStudio.TestTools.UnitTesting;
using ResourceStringChecker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResourceStringChecker.Tests
{
    public class MockResourceFileReaderFactory : IResourceFileReaderFactory
    {
        public IResourceFileReader Create(ResourceFile resourceFile)
        {
            return new MockResourceFileReader();
        }
    }
    public class MockResourceFileReader : IResourceFileReader
    {
        public MockResourceFileReader() { }
        public string FindString(string resourceId) { return "a"; }

    }
    public class MockSpecDocMetaInfoRepository : ISpecDocMetaInfoRepository
    {
        public SpecDocMetaInfo GetMetaInfo(string specName)
        {
            return new SpecDocMetaInfo()
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
        }

        public List<SpecDoc> GetSpecList()
        {
            throw new NotImplementedException();
        }
    }

    [TestClass()]
    public class ResourceCheckerTests
    {
        [TestMethod()]
        public void CheckTest()
        {
            var checker = new ResourceChecker(new MockResourceFileReaderFactory(), new MockSpecDocMetaInfoRepository());
            var results = checker.Check(new List<CheckTarget>() {
                new CheckTarget() { SpecName="spec", SheetName="sheet", CheckRows= new List<int> { 1, 2 } } }
            );
            //Assert.Fail();
        }
        [TestMethod()]
        public void CheckTest2()
        {
            var checker = new ResourceChecker(new ResourceFileReaderFactory(), new SpecDocMetaInfoRepository());
            var results = checker.Check(new List<CheckTarget>() {
                new CheckTarget() { SpecName="abc", SheetName="sheet", CheckRows= new List<int> { 2, 3 } } }
            );
            //Assert.Fail();
        }
    }
}