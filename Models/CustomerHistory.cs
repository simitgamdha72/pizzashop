using System;
using System.Collections.Generic;

namespace pizzashop.Models;

public partial class CustomerHistory
{
    public int Id { get; set; }

    public int CustomerId { get; set; }

    public DateTime ComingSince { get; set; }

    public decimal AverageBill { get; set; }

    public decimal MaxOrder { get; set; }

    public int Visits { get; set; }

    public DateTime CreatedAt { get; set; }

    public int CreatedBy { get; set; }

    public DateTime? ModifiedAt { get; set; }

    public int? ModifiedBy { get; set; }

    public virtual User CreatedByNavigation { get; set; } = null!;

    public virtual Customer Customer { get; set; } = null!;

    public virtual User? ModifiedByNavigation { get; set; }
}
