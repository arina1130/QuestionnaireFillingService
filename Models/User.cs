using System;
using System.Collections.Generic;

namespace QuestionnaireFillingService.Models;

public partial class User
{
    public long Id { get; set; }

    public long? IdLegalEntity { get; set; }

    public long? IdBankDetails { get; set; }

    public virtual BankDetail? IdBankDetailsNavigation { get; set; }

    public virtual LegalEntity? IdLegalEntityNavigation { get; set; }
    public User() { }
    public User(long id,long? idLegalEntity, long? idBankDetails)
    {
        Id=id;
        IdLegalEntity = idLegalEntity;
        IdBankDetails = idBankDetails;
    }
}
