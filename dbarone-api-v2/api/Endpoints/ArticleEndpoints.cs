public static class ArticleEndpoints
{
    public static void RegisterArticleEndpoints(this IEndpointRouteBuilder routeBuilder)
    {
        routeBuilder.MapPost("/articles", (Article article, IDataStore dataStore) =>
        {
            var next = dataStore.Insert(article);
            return Results.Ok(next);
        })
        .WithOpenApi()
        .WithTags("Articles");

        routeBuilder.MapGet("/articles", (IDataStore dataStore) =>
        {
            var articles = dataStore.Get<Article>();
            return Results.Ok(articles);
        })
        .WithOpenApi()
        .WithTags("Articles");

        routeBuilder.MapPut("/articles/{id:int}", (int id, Article article, IDataStore dataStore) =>
        {
            dataStore.Update<Article>(id, article);
            return Results.Ok();
        })
        .WithOpenApi()
        .WithTags("Articles");

        routeBuilder.MapDelete("/articles/{id:int}", (int id, IDataStore dataStore) =>
        {
            dataStore.Delete<Article>(id);
            return Results.Ok();
        })
        .WithOpenApi()
        .WithTags("Articles");
    }
}