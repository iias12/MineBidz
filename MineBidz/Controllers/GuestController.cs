using Domain;
using MineBidz.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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
            ViewBag.Months = new SelectList(new List<int> {1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 });
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
            return View("Confirmation", model);
        }

    }
}
