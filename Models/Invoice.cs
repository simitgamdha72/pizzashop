using System;
using System.Collections.Generic;

namespace pizzashop.Models;

public partial class Invoice
{
    public int Id { get; set; }

    public int SectionId { get; set; }

    public int TableId { get; set; }

    public int OrderId { get; set; }

    public int OrderModified { get; set; }

    public int OrdertaxId { get; set; }

    public DateTime CreatedAt { get; set; }

    public int CreatedBy { get; set; }

    public DateTime? ModifiedAt { get; set; }

    public int? ModifiedBy { get; set; }

    public virtual User CreatedByNavigation { get; set; } = null!;

    public virtual User? ModifiedByNavigation { get; set; }

    public virtual Order Order { get; set; } = null!;

    public virtual ModifiedOrderDetail OrderModifiedNavigation { get; set; } = null!;

    public virtual OrderTax Ordertax { get; set; } = null!;

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    public virtual Section Section { get; set; } = null!;

    public virtual Table Table { get; set; } = null!;
}
