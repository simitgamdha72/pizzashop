using System;
using System.Collections.Generic;

namespace pizzashop.Models;

public partial class TaxesFee
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int Type { get; set; }

    public decimal TaxAmount { get; set; }

    public bool? IsEnable { get; set; }

    public bool? IsDefault { get; set; }

    public bool? Isdeleted { get; set; }

    public DateTime CreatedAt { get; set; }

    public int CreatedBy { get; set; }

    public DateTime? ModifiedAt { get; set; }

    public int? ModifiedBy { get; set; }

    public virtual User CreatedByNavigation { get; set; } = null!;

    public virtual User? ModifiedByNavigation { get; set; }

    public virtual ICollection<OrderTax> OrderTaxes { get; set; } = new List<OrderTax>();
}
