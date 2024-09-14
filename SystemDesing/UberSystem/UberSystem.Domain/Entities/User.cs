using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace UberSystem.Domain.Entities;

public partial class User/*:IdentityUser<int>*/
{
    public long Id { get; set; }

    public int? Role { get; set; }

    public string? UserName { get; set; }

    public string Email { get; set; } = null!;

    public string? Password { get; set; }

    public virtual ICollection<Customer> Customers { get; } = new List<Customer>();

    public virtual ICollection<Driver> Drivers { get; } = new List<Driver>();
}
