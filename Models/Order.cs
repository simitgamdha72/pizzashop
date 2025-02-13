using System;
using System.Collections.Generic;

namespace pizzashop.Models;

public partial class Order
{
    public int Id { get; set; }

    public DateTime Date { get; set; }

    public int ItemsOrdered { get; set; }

    public int OrderType { get; set; }

    public int CustomerId { get; set; }

    public int? Status { get; set; }

    public int PaymentId { get; set; }

    public int? RatingId { get; set; }

    public string? SpecialInstruction { get; set; }

    public string? Comment { get; set; }

    public bool? Isdeleted { get; set; }

    public DateTime CreatedAt { get; set; }

    public int CreatedBy { get; set; }

    public DateTime? ModifiedAt { get; set; }

    public int? ModifiedBy { get; set; }

    public virtual User CreatedByNavigation { get; set; } = null!;

    public virtual Customer Customer { get; set; } = null!;

    public virtual ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();

    public virtual User? ModifiedByNavigation { get; set; }

    public virtual ICollection<OrderTax> OrderTaxes { get; set; } = new List<OrderTax>();

    public virtual ICollection<Ordersitemdetail> Ordersitemdetails { get; set; } = new List<Ordersitemdetail>();

    public virtual Payment Payment { get; set; } = null!;

    public virtual Rating? Rating { get; set; }

    public virtual ICollection<Rating> Ratings { get; set; } = new List<Rating>();

    public virtual ICollection<Tableorder> Tableorders { get; set; } = new List<Tableorder>();
}
