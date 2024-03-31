using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CityModel;

[Table("Park")]
public partial class Park
{
    [Key]
    public int ParkId { get; set; }

    [Unicode(false)]
    public string? ParkName { get; set; }

    [Unicode(false)]
    public string? Type { get; set; }

    [Column(TypeName = "decimal(18, 3)")]
    public decimal? Acres { get; set; }

    [Unicode(false)]
    public string? Address { get; set; }

    public int CityId { get; set; }

    [ForeignKey("CityId")]
    [InverseProperty("Parks")]
    public virtual City City { get; set; } = null!;
}
