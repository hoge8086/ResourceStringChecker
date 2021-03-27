using System.Runtime.Serialization.Json;
namespace ResourceStringChecker
{
    public class SpecDocMetaInfoRepository : ISpecDocMetaInfoRepository
    {
        public SpecDocMetaInfo GetMetaInfo(string specName)
        {
            var repo = new JsonRepository();
            return repo.Load<SpecDocMetaInfo>(specName);
        }
    }
}
