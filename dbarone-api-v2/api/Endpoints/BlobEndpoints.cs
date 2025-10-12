public static class ResourceEndpoints
{
    public static void RegisterResourceEndpoints(this IEndpointRouteBuilder routeBuilder)
    {
        routeBuilder.MapPost("/blobs", (Blob blob, IDataStore dataStore) =>
        {
            var next = dataStore.Insert(blob);
            return Results.Ok(next);
        })
        .WithOpenApi()
        .WithTags("Blobs");

        routeBuilder.MapGet("/blobs", (IDataStore dataStore) =>
        {
            var blob = dataStore.Get<Blob>();
            return Results.Ok(blob);
        })
        .WithOpenApi()
        .WithTags("Blobs");

        routeBuilder.MapPut("/blobs/{id:int}", (int id, Blob blob, IDataStore dataStore) =>
        {
            dataStore.Update<Blob>(id, blob);
            return Results.Ok();
        })
        .WithOpenApi()
        .WithTags("Blobs");

        routeBuilder.MapDelete("/blobs/{id:int}", (int id, IDataStore dataStore) =>
        {
            dataStore.Delete<Blob>(id);
            return Results.Ok();
        })
        .WithOpenApi()
        .WithTags("Blobs");
    }
}