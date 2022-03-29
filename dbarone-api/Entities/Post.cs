namespace dbarone_api.Entities;
using dbarone_api.Lib.Data;

public class Post : IEntity
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Slug { get; set; }
    public string Teaser { get; set; }
    public string Content { get; set; }
    public string Code { get; set; }
    public string Style { get; set; }
    public string Head { get; set; }
    public string PostType { get; set; }
    public int? ParentId { get; set; }
    public DateTime CreatedDt { get; set; }
    public string CreatedBy { get; set; }
    public DateTime UpdatedDt { get; set; }
    public string UpdatedBy { get; set; }

    public bool IsChild => this.ParentId != null;
}