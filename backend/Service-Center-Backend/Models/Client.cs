using System;
using System.Collections.Generic;

namespace Service_Center_Backend.Models;

public partial class Client
{
    public int Id { get; set; }

    public string FullName { get; set; } = null!;

    public string PhoneNumber { get; set; } = null!;

    public string? Email { get; set; }

    public virtual ICollection<EquipmentHandoverReceipt> EquipmentHandoverReceipts { get; set; } = new List<EquipmentHandoverReceipt>();
}
