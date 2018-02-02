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

        public ActionResult Index()
        {
            //ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";

            //List<RequestInfoListViewModel> model = new List<RequestInfoListViewModel>();

            IEnumerable<RequestInfo> requestList = repository.ListRequestInfo();
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
                Condition = String.Join("/", r.ConditionList.Select(c=>c.Name).ToArray()),
                FormName = requestFormList.FirstOrDefault(f => f.EquipmentId == r.ClassName).FormName,
                FormTitle = requestFormList.FirstOrDefault(f => f.EquipmentId == r.ClassName).Title,
                BidName = r.BidInfo.BidName,
                BidStart = r.BidInfo.BidStartDate.ToShortDateString()
            }).OrderByDescending(r=>r.BidStart).ToList();
            
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
