#nullable enable
using System;
using System.Collections.Generic;
using Codecool.CodecoolShop.Models;

namespace Codecool.CodecoolShop.Daos;

public interface IUserDao
{
    public HashSalt? GetPassword(string username);
    public Tuple<int, string> GetUserIdAndName(string username);
    public IEnumerable<string>? GetAllNames();
    public IEnumerable<string>? GetAllEmails();
    public void Add(User user);
}