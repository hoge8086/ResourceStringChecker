using System.Collections.Generic;

namespace ResourceStringChecker
{
    public class SpecDoc
    {
        public string SpecName;
        public List<string> SheetNames;
    }
    public interface ISpecDocMetaInfoRepository
    {

        List<SpecDoc> GetSpecList();
        
        SpecDocMetaInfo GetMetaInfo(string specName);
    }
}