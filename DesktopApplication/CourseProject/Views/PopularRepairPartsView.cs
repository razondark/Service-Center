using System;
using System.Collections.Generic;

namespace CourseProject.Views;

public partial class PopularRepairPartsView
{
    public string? Name { get; set; }

    public string? Manufacturer { get; set; }

    public decimal? Cost { get; set; }

    public long? UsesCount { get; set; }
}
