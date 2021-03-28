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
        public string SpecName { get; set; }
        public string SheetName { get; set; }
        public int Row { get; set; }
        public string Error { get; set; }
        public string ResouceFilePath{ get; set; }
        public string SpecString { get; set; }
        public string ResourceString { get; set; }
        public bool Result { get; set; }

        public CheckResult(
            string specName,
            string sheetName,
            int row,
            string resouceFilePath,
            string error)
        {
            SpecName = specName;
            SheetName = sheetName;
            Row = row;
            ResouceFilePath = resouceFilePath;
            SpecString = "-";
            ResourceString = "-";
            Result = false;
            Error = error;
        }
        public CheckResult(
            string specName,
            string sheetName,
            int row,
            string resouceFilePath,
            string specString,
            string resourceString)
        {
            SpecName = specName;
            SheetName = sheetName;
            Row = row;
            ResouceFilePath = resouceFilePath;
            SpecString = specString;
            ResourceString = resourceString;

            var convSpecString = ResourceStringConverter.Convert(specString);
            int diffIndex;
            Result = StringCompare(convSpecString,resourceString, out diffIndex);
            if (Result)
                Error = "-";
            else
                Error = "外仕とリソースに差異があります.(" + diffIndex.ToString() + "文字目)";
        }

        static public bool StringCompare(string left, string right, out int diffIndex)
        {
            for(int i = 0; i<left.Length && i<right.Length; i++)
            {
                if(left[i] != right[i])
                {
                    diffIndex = i + 1;
                    return false;
                }

            }
            if(left.Length != right.Length)
            {
                diffIndex = Math.Min(left.Length, right.Length) + 1;
                return false;
            }

            diffIndex = 0;
            return true;
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

        class ResouceFileInfo
        {
            public IResourceFileReader Reader;
            public ResourceFile ResourceFile;
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
                            if(string.IsNullOrEmpty(resourceId))
                            {
                                results.Add(new CheckResult(
                                        target.SpecName,
                                        sheet.SheetName,
                                        row,
                                        "-",
                                        "外仕のリソースIDが空欄です."
                                    ));
                                continue;
                            }

                            foreach (var langColumn in header.LanguageColumns)
                            {
                                var specString = excel.Read<string>(Cell.A1(row, langColumn.Column));
                                var resouceFileInfo = resourceReaders[langColumn.Language];
                                var resourceString = resouceFileInfo.Reader.FindString(resourceId);

                                if(resourceString == null)
                                {
                                    results.Add(new CheckResult(
                                            target.SpecName,
                                            sheet.SheetName,
                                            row,
                                            resouceFileInfo.ResourceFile.FilePath,
                                            "リソースID(" + resourceId +")がリソースファイルに見つかりません."
                                        ));
                                    continue;
                                }

                                results.Add(
                                    new CheckResult(
                                        target.SpecName,
                                        sheet.SheetName,
                                        row,
                                        resouceFileInfo.ResourceFile.FilePath,
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

        private Dictionary<Language, ResouceFileInfo> CreateResourceReaders(List<ResourceFile> resourceFiles)
        {
            var resouceFileDic = new Dictionary<Language, ResouceFileInfo>();
            foreach (var resourceFile in resourceFiles)
            {
                var resourceFileReader = resourceFileReaderFactory.Create(resourceFile);

                resouceFileDic.Add(
                    resourceFile.Language,
                    new ResouceFileInfo() { Reader = resourceFileReader, ResourceFile = resourceFile }
                    );
            }

            return resouceFileDic;
        }
    }

    public class ResourceStringConverter
    {
        static public string Convert(string specString)
        {
            specString.Replace("\"\"", "\"");
            specString.Replace("\r\n", "\\n");
            specString.Replace("\n", "\\n");
            specString.Replace("\r", "\\n");
            return specString;
        }
    }
}
