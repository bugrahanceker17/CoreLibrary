﻿using CoreLibrary.Models.Abstract;
using CoreLibrary.Models.Concrete.Entities;

namespace CoreLibrary.Models.Concrete.DataTransferObjects;

public class RegisterResponse : IResponse
{
    public bool IsSuccess { get; set; }
    public string? Message { get; set; }
    public AppUser? User { get; set; }
}