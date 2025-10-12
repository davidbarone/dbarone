record Blob(
    int Id,
    string Filename,
    byte[] Data,
    string ContentType,
    bool IsPublished,
    DateTime CreatedDt,
    string CreatedBy
) : Identifiable
{
    public int Size => Data.Length;
}