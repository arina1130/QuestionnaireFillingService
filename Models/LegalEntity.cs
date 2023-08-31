using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace QuestionnaireFillingService.Models;

public partial class LegalEntity
{
    public long? Id { get; set; }

    public long? IsLlc { get; set; } // тип организации (ООО - 0, ИП - 1)

    public string? Tin { get; set; } // ИНН

    public string? PathTin { get; set; } // путь к скану ИНН

    public string? Msrnie { get; set; } // ОГРНИП

    public string? PathMsrnie { get; set; } // путь к скану

    public string? Msrn { get; set; } // ОГРН

    public string? PathMsrn { get; set; } // путь к скану ОГРН

    public string? RegistrationDate { get; set; } // дата регистрации

    public string? PathUsrie { get; set; } // путь к скану выписки из ЕГРИП

    public string? PathRoomRental { get; set; } // путь к скану договора аренды помещения

    public long? NoContract { get; set; } // наличие договора (0 - нет, 1 - есть)

    public string? FullName { get; set; } // полное наименование ООО

    public string? Abbreviation { get; set; } // абревиатура полного наименования

    public string? Usrie { get; set; }
    [JsonIgnore]
    public virtual ICollection<User> Users { get; set; } = new List<User>();
    public LegalEntity() { }

    /// <summary>
    /// Конструктор для ИП
    /// </summary>
    public LegalEntity(string tin, string pathTin, string msrnie, string pathMsrnie, DateTime registrationDate, string pathUsrie, string pathRoomRental, long noContract = 0)
    {
        IsLlc = 0;
        Tin = tin;
        PathTin = pathTin;
        Msrnie = msrnie;
        PathMsrnie = pathMsrnie;
        RegistrationDate = registrationDate.ToString("dd.MM.yyyy");
        PathUsrie = pathUsrie;
        PathRoomRental = pathRoomRental;
        NoContract = noContract;
    }

    /// <summary>
    /// Конструктор для ООО
    /// </summary>
    public LegalEntity(string fullName, string abbreviation, DateTime registrationDate, string tin, string pathTin, string msrn, string pathMsrn, string pathUsrie, string pathRoomRental, long noContract = 0)
    {
        IsLlc = 1;
        FullName = fullName;
        Abbreviation = abbreviation;
        Tin = tin;
        PathTin = pathTin;
        Msrn = msrn;
        PathMsrn = pathMsrn;
        RegistrationDate = registrationDate.ToString("dd.MM.yyyy");
        PathUsrie = pathUsrie;
        PathRoomRental = pathRoomRental;
        NoContract = noContract;
    }
}
