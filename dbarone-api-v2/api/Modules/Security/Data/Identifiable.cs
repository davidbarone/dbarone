public abstract record Identifiable : IIdentifiable
{
    public abstract int Id { get; init; }
}
