﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CoreLibrary.Models.Abstract;
using CoreLibrary.Models.Concrete.Entities.Base;
using CoreLibrary.Utilities.Security.JWT;

namespace CoreLibrary.Models.Concrete.Entities;

[Table("AppUsers")]
public class AppUser : BaseEntity<Guid>, IEntity
{
    [StringLength(75)] public string FirstName { get; set; }
    [StringLength(75)] public string LastName { get; set; }
    [StringLength(20)] public string? PhoneNumber { get; set; }
    public bool? PhoneNumberConfirmed { get; set; }
    [StringLength(100)] public string? Email { get; set; }
    public bool? EmailConfirmed { get; set; }
    [StringLength(100)] public string? UserName { get; set; }
    public byte[]? PasswordHash { get; set; }
    public byte[]? PasswordSalt { get; set; }
    public string? RefreshTokenHash { get; set; }
    public string? RefreshTokenSalt { get; set; }
    public DateTimeOffset? RefreshTokenExpires { get; set; }
    public bool? LockoutEnabled { get; set; }
    public DateTimeOffset? LockoutEnd { get; set; }
}