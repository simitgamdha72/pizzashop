using System;
using System.Collections.Generic;

namespace pizzashop.Models;

public partial class Ordersitemdetail
{
    public int Id { get; set; }

    public int ItemsId { get; set; }

    public int OrderId { get; set; }

    public decimal Itemtax { get; set; }

    public DateTime Createdat { get; set; }

    public DateTime? Modifiedat { get; set; }

    public bool? Isdeleted { get; set; }

    public string? Instruction { get; set; }

    public int? Itemquantity { get; set; }

    public decimal? Itemrate { get; set; }

    public int? Itemready { get; set; }

    public int CreatedBy { get; set; }

    public int? ModifiedBy { get; set; }

    public virtual User CreatedByNavigation { get; set; } = null!;

    public virtual MenuItem Items { get; set; } = null!;

    public virtual User? ModifiedByNavigation { get; set; }

    public virtual ICollection<ModifiedOrderDetail> ModifiedOrderDetails { get; set; } = new List<ModifiedOrderDetail>();

    public virtual Order Order { get; set; } = null!;

    public virtual ICollection<OrderTax> OrderTaxes { get; set; } = new List<OrderTax>();
}
