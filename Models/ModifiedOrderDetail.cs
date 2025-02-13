using System;
using System.Collections.Generic;

namespace pizzashop.Models;

public partial class ModifiedOrderDetail
{
    public int Id { get; set; }

    public int OrdersItemDetailsId { get; set; }

    public int ModifiedName { get; set; }

    public decimal Modifiertax { get; set; }

    public bool? Isdeleted { get; set; }

    public int? Modifierquantity { get; set; }

    public decimal? ModifierRate { get; set; }

    public DateTime CreatedAt { get; set; }

    public int CreatedBy { get; set; }

    public DateTime? ModifiedAt { get; set; }

    public int? ModifiedBy { get; set; }

    public virtual User CreatedByNavigation { get; set; } = null!;

    public virtual ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();

    public virtual User? ModifiedByNavigation { get; set; }

    public virtual MenuModifier ModifiedNameNavigation { get; set; } = null!;

    public virtual ICollection<OrderTax> OrderTaxes { get; set; } = new List<OrderTax>();

    public virtual Ordersitemdetail OrdersItemDetails { get; set; } = null!;
}
