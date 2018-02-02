using Domain;
using MineBidz.Filters;
using MineBidz.Models;
using MineBidz.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MineBidz.Controllers
{
    [Authorize]
    [InitializeSimpleMembership]
    public class AdminController : Controller
    {
        Repository repository = new Repository(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        //
        // GET: /Admin/

        public ActionResult Index()
        {
            List<RequestInfoListViewModel> model = GetModel();

            return View(model);
        }

        public ActionResult Quotes()
        {
            List<BidListViewModel> model = GetQuotesModel();

            return View(model);
        }

        private List<RequestInfoListViewModel> GetModel()
        {
            List<RequestInfo> requestList = repository.ListRequestInfoAdmin().ToList();
            IEnumerable<Country> countryList = repository.ListCountries();
            IEnumerable<ProvinceState> provinceList = repository.ListProvincesStates();
            IEnumerable<Category> categoryList = repository.ListCategory();
            IEnumerable<Subcategory> subCategoryList = repository.ListSubcategory();
            IEnumerable<RequestForm> requestFormList = repository.GetAllRequestForms();

            List<RequestInfoListViewModel> model = requestList.Select(r => new RequestInfoListViewModel()
            {
                BidEnd = r.BidInfo.BidEndDate.ToShortDateString(),
                Category = categoryList.FirstOrDefault(s => s.Id == r.CategoryId).Title,
                Country = countryList.Any(c => c.CountryCode == r.DeliveryInfo.CountryCode) ? countryList.FirstOrDefault(c => c.CountryCode == r.DeliveryInfo.CountryCode).CountryName : null,
                RequestInfoId = r.Id,
                Location = provinceList.Any(p => p.CountryCode == r.DeliveryInfo.CountryCode && p.ProvinceStateCode == r.DeliveryInfo.ProvinceStateCode) ? provinceList.FirstOrDefault(p => p.CountryCode == r.DeliveryInfo.CountryCode && p.ProvinceStateCode == r.DeliveryInfo.ProvinceStateCode).ProvinceStateName : null,
                RefNumber = String.Format("MBR#{0}", r.Id),
                Subcategory = subCategoryList.FirstOrDefault(s => s.Id == r.SubcategoryId).Title,
                Condition = String.Join("/", r.ConditionList.Select(c => c.Name).ToArray()),
                FormName = requestFormList.FirstOrDefault(f => f.EquipmentId == r.ClassName).FormName,
                BidName = r.BidInfo.BidName,
                BidStart = r.BidInfo.BidStartDate.ToShortDateString(),
                DocumentInfo = r.DocumentInfo,
                Approved = r.Approved,
                FormTitle = requestFormList.FirstOrDefault(f => f.EquipmentId == r.ClassName).Title
            }).OrderByDescending(r=>r.BidStart).ToList();
            return model;
        }

        private List<BidListViewModel> GetQuotesModel()
        {
            var requestList = repository.ListRequestInfoAdmin().ToList();
            var quoteList = repository.GetQuotesListAdmin().Where(q => requestList.Select(r => r.Id).Contains(q.RequestInfoId)).ToList();

            return quoteList.Select(q => new BidListViewModel
            {
                Accepted = q.Accepted,
                Approved = q.Approved,
                CompanyName = q.CompanyInfo.CompanyName,
                ContactName = q.CompanyInfo.ContactName,
                Description = q.Description,
                DocumentInfo = q.EngineeringDesign,
                Email = q.CompanyInfo.Email,
                Id = q.Id,
                Payment = new PaymentViewModel
                {
                    Amount = q.CcPayment.Amount,
                    City = q.CcPayment.City,
                    CountryCode = q.CcPayment.CountryCode,
                    CreditCardNumber = q.CcPayment.CreditCardNumber,
                    ExpirationMonth = q.CcPayment.ExpirationMonth,
                    ExpirationYear = q.CcPayment.ExpirationYear,
                    NameOnCard = q.CcPayment.NameOnCard,
                    PaymentId = q.CcPayment.PaymentId,
                    PostalCode = q.CcPayment.PostalCode,
                    Processed = q.CcPayment.Processed,
                    ProvinceStateCode = q.CcPayment.ProvinceStateCode,
                    SecurityCode = q.CcPayment.SecurityCode,
                    StreetAddress = q.CcPayment.StreetAddress
                },
                Phone = q.CompanyInfo.Phone,
                RefNumber = q.ReferenceNumber,
                RefNumberRequest = String.Format("MBR#{0}", q.RequestInfoId),
                //RefNumberRequest = requestList.FirstOrDefault(r => r.Id == q.RequestInfoId)..BidInfo..BidName,
                RequestDocumentInfo = requestList.FirstOrDefault(r => r.Id == q.RequestInfoId).DocumentInfo,
                RequestInfoId = q.RequestInfoId
            }).ToList();
        }

        public ActionResult Approve(int requestId, bool approved)
        {
            //approve logic here
            repository.ApproveBidRequest(requestId, !approved);
            List<RequestInfoListViewModel> model = GetModel();
            return View("Index", model);
        }

        public ActionResult Packages()
        {
            return View();
        }

        //
        // GET: /Admin/Edit/5
        [HttpPost]
        public ActionResult EditBid(EditBidViewModel bid)
        {

            return RedirectToAction("Bids");
        }

        //
        // POST: /Admin/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Admin/Delete/5

        public ActionResult Delete(int requestId)
        {
            //approve logic here
            repository.DeleteBidRequest(requestId);
            List<RequestInfoListViewModel> model = GetModel();
            return View("Index", model);
        }

        // GET: /Bids/Delete/5
        public ActionResult DeleteQuote(int quoteId)
        {
            //delete logic here
            repository.DeleteBid(quoteId);
            return RedirectToAction("Quotes");
        }

        public ActionResult ApproveQuote(int quoteId, bool approved, int requestId)
        {
            repository.ApproveBid(quoteId, !approved);
            
            if (!approved)
            {                    
                var body = String.Empty;
                var subject = "New Quote for " + "MBR#" + requestId;

                var request = repository.GetRequestInfo(requestId);
                var quote = repository.GetBid(quoteId);


                try
                {

                    body = "Your Request got a quote from " + quote.CompanyInfo.CompanyName;
                    if (String.IsNullOrEmpty(request.DocumentInfo))
                    {
                        body += "No document provided";
                    }
                    else
                    {
                        body += " <a href=\"" + String.Format("http://minebidz.com/Documents/{0}", quote.EngineeringDesign) + "\"  target=\"_blank\">Bid Document</a>";
                    }
                    body += " Vendor e-mail " + quote.CompanyInfo.Email;
                    body += " Vendor phone " + quote.CompanyInfo.Phone;
                    body += " Contact name " + quote.CompanyInfo.ContactName;
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

                body = "You Quote for " + "MBR#" + requestId + "  has bee approved and forwarded to the requestor";
                subject = "You Quote for " + "MBR#" + requestId + "  has been approved";

                if (request.VendorCanContact)
                {
                    body += " Requestor e-mail " + request.CompanyInfo.Email;
                    body += " Vendor phone " + request.CompanyInfo.Phone;
                    body += " Contact name " + request.CompanyInfo.ContactName;
                }
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
                    Utilities.SendMail(quote.CompanyInfo.Email, subject, body);
                }
                catch { }


            }

            var model = GetQuotesModel();
            return RedirectToAction("Quotes");
        }

        public ActionResult MarkPaymentProcessed(Guid paymentId, bool processed)
        {
            if (processed)
            {
                return RedirectToAction("Quotes");
            }
            //approve logic here
            repository.MarkPaymentProcessed(paymentId, processed);
            return RedirectToAction("Quotes");
        }

    }
}
