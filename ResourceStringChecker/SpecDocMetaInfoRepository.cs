namespace ResourceStringChecker
{
    public class SpecDocMetaInfoRepository : ISpecDocMetaInfoRepository
    {
        public SpecDocMetaInfo GetMetaInfo(string specName)
        {
            return new SpecDocMetaInfo();
        }
    }
}
