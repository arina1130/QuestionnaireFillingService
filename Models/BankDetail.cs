using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace QuestionnaireFillingService.Models;

public partial class BankDetail
{
    public long Id { get; set; }

    public string? Bic { get; set; }

    public string? BankName { get; set; }

    public string? CheckingAccount { get; set; }

    public string? CorrespondentAccount { get; set; }
    [JsonIgnore]
    public virtual ICollection<User> Users { get; set; } = new List<User>();

    public BankDetail() { }

    /// <summary>
    /// Конструктор
    /// </summary>
    public BankDetail(string bic, string bankName, string correspondentAccount, string checkingAccount = "")
    {
        Bic = bic;
        BankName = bankName;
        CorrespondentAccount = correspondentAccount;
        CheckingAccount = checkingAccount;
    }
}
