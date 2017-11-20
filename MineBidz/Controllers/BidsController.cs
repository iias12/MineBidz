using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Domain;
using MineBidz.Filters;
using MineBidz.Models;
using MineBidz.Utility;
using WebMatrix.WebData;

namespace MineBidz.Controllers
{
    [Authorize]
    [InitializeSimpleMembership]
    public class BidsController : Controller
    {
        Repository repository = new Repository(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        //
        // GET: /Bids/

        public ActionResult Index()
        {
            var model = GetBidListModel(true);
            return View(model);
        }

        //
        // GET: /Bids/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /Bids/Create

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

            ViewBag.Countries = new SelectList(countries, "CountryCode", "CountryName");
            ViewBag.Provinces = new SelectList(provinces, "ProvinceStateCode", "ProvinceStateName");

            CreateBidViewModel model = new CreateBidViewModel();

            model.RequestInfo = new RequestInfoListViewModel()
            {
                BidEnd = request.BidInfo.BidEndDate.ToLongDateString(),
                Category = categoryList.FirstOrDefault(s => s.Id == request.CategoryId).Title,
                Country = countryList.Any(c => c.CountryCode == request.DeliveryInfo.CountryCode)?countryList.FirstOrDefault(c => c.CountryCode == request.DeliveryInfo.CountryCode).CountryName : String.Empty,
                RequestInfoId = request.Id,
                Location = provinceList.Any(p => p.CountryCode == request.DeliveryInfo.CountryCode && p.ProvinceStateCode == request.DeliveryInfo.ProvinceStateCode)?provinceList.FirstOrDefault(p => p.CountryCode == request.DeliveryInfo.CountryCode && p.ProvinceStateCode == request.DeliveryInfo.ProvinceStateCode).ProvinceStateName:String.Empty,
                RefNumber = String.Format("MBR#{0}", request.Id),
                Subcategory = subCategoryList.FirstOrDefault(s => s.Id == request.SubcategoryId).Title,
                Condition = request.ConditionList == null? "" :String.Join("/", request.ConditionList.Select(c => c.Name).ToArray()),
                FormName = requestFormList.FirstOrDefault(f => f.ClassName == request.ClassName).FormName
            };

            model.CompanyInfo = new ContactInfo();
            ViewBag.Message = null;

            return View(model);
        }

        //
        // POST: /Bids/Create

        [HttpPost]
        public ActionResult Create(CreateBidViewModel model)
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
                    FormName = requestFormList.FirstOrDefault(f => f.ClassName == request.ClassName).FormName
                };

                return View(model);
            }


            try
            {
                string fileName = SaveDocument(model);

                Bid bid = new Bid
                {
                    Amount = 0,
                    EngineeringDesign = fileName,
                    CompanyInfo = model.CompanyInfo,
                    Description = model.Description,
                    ReferenceNumber = model.ReferenceNumber,
                    RequestInfoId = model.RequestInfo.RequestInfoId,
                    UserId = WebSecurity.CurrentUserId
                };
                repository.SaveBid(bid);
                // TODO: Add insert logic here

                RequestInfo request = repository.GetRequestInfo(model.RequestInfo.RequestInfoId);

                try
                {
                    string body = "Your Bid Request got a bid! " + "<a href=\"" + Url.Content(String.Format("~/Documents/{0}", fileName)) + "\"  target=\"_blank\">Bid Document</a>";
                    string subject = "New Bid for " + "MBR#" + model.RequestInfo.RequestInfoId ;

                    body = "Your Bid Request got a bid! "; 
                    if (String.IsNullOrEmpty(fileName))
                    {
                        body += "No document provided";
                    }
                    else
                    {
                        body +="<a href=\"" + String.Format("http://minebidz.com/Documents/{0}", fileName) + "\"  target=\"_blank\">Bid Document</a>";
                    }
                    body +=  " bidder e-mail " + model.CompanyInfo.Email;
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

                return RedirectToAction("Index", "Home");
            }
            catch(Exception ex)
            {
                return View();
            }
        }

        //
        // GET: /Bids/Delete/5
        public ActionResult Delete(int id)
        {
            //delete logic here
            repository.DeleteBid(id);

            return RedirectToAction("Index");

            //List<BidListViewModel> model = GetBidListModel(true);
            //return View("Index", model);
        }

        //
        // GET: /Bids/Edit/5
        public ActionResult Edit(int id)
        {
            Bid bid = repository.GetBid(id);
            RequestInfo request = repository.GetRequestInfo(bid.RequestInfoId);
            IEnumerable<Country> countryList = repository.ListCountries();
            IEnumerable<ProvinceState> provinceList = repository.ListProvincesStates();
            IEnumerable<Category> categoryList = repository.ListCategory();
            IEnumerable<Subcategory> subCategoryList = repository.ListSubcategory();
            IEnumerable<RequestForm> requestFormList = repository.GetAllRequestForms();

            var countries = repository.ListCountries();
            var provinces = repository.ListProvincesStates(bid.CompanyInfo.CountryCode);

            SelectList countriesList = new SelectList(countries, "CountryCode", "CountryName");
            if (countriesList.Any(c => c.Value == bid.CompanyInfo.CountryCode))
            {
                countriesList.FirstOrDefault(c => c.Value == bid.CompanyInfo.CountryCode).Selected = true;
            }

            ViewBag.Countries = countriesList;
            ViewBag.Provinces = new SelectList(provinces, "ProvinceStateCode", "ProvinceStateName");

            EditBidViewModel model = new EditBidViewModel()
            {
                Id = id,
                Approved = bid.Approved,
                Amount = 0,
                CompanyInfo = bid.CompanyInfo,
                Description = bid.Description,
                EngineeringDesign = bid.EngineeringDesign,
                ReferenceNumber = bid.ReferenceNumber
            };

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
                FormName = requestFormList.FirstOrDefault(f => f.ClassName == request.ClassName).FormName
            };

            ViewBag.Message = null;

            return View(model);
        }

        public ActionResult Approve(int id)
        {
            //approve logic here
            repository.ApproveBid(id, true);
            var model = GetBidListModel(true);
            return View("Index", model);
        }

        public ActionResult Disapprove(int id)
        {
            //approve logic here
            repository.ApproveBid(id, false);
            var model = GetBidListModel(true);
            return View("Index", model);
        }

        //
        // POST: /Bids/Edit/5

        [HttpPost]
        public ActionResult Edit(EditBidViewModel model)
        {
            try
            {
                Bid bid = new Bid()
                    {
                        Accepted = false,
                        Amount = 0,
                        Approved = false,
                        Description = model.Description,
                        EngineeringDesign = "",
                        Id = model.Id,
                        ReferenceNumber = model.ReferenceNumber,
                        RequestInfoId = 0,
                        UserId = 0,
                        CompanyInfo = model.CompanyInfo
                    };
                // TODO: Add update logic here

                repository.UpdateBid(bid);

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // POST: /Bids/Delete/5

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        private string SaveDocument(CreateBidViewModel model)
        {

            try
            {
                if (model.EngineeringDesign != null && model.EngineeringDesign.ContentLength > 0)
                {
                    var fileName = String.Format("{0}{1}", Guid.NewGuid().ToString(), Path.GetExtension(model.EngineeringDesign.FileName));
                    var path = Path.Combine(Server.MapPath(Url.Content("~/Documents/")), fileName);
                    model.EngineeringDesign.SaveAs(path);
                    return fileName;
                }
                return null;
            }
            catch (Exception fileExc)
            {
                return null;
            }
        }

        private IEnumerable<BidListViewModel> GetBidListModel(bool admin)
        {
            IEnumerable<Bid> bidList = repository.ListBid(admin);
            //List<RequestInfo> requestList = repository.ListRequestInfoAdmin();

            List<BidListViewModel> model = bidList.Select(b => new BidListViewModel()
            {
                Id = b.Id,
                RequestInfoId = b.RequestInfoId,
                RefNumber = b.ReferenceNumber,
                RefNumberRequest = String.Format("MBR#{0}", b.RequestInfoId),
                DocumentInfo = b.EngineeringDesign,
                Approved = b.Approved,
                Accepted = b.Accepted,
                CompanyName = b.CompanyInfo.CompanyName,
                ContactName = b.CompanyInfo.ContactName,
                Email = b.CompanyInfo.Email,
                Phone = b.CompanyInfo.Phone,
                Description = b.Description
            }).OrderBy(b => b.RequestInfoId).ToList();
            return model;
        }

    }
}
