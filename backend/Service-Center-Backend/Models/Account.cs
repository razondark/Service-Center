using System;
using System.Collections.Generic;

namespace Service_Center_Backend.Models;

public partial class Account
{
    public int Id { get; set; }

    public string Login { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string? Email { get; set; }

    public string Status { get; set; } = null!;
}
