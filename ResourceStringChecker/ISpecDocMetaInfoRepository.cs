namespace ResourceStringChecker
{
    public interface ISpecDocMetaInfoRepository
    {
        SpecDocMetaInfo GetMetaInfo(string specName);
    }
}