using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using QuestionnaireFillingService.Models;
using System;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace QuestionnaireFillingService.Controllers
{
    [Route("api/")]
    [ApiController]
    public class ApiController : ControllerBase
    {
        DbServiceContext context = new DbServiceContext();
        [HttpPost, Route("load")]
        public async Task LoadFileAsync()
        {
            IFormCollection form = Request.Form;
            IFormFileCollection files = Request.Form.Files;
            var path = $"{Directory.GetCurrentDirectory()}/files";
            LegalEntity legalEntity;
            foreach (var file in files)
            {
                string fullPath = $"{path}/{file.FileName}";
                using (var fileStream = new FileStream(fullPath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }
            }
            string k = form["tin"].ToString();
            if (form["tin"].ToString().Length > 10)
            {
                legalEntity = new LegalEntity(form["tin"].ToString(), $"{form["tin"]}_pathTin", form["msrnie"].ToString(), $"{form["tin"]}_pathMsrnie", DateTime.Parse(form["registrationDate"].ToString()), $"{form["tin"]}_pathUsrie", $"{form["tin"]}_pathRoomRental", long.Parse(form["noContract"].ToString()));
            }
            else
            {
                legalEntity = new LegalEntity(form["fullName"].ToString(), form["abbreviation"].ToString(), DateTime.Parse(form["registrationDate"].ToString()), form["tin"].ToString(), $"{form["tin"]}_pathTin", form["msrn"].ToString(), $"{form["tin"]}_pathMsrn", $"{form["tin"]}_pathUsrie", $"{form["tin"]}_pathRoomRental", long.Parse(form["noContract"].ToString()));
            }
            HttpContext.Session.SetString("LegalEntity", JsonSerializer.Serialize(legalEntity));
        }


        [HttpPost, Route("find")]
        public IResult FindTin()
        {
            string tin = Request.Form["tin"].ToString();
            LegalEntity? legal = context.LegalEntities.FirstOrDefault(l => l.Tin == tin);
            if (legal != null)
            {
                Results.StatusCode(200);
                return Results.Json(legal);
            }
            else
            {
                LegalEntity ent = tin.Length>10 ? Parser.GetLegalEntityPersonal(tin):Parser.GetLegalEntityOrganization(tin);
                context.LegalEntities.Add(ent);
                context.SaveChanges();

                Results.StatusCode(200);
                return Results.Json(ent);
            }
        }

        [HttpPost, Route("findBank")]
        public IResult FindBicBic()
        {

            string bic = Request.Form["bic"].ToString();
            BankDetail? bank = context.BankDetails.FirstOrDefault(b=>b.Bic ==bic);
            if (bank != null)
            {
                Results.StatusCode(200);
                return Results.Json(bank);
            }
            else
            {
                bank =  Parser.GetBankDetail(bic);
                context.BankDetails.Add(bank);
                context.SaveChanges();

                Results.StatusCode(200);
                return Results.Json(bank);
            }
        }

        [HttpPost, Route("send")]
        public IResult SendBic()
        {
            IFormCollection form = HttpContext.Request.Form;
            try{
                var k = HttpContext.Session.GetString("LegalEntity");
                LegalEntity? legalEntity = JsonSerializer.Deserialize <LegalEntity> (json: HttpContext.Session.GetString("LegalEntity"));
                BankDetail bankDetail = new BankDetail(form["bic"].ToString(), form["bankName"].ToString(), form["correspondentAccount"].ToString(), form["checkingAccount"].ToString());
                BankDetail? bankOld = context.BankDetails.FirstOrDefault(b => b.Bic == bankDetail.Bic);
                LegalEntity? entityOld = context.LegalEntities.FirstOrDefault(b => b.Tin == legalEntity.Tin);
                if(bankOld!=null)
                {
                    bankOld.CheckingAccount = bankDetail.CheckingAccount;
                    context.BankDetails.Update(bankOld);
                }
                else
                {
                    bankOld = bankDetail;
                    context.BankDetails.Add(bankOld);


                }
                if (entityOld != null)
                {
                    entityOld.FullName = legalEntity.FullName;
                    entityOld.PathRoomRental = legalEntity.PathRoomRental;
                    entityOld.PathTin = legalEntity.PathTin;
                    entityOld.Abbreviation = legalEntity.Abbreviation;
                    entityOld.Msrn = legalEntity.Msrn;
                    entityOld.Msrnie = legalEntity.Msrnie;
                    entityOld.NoContract = legalEntity.NoContract;
                    entityOld.PathMsrn = legalEntity.PathMsrn;
                    entityOld.PathMsrnie = legalEntity.PathMsrnie;
                    entityOld.PathUsrie = legalEntity.PathUsrie;
                    entityOld.RegistrationDate = legalEntity.RegistrationDate;
                    context.LegalEntities.Update(entityOld);
                }
                else
                {
                    entityOld = legalEntity;
                    context.LegalEntities.Add(entityOld);
                }
                context.SaveChanges();
                context.Users.Add(new User(long.Parse(entityOld.Tin),entityOld.Id, bankOld.Id));
                context.SaveChanges();
                return Results.StatusCode(200);
            }
            catch (Exception e)
            {
                string s = e.Message;
                return Results.StatusCode(400);
            }
        }
    }
}
