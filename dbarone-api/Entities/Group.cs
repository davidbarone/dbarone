namespace dbarone_api.Entities;
using dbarone_api.Lib.Data;
using dbarone_api.Lib.Validation;

public class Group
{
    [Key(Order = 1)]
    [Sequence(Name = "GroupSeq")]
    public int Id { get; set; }

    public string Code { get; set; }

    [StringLengthValidator(Min = 1, Max = 20)]
    public string Desc { get; set; }
}