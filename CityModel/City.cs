using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CityModel;

[Table("City")]
public partial class City
{
    [Key]
    public int CityId { get; set; }

    [Unicode(false)]
    public string? CityName { get; set; }

    [InverseProperty("City")]
    public virtual ICollection<Park> Parks { get; set; } = new List<Park>();
}
