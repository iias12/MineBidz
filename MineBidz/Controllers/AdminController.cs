using Domain;
using MineBidz.Filters;
using MineBidz.Models;
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

        private List<RequestInfoListViewModel> GetModel()
        {
            List<RequestInfo> requestList = repository.ListRequestInfoAdmin();
            IEnumerable<Country> countryList = repository.ListCountries();
            IEnumerable<ProvinceState> provinceList = repository.ListProvincesStates();
            IEnumerable<Category> categoryList = repository.ListCategory();
            IEnumerable<Subcategory> subCategoryList = repository.ListSubcategory();
            IEnumerable<RequestForm> requestFormList = repository.ListForm();

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
                FormName = requestFormList.FirstOrDefault(f => f.ClassName == r.ClassName).FormName,
                BidName = r.BidInfo.BidName,
                BidStart = r.BidInfo.BidStartDate.ToShortDateString(),
                DocumentInfo = r.DocumentInfo,
                Approved = r.Approved,
                FormTitle = requestFormList.FirstOrDefault(f => f.ClassName == r.ClassName).Title
            }).OrderByDescending(r=>r.BidStart).ToList();
            return model;
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

    }
}
