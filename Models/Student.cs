using System;
using System.Collections.Generic;

namespace TestMySql.Models;

public partial class Student
{
    public int StudentId { get; set; }

    public int ParentId { get; set; }

    public string Name { get; set; } = null!;

    public string? Gender { get; set; }

    public DateTime DayOfBirth { get; set; }

    public string Class { get; set; } = null!;

    public string Address { get; set; } = null!;

    public int? Phone { get; set; }

    public DateTime JoinDay { get; set; }

    public virtual ICollection<Absentreport> Absentreports { get; set; } = new List<Absentreport>();

    public virtual ICollection<Alert> Alerts { get; set; } = new List<Alert>();

    public virtual ICollection<Entrylog> Entrylogs { get; set; } = new List<Entrylog>();

    public virtual Parent Parent { get; set; } = null!;
}
