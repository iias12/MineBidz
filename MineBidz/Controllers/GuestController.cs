using Domain;
using MineBidz.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MineBidz.Utility;
using System.IO;

namespace MineBidz.Controllers
{
    public class GuestController : Controller
    {
        Repository repository = new Repository(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);

        [HttpGet]
        public ActionResult Create(int formId)
        {
            RequestInfo request = repository.GetRequestInfo(formId);
            IEnumerable<Country> countryList = repository.ListCountries();
            IEnumerable<ProvinceState> provinceList = repository.ListProvincesStates();
            IEnumerable<Category> categoryList = repository.ListCategory();
            IEnumerable<Subcategory> subCategoryList = repository.ListSubcategory();
            IEnumerable<RequestForm> requestFormList = repository.GetAllRequestForms();

            var countries = repository.ListCountries();
            var provinces = new List<ProvinceState>();
            var currentYear = DateTime.Now.Year;

            ViewBag.Countries = new SelectList(countries, "CountryCode", "CountryName");
            ViewBag.Provinces = new SelectList(provinces, "ProvinceStateCode", "ProvinceStateName");
            ViewBag.Months = new SelectList(new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 });
            ViewBag.Years = new SelectList(new List<int> { currentYear, currentYear + 1, currentYear + 2, currentYear + 3, currentYear + 4, currentYear + 5, currentYear + 6 });

            var model = new CreateBidGuestViewModel();

            model.RequestInfo = new RequestInfoListViewModel()
            {
                BidEnd = request.BidInfo.BidEndDate.ToLongDateString(),
                Category = categoryList.FirstOrDefault(s => s.Id == request.CategoryId).Title,
                Country = countryList.Any(c => c.CountryCode == request.DeliveryInfo.CountryCode) ? countryList.FirstOrDefault(c => c.CountryCode == request.DeliveryInfo.CountryCode).CountryName : String.Empty,
                RequestInfoId = request.Id,
                Location = provinceList.Any(p => p.CountryCode == request.DeliveryInfo.CountryCode && p.ProvinceStateCode == request.DeliveryInfo.ProvinceStateCode) ? provinceList.FirstOrDefault(p => p.CountryCode == request.DeliveryInfo.CountryCode && p.ProvinceStateCode == request.DeliveryInfo.ProvinceStateCode).ProvinceStateName : String.Empty,
                RefNumber = String.Format("MBR#{0}", request.Id),
                Subcategory = subCategoryList.FirstOrDefault(s => s.Id == request.SubcategoryId).Title,
                Condition = request.ConditionList == null ? "" : String.Join("/", request.ConditionList.Select(c => c.Name).ToArray()),
                FormName = requestFormList.FirstOrDefault(f => f.EquipmentId == request.ClassName).FormName,
                ClassName = request.ClassName,
                VendorCanContact = request.VendorCanContact
            };

            model.Payment = new PaymentViewModel
            {
                Amount = 19.95M
            };


            model.CompanyInfo = new ContactInfo();
            ViewBag.Message = null;

            return View(model);
        }

        [HttpPost]
        public ActionResult Create(CreateBidGuestViewModel model)
        {

            if (!model.Acknowledged)
            {
                ModelState.AddModelError("", "You must agree with terms and conditions");

                RequestInfo request = repository.GetRequestInfo(model.RequestInfo.RequestInfoId);
                IEnumerable<Country> countryList = repository.ListCountries();
                IEnumerable<ProvinceState> provinceList = repository.ListProvincesStates();
                IEnumerable<Category> categoryList = repository.ListCategory();
                IEnumerable<Subcategory> subCategoryList = repository.ListSubcategory();
                IEnumerable<RequestForm> requestFormList = repository.GetAllRequestForms();

                var countries = repository.ListCountries();
                var provinces = new List<ProvinceState>();

                ViewBag.Countries = new SelectList(countries, "CountryCode", "CountryName");
                ViewBag.Provinces = new SelectList(provinces, "ProvinceStateCode", "ProvinceStateName");

                model.RequestInfo = new RequestInfoListViewModel()
                {
                    BidEnd = request.BidInfo.BidEndDate.ToLongDateString(),
                    Category = categoryList.FirstOrDefault(s => s.Id == request.CategoryId).Title,
                    Country = countryList.Any(c => c.CountryCode == request.DeliveryInfo.CountryCode) ? countryList.FirstOrDefault(c => c.CountryCode == request.DeliveryInfo.CountryCode).CountryName : String.Empty,
                    RequestInfoId = request.Id,
                    Location = provinceList.Any(p => p.CountryCode == request.DeliveryInfo.CountryCode && p.ProvinceStateCode == request.DeliveryInfo.ProvinceStateCode) ? provinceList.FirstOrDefault(p => p.CountryCode == request.DeliveryInfo.CountryCode && p.ProvinceStateCode == request.DeliveryInfo.ProvinceStateCode).ProvinceStateName : String.Empty,
                    RefNumber = String.Format("MBR#{0}", request.Id),
                    Subcategory = subCategoryList.FirstOrDefault(s => s.Id == request.SubcategoryId).Title,
                    Condition = "New",
                    FormName = requestFormList.FirstOrDefault(f => f.EquipmentId == request.ClassName).FormName
                };

                return View(model);
            }

            try
            {
                var file = model.EngineeringDesign;
                string fileName = String.Empty;
                if (file != null)
                {
                    fileName = String.Format("{0}{1}", Guid.NewGuid().ToString(), Path.GetExtension(file.FileName));
                    var result = Utilities.SaveDocument(model.EngineeringDesign, Path.Combine(Server.MapPath(Url.Content("~/Documents/")), fileName));
                }

                Bid bid = new Bid
                {
                    UserId = 0,
                    Amount = 0,
                    EngineeringDesign = fileName,
                    CompanyInfo = model.CompanyInfo,
                    Description = model.Description,
                    ReferenceNumber = model.ReferenceNumber,
                    RequestInfoId = model.RequestInfo.RequestInfoId,
                    CcPayment = new CcPayment 
                    {
                        PaymentId = Guid.NewGuid(),
                        Amount = model.Payment.Amount,
                        CreditCardNumber = model.Payment.CreditCardNumber,
                        NameOnCard = model.Payment.NameOnCard,
                        ExpirationMonth = model.Payment.ExpirationMonth,
                        ExpirationYear = model.Payment.ExpirationYear,
                        SecurityCode = model.Payment.SecurityCode,
                        StreetAddress = model.Payment.StreetAddress,
                        City = model.Payment.City,
                        ProvinceStateCode = model.Payment.ProvinceStateCode,
                        CountryCode = model.Payment.CountryCode,
                        PostalCode = model.Payment.PostalCode
                    }
                };
                repository.SaveBid(bid);
                // TODO: Add insert logic here

                RequestInfo request = repository.GetRequestInfo(model.RequestInfo.RequestInfoId);

                try
                {
                    string body = "Your Bid Request got a bid! " + "<a href=\"" + Url.Content(String.Format("~/Documents/{0}", fileName)) + "\"  target=\"_blank\">Bid Document</a>";
                    string subject = "New Bid for " + "MBR#" + model.RequestInfo.RequestInfoId;

                    body = "Your Bid Request got a bid! ";
                    if (String.IsNullOrEmpty(fileName))
                    {
                        body += "No document provided";
                    }
                    else
                    {
                        body += "<a href=\"" + String.Format("http://minebidz.com/Documents/{0}", fileName) + "\"  target=\"_blank\">Bid Document</a>";
                    }
                    body += " bidder e-mail " + model.CompanyInfo.Email;
                    try
                    {
                        Utilities.SendMail("serguei.razykov@gmail.com", subject, body);
                    }
                    catch { }
                    try
                    {
                        Utilities.SendMail("info@minebidz.com", subject, body);
                    }
                    catch { }
                    try
                    {
                        Utilities.SendMail(request.CompanyInfo.Email, subject, body);
                    }
                    catch { }
                }
                catch
                {
                }

                return View("Confirmation", model);
            }
            catch (Exception ex)
            {
                return View();
            }

        }

    }
}
