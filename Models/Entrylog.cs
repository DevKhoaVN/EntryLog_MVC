using System;
using System.Collections.Generic;

namespace TestMySql.Models;

public partial class Entrylog
{
    public int LogId { get; set; }

    public int StudentId { get; set; }

    public DateTime LogTime { get; set; }

    public string? Status { get; set; }

    public virtual Student Student { get; set; } = null!;
}
