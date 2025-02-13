using System;
using System.Collections.Generic;

namespace pizzashop.Models;

public partial class WaitingList
{
    public int Id { get; set; }

    public int TokenNo { get; set; }

    public TimeOnly WaitingTime { get; set; }

    public int CustomerId { get; set; }

    public DateTime CreatedAt { get; set; }

    public int CreatedBy { get; set; }

    public DateTime? ModifiedAt { get; set; }

    public int? ModifiedBy { get; set; }

    public virtual User CreatedByNavigation { get; set; } = null!;

    public virtual Customer Customer { get; set; } = null!;

    public virtual User? ModifiedByNavigation { get; set; }

    public virtual Token TokenNoNavigation { get; set; } = null!;
}
