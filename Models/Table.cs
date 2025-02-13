using System;
using System.Collections.Generic;

namespace pizzashop.Models;

public partial class Table
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int SectionId { get; set; }

    public int Capacity { get; set; }

    public bool Status { get; set; }

    public bool? Isdeleted { get; set; }

    public DateTime CreatedAt { get; set; }

    public int CreatedBy { get; set; }

    public DateTime? ModifiedAt { get; set; }

    public int? ModifiedBy { get; set; }

    public virtual ICollection<AssignTable> AssignTables { get; set; } = new List<AssignTable>();

    public virtual User CreatedByNavigation { get; set; } = null!;

    public virtual ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();

    public virtual User? ModifiedByNavigation { get; set; }

    public virtual Section Section { get; set; } = null!;

    public virtual ICollection<Tableorder> Tableorders { get; set; } = new List<Tableorder>();
}
