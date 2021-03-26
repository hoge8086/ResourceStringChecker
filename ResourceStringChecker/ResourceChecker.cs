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
        IResourceFileReader resourceFileReader;
        ISpecDocMetaInfoRepository specDocMetaInfoRepository;

        public ResourceChecker(IResourceFileReader resourceFileReader, ISpecDocMetaInfoRepository specDocMetaInfoRepository)
        {
            this.resourceFileReader = resourceFileReader;
            this.specDocMetaInfoRepository = specDocMetaInfoRepository;
        }

        public List<CheckResult> Check(List<CheckTarget> targets)
        {
            var results = new List<CheckResult>();
            foreach (var target in targets)
            {
                var specMetaInfo = specDocMetaInfoRepository.GetMetaInfo(target.SpecName);
                var sheet = specMetaInfo.Sheets.FirstOrDefault(x => x.SheetName == target.SheetName);
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

                                var resourceFile = sheet.ResourceFiles.FirstOrDefault(x => x.Language == langColumn.Language);
                                var resourceString = resourceFileReader.Read(resourceFile.FilePath, resourceId);

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

    }

    public class ResourceStringConverter
    {
        static public string Convert(string specString)
        {
            return specString;
        }
    }
}
