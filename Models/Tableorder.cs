using System;
using System.Collections.Generic;

namespace pizzashop.Models;

public partial class Tableorder
{
    public int Id { get; set; }

    public int OrderId { get; set; }

    public int TableId { get; set; }

    public bool Isavailable { get; set; }

    public bool? Isdeleted { get; set; }

    public DateTime CreatedAt { get; set; }

    public int CreatedBy { get; set; }

    public DateTime? ModifiedAt { get; set; }

    public int? ModifiedBy { get; set; }

    public virtual User CreatedByNavigation { get; set; } = null!;

    public virtual User? ModifiedByNavigation { get; set; }

    public virtual Order Order { get; set; } = null!;

    public virtual Table Table { get; set; } = null!;
}
