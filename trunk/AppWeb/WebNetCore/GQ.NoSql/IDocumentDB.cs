namespace GQ.NoSql
{
    public interface IDocumentDB
    {
        object GetCollection<T>() where T : IDocumentEntity;
        void DropCollection<T>() where T : IDocumentEntity;
    }
}
