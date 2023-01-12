#nullable enable
using System;
using System.Collections.Generic;
using Codecool.CodecoolShop.Models;

namespace Codecool.CodecoolShop.Daos;

public interface IUserDao
{
    public HashSalt? GetPassword(string username);
    public Tuple<int, string> GetUserIdAndName(string username);
    public string GetEmail(int? userId);
    public IEnumerable<string>? GetAllNames();
    public IEnumerable<string>? GetAllEmails();
    public void Add(User user);
    public void SaveCheckoutData(CheckoutModel checkoutData, int? userId);
    public void SavePaymentData(PaymentModel paymentData, int? userId);
    public CheckoutModel? GetCheckoutData(int? userId);
    public PaymentModel? GetPaymentData(int? userId);
}