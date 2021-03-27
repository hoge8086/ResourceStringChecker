using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OperateExcel;

namespace ResourceStringChecker
{
    public class CheckTarget
    {
        public string SpecName;
        public string SheetName;
        public List<int> CheckRows;
    }

    public class CheckResult
    {
        public string SpecName;
        public string SheetName;
        public int Row;
        public string SpecString;
        public string ResourceString;
        public bool Result;

        public CheckResult(
            string specName,
            string sheetName,
            int row,
            string specString,
            string resourceString)
        {
            SpecName = specName;
            SheetName = sheetName;
            Row = row;
            SpecString = specString;
            ResourceString = resourceString;
            Result = ResourceStringConverter.Convert(specString) == resourceString;
        }
    }

    public class ResourceChecker
    {
        IResourceFileReaderFactory resourceFileReaderFactory;
        ISpecDocMetaInfoRepository specDocMetaInfoRepository;

        public ResourceChecker(IResourceFileReaderFactory resourceFileReaderFactory, ISpecDocMetaInfoRepository specDocMetaInfoRepository)
        {
            this.resourceFileReaderFactory = resourceFileReaderFactory;
            this.specDocMetaInfoRepository = specDocMetaInfoRepository;
        }

        public List<CheckResult> Check(List<CheckTarget> targets)
        {
            var results = new List<CheckResult>();
            foreach (var target in targets)
            {
                var specMetaInfo = specDocMetaInfoRepository.GetMetaInfo(target.SpecName);
                var sheet = specMetaInfo.Sheets.FirstOrDefault(x => x.SheetName == target.SheetName);
                var resourceReaders = CreateResourceReaders(sheet.ResourceFiles);

                using (var excel = new OperateExcel.OperateExcel())
                {
                    excel.Open(specMetaInfo.SpecDocPath);
                    excel.SelectSheet(target.SheetName);
                    foreach (var row in target.CheckRows)
                    {
                        foreach (var header in sheet.TableHeaders)
                        {

                            var resourceId = excel.Read<string>(Cell.A1(row, header.ResourceIdColumn));

                            foreach (var langColumn in header.LanguageColumns)
                            {
                                var specString = excel.Read<string>(Cell.A1(row, langColumn.Column));
                                var resourceString = resourceReaders[langColumn.Language].FindString(resourceId);

                                results.Add(
                                    new CheckResult(
                                        target.SpecName,
                                        sheet.SheetName,
                                        row,
                                        resourceString,
                                        specString
                                    ));
                            }
                        }
                    }
                }
            }
            return results;
        }

        private Dictionary<Language, IResourceFileReader> CreateResourceReaders(List<ResourceFile> resourceFiles)
        {
            var resourceReaders = new Dictionary<Language, IResourceFileReader>();
            foreach (var resourceFile in resourceFiles)
            {
                var resourceFileReader = resourceFileReaderFactory.Create(resourceFile);
                resourceReaders.Add(resourceFile.Language, resourceFileReader);
            }

            return resourceReaders;
        }
    }

    public class ResourceStringConverter
    {
        static public string Convert(string specString)
        {
            return specString;
        }
    }
}
