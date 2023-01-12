#nullable enable
using System;
using System.Collections.Generic;
using Codecool.CodecoolShop.Models;

namespace Codecool.CodecoolShop.Daos;

public interface IAdminDao
{
    public HashSalt? GetPassword(string username);
    public Tuple<int, string> GetAdminIdAndName(string username);
    public IEnumerable<string>? GetAllNames();
    public IEnumerable<string>? GetAllEmails();
    public void Add(User user);
}