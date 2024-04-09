using System;
using System.Collections.Generic;

namespace Service_Center_Backend.Models;

public partial class EmployeeDto
{
    public int Id { get; set; }

    public string? FullName { get; set; }

    public string Passport { get; set; } = null!;

    public string PhoneNumber { get; set; } = null!;

    public string? Email { get; set; }

    public string Position { get; set; } = null!;
}
