using System;
using System.Collections.Generic;

namespace pizzashop.Models;

public partial class Token
{
    public int Id { get; set; }

    public int CustomerId { get; set; }

    public decimal NoOfPeople { get; set; }

    public int SectionId { get; set; }

    public bool? Isdeleted { get; set; }

    public DateTime CreatedAt { get; set; }

    public int CreatedBy { get; set; }

    public DateTime? ModifiedAt { get; set; }

    public int? ModifiedBy { get; set; }

    public virtual User CreatedByNavigation { get; set; } = null!;

    public virtual Customer Customer { get; set; } = null!;

    public virtual User? ModifiedByNavigation { get; set; }

    public virtual Section Section { get; set; } = null!;

    public virtual ICollection<WaitingList> WaitingLists { get; set; } = new List<WaitingList>();
}
