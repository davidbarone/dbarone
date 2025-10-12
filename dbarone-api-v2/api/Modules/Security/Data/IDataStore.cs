public interface IDataStore
{
    object GetContext(bool create = false);
    int Next<T>() where T : Identifiable;
    int Insert<T>(T row) where T : Identifiable;
    void Update<T>(int id, T row) where T : Identifiable;
    void Delete<T>(int id) where T : Identifiable;
    IEnumerable<T> Get<T>() where T : Identifiable;
    T? GetById<T>(int id) where T : Identifiable;
}
