﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace AtonAPI.Database;

public partial class User
{
    public Guid Guid { get; set; }

    public string Login { get; set; }

    public string Password { get; set; }

    public string Name { get; set; }

    public int GenderId { get; set; }

    public DateTime? Birthday { get; set; }

    public bool Admin { get; set; }

    public DateTime CreatedOn { get; set; }

    public string CreatedBy { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public string ModifiedBy { get; set; }

    public DateTime? RevokedOn { get; set; }

    public string RevokedBy { get; set; }
}