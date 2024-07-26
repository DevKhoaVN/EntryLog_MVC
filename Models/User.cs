using System;
using System.Collections.Generic;

namespace TestMySql.Models;

public partial class User
{
    public int UserId { get; set; }

    public string UserName { get; set; } = null!;

    public string Password { get; set; } = null!;

    public int? StudentId { get; set; }

    public int RoleId { get; set; }

    public virtual Userrole Role { get; set; } = null!;
}
