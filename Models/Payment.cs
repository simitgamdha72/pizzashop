using System;
using System.Collections.Generic;

namespace pizzashop.Models;

public partial class Payment
{
    public int Id { get; set; }

    public int TaxId { get; set; }

    public int InvoiceId { get; set; }

    public string PaymentMode { get; set; } = null!;

    public string Status { get; set; } = null!;

    public decimal Amount { get; set; }

    public DateTime CreatedAt { get; set; }

    public int CreatedBy { get; set; }

    public DateTime? ModifiedAt { get; set; }

    public int? ModifiedBy { get; set; }

    public virtual User CreatedByNavigation { get; set; } = null!;

    public virtual Invoice Invoice { get; set; } = null!;

    public virtual User? ModifiedByNavigation { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual OrderTax Tax { get; set; } = null!;
}
