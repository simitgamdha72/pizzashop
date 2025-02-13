using System;
using System.Collections.Generic;

namespace pizzashop.Models;

public partial class OrderTax
{
    public int Id { get; set; }

    public int OrderId { get; set; }

    public int TaxId { get; set; }

    public int OrdersItemDetailsId { get; set; }

    public int ModifiedOrderDetailId { get; set; }

    public decimal TotalAmount { get; set; }

    public DateTime CreatedAt { get; set; }

    public int CreatedBy { get; set; }

    public DateTime? ModifiedAt { get; set; }

    public int? ModifiedBy { get; set; }

    public virtual User CreatedByNavigation { get; set; } = null!;

    public virtual ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();

    public virtual User? ModifiedByNavigation { get; set; }

    public virtual ModifiedOrderDetail ModifiedOrderDetail { get; set; } = null!;

    public virtual Order Order { get; set; } = null!;

    public virtual Ordersitemdetail OrdersItemDetails { get; set; } = null!;

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    public virtual TaxesFee Tax { get; set; } = null!;
}
