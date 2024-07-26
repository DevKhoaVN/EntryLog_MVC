using System;
using System.Collections.Generic;

namespace TestMySql.Models;

public partial class Parent
{
    public int ParentId { get; set; }

    public string Name { get; set; } = null!;

    public string Email { get; set; } = null!;

    public int? Phone { get; set; }

    public string Address { get; set; } = null!;

    public virtual ICollection<Absentreport> Absentreports { get; set; } = new List<Absentreport>();

    public virtual ICollection<Student> Students { get; set; } = new List<Student>();
}
