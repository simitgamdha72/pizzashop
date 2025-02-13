using System;
using System.Collections.Generic;

namespace pizzashop.Models;

public partial class Country
{
    public int Id { get; set; }

    public string Country1 { get; set; } = null!;

    public bool? Isdeleted { get; set; }

    public DateTime CreatedAt { get; set; }

    public int CreatedBy { get; set; }

    public DateTime? ModifiedAt { get; set; }

    public int? ModifiedBy { get; set; }

    public virtual User CreatedByNavigation { get; set; } = null!;

    public virtual User? ModifiedByNavigation { get; set; }

    public virtual ICollection<State> States { get; set; } = new List<State>();
}
