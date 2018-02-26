using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Domain;
using MineBidz.Models;
using MineBidz.Filters;

namespace MineBidz.Controllers
{
    [InitializeSimpleMembership]
    public class HomeController : Controller
    {
        Repository repository = new Repository(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);

        [HttpGet]
        public ActionResult Index()
        {
            //ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";

            //List<RequestInfoListViewModel> model = new List<RequestInfoListViewModel>();
            var filter = new RequestInfoFilter();

            IEnumerable<RequestInfo> requestList = repository.ListRequestInfo(filter);
            IEnumerable<Country> countryList = repository.ListCountries();
            IEnumerable<ProvinceState> provinceList = repository.ListProvincesStates();
            var categoryList = repository.ListCategory().ToList();
            var subCategoryList = repository.ListSubcategory();
            var requestFormList = repository.GetAllRequestForms();
            categoryList.Insert(0, new Category { Id = 0, Title = "-CATEGORY-" });


            var listModel = requestList.Select(r => new RequestInfoListViewModel()
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
                FormTitle = requestFormList.FirstOrDefault(f => f.EquipmentId == r.ClassName).Title,
                BidName = r.BidInfo.BidName,
                BidStart = r.BidInfo.BidStartDate.ToShortDateString()
            }).OrderByDescending(r => r.BidStart).ToList();

            var model = new RequestInfoFilteredListViewModel
            {
                RequestInfoList = listModel,
                Filters = new RequestInfoFilters
                {
                    Categories = new SelectList(categoryList, "Id", "Title"),
                    Countries = new SelectList(countryList, "CountryCode", "CountryName"),
                    StatesProvinces = new SelectList(new List<ProvinceState>(), "ProvinceStateCode", "ProvinceStateName"),
                    CategoryId = 0
                }
            };

            return View(model);
        }

        [HttpPost]
        public ActionResult Index(RequestInfoFilteredListViewModel model)
        {

            var filter = new RequestInfoFilter
            {
                CategoryId = model.Filters.CategoryId,
                CountryCode = model.Filters.CountryCode,
                StateProvinceCode = model.Filters.StateProvinceCode,
                Title = model.Filters.Title
            };

            IEnumerable<RequestInfo> requestList = repository.ListRequestInfo(filter);
            IEnumerable<Country> countryList = repository.ListCountries();
            IEnumerable<ProvinceState> provinceList = repository.ListProvincesStates();
            var categoryList = repository.ListCategory().ToList();
            IEnumerable<Subcategory> subCategoryList = repository.ListSubcategory();
            IEnumerable<RequestForm> requestFormList = repository.GetAllRequestForms();

            var listModel = requestList.Select(r => new RequestInfoListViewModel()
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
                FormTitle = requestFormList.FirstOrDefault(f => f.EquipmentId == r.ClassName).Title,
                BidName = r.BidInfo.BidName,
                BidStart = r.BidInfo.BidStartDate.ToShortDateString()
            }).OrderByDescending(r => r.BidStart).ToList();

            categoryList.Insert(0, new Category { Id = 0, Title = "-CATEGORY-" });
            model.RequestInfoList = listModel;
            model.Filters.Categories = new SelectList(categoryList, "Id", "Title");
            model.Filters.Countries = new SelectList(countryList, "CountryCode", "CountryName");
            model.Filters.StatesProvinces = new SelectList(provinceList.Where(p => p.CountryCode == model.Filters.CountryCode), "ProvinceStateCode", "ProvinceStateName");

            return View(model);
        }


        public ActionResult About()
        {
            ViewBag.Message = "About Mine Bidz";

            return View();
        }

        public ActionResult Overview()
        {
            ViewBag.Message = "Overview";

            return View();
        }

        public ActionResult Faq()
        {
            ViewBag.Message = "FAQ";

            return View();
        }

        public ActionResult Services()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }


        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Forms(int? categoryId)
        {
            ViewBag.Message = "Forms";

            RequestFormViewModel model = new RequestFormViewModel();

            if (categoryId.HasValue)
            {
                model.RequestForms = repository.GetRequestFormsByCategory(categoryId.Value);
                model.CategoryId = categoryId.Value;
                model.SubcategoryList = repository.ListSubcategory();
            }

            return View(model);
        }

    }
}
