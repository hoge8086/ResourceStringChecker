namespace ResourceStringChecker
{
    public interface IResourceFileReader
    {
        string Read(string path, string resourceId);
    }
}