using System;
using System.Collections.Generic;

namespace Service_Center_Backend.Models;

public partial class ServiceWorkDto
{
    public int Id { get; set; }

    public int IdEquipmentHandoverReceipt { get; set; }

    public string Employee { get; set; } = null!;

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public decimal? Cost { get; set; }
}
