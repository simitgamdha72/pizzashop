using System;
using System.Collections.Generic;

namespace pizzashop.Models;

public partial class MenuModifier
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int ModifierGroupId { get; set; }

    public string Unit { get; set; } = null!;

    public decimal Rate { get; set; }

    public int Quantity { get; set; }

    public int UnitId { get; set; }

    public string? Description { get; set; }

    public bool? Isdeleted { get; set; }

    public DateTime CreatedAt { get; set; }

    public int CreatedBy { get; set; }

    public DateTime? ModifiedAt { get; set; }

    public int? ModifiedBy { get; set; }

    public virtual User CreatedByNavigation { get; set; } = null!;

    public virtual ICollection<Itemmodifier> Itemmodifiers { get; set; } = new List<Itemmodifier>();

    public virtual User? ModifiedByNavigation { get; set; }

    public virtual ICollection<ModifiedOrderDetail> ModifiedOrderDetails { get; set; } = new List<ModifiedOrderDetail>();

    public virtual ModifiersGroup ModifierGroup { get; set; } = null!;

    public virtual Unit UnitNavigation { get; set; } = null!;
}
