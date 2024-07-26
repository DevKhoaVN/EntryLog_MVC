using System;
using System.Collections.Generic;

namespace TestMySql.Models;

public partial class Absentreport
{
    public int AbsentReportId { get; set; }

    public int StudentId { get; set; }

    public int ParentId { get; set; }

    public DateTime CreateDay { get; set; }

    public string Reason { get; set; } = null!;

    public virtual Parent Parent { get; set; } = null!;

    public virtual Student Student { get; set; } = null!;
}
