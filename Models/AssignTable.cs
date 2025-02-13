﻿using System;
using System.Collections.Generic;

namespace pizzashop.Models;

public partial class AssignTable
{
    public int Id { get; set; }

    public int CustomerId { get; set; }

    public int SectionId { get; set; }

    public int TableId { get; set; }

    public DateTime CreatedAt { get; set; }

    public int CreatedBy { get; set; }

    public DateTime? ModifiedAt { get; set; }

    public int? ModifiedBy { get; set; }

    public virtual User CreatedByNavigation { get; set; } = null!;

    public virtual Customer Customer { get; set; } = null!;

    public virtual User? ModifiedByNavigation { get; set; }

    public virtual Section Section { get; set; } = null!;

    public virtual Table Table { get; set; } = null!;
}
