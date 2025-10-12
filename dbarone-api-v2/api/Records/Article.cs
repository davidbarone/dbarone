record Article(
    int Id,
    string Title,
    string? Slug,
    string? Teaser,
    string Content,
    string? Code,
    string? Style,
    string? Head,
    int? ParentId,
    DateTime CreatedDt,
    string CreatedBy,
    DateTime UpdatedDt,
    string UpdatedBy,
    PostType PostType,
    bool IsPublished
    ) : Identifiable
{
    public bool IsChild => this.ParentId is not null;
}