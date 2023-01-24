﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace taskmanagerapi.Models.IdentityModels;

public partial class AspNetUserLogin
{
    public string LoginProvider { get; set; } = null!;

    public string ProviderKey { get; set; } = null!;

    public string? ProviderDisplayName { get; set; }
    [Key]
    public string UserId { get; set; } = null!;

    public virtual AspNetUser User { get; set; } = null!;
}
