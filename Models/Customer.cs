using System;
using System.Collections.Generic;

namespace pizzashop.Models;

public partial class Customer
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Email { get; set; } = null!;

    public int Phone { get; set; }

    public DateTime Date { get; set; }

    public int TotalOrder { get; set; }

    public bool? Isdeleted { get; set; }

    public DateTime CreatedAt { get; set; }

    public int CreatedBy { get; set; }

    public DateTime? ModifiedAt { get; set; }

    public int? ModifiedBy { get; set; }

    public virtual ICollection<AssignTable> AssignTables { get; set; } = new List<AssignTable>();

    public virtual User CreatedByNavigation { get; set; } = null!;

    public virtual ICollection<CustomerHistory> CustomerHistories { get; set; } = new List<CustomerHistory>();

    public virtual User? ModifiedByNavigation { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual ICollection<Token> Tokens { get; set; } = new List<Token>();

    public virtual ICollection<WaitingList> WaitingLists { get; set; } = new List<WaitingList>();
}
