using System;
using System.Collections.Generic;

namespace pizzashop.Models;

public partial class MenuItem
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int CategoryId { get; set; }

    public int ItemType { get; set; }

    public decimal Rate { get; set; }

    public int Quantity { get; set; }

    public bool Available { get; set; }

    public bool DefaultTax { get; set; }

    public decimal? Taxpercentage { get; set; }

    public int? ShortCode { get; set; }

    public int UnitId { get; set; }

    public string? Description { get; set; }

    public byte[]? Image { get; set; }

    public bool? Isdeleted { get; set; }

    public DateTime CreatedAt { get; set; }

    public int CreatedBy { get; set; }

    public DateTime? ModifiedAt { get; set; }

    public int? ModifiedBy { get; set; }

    public virtual Category Category { get; set; } = null!;

    public virtual User CreatedByNavigation { get; set; } = null!;

    public virtual ICollection<Itemmodifier> Itemmodifiers { get; set; } = new List<Itemmodifier>();

    public virtual User? ModifiedByNavigation { get; set; }

    public virtual ICollection<Ordersitemdetail> Ordersitemdetails { get; set; } = new List<Ordersitemdetail>();

    public virtual Unit Unit { get; set; } = null!;
}
