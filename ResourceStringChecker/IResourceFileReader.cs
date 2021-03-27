namespace ResourceStringChecker
{
    public interface IResourceFileReaderFactory
    {
        IResourceFileReader Create(ResourceFile resourceFile);
    }
    public interface IResourceFileReader
    {
        string FindString(string resourceId);
    }
}