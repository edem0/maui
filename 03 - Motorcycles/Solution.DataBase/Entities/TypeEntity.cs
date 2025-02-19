﻿namespace Solution.Database.Entities;

[Table("Type")]
[Index(nameof(Name), IsUnique = true)]
public class TypeEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public uint Id { get; set; }

    [Required]
    public string Name { get; set; }

    public virtual ICollection<MotorcycleEntity> Motorcycles { get; set; }
}
