using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Domain;
using Domain.Concrete;
using MineBidz.Filters;
using MineBidz.Models;
using MineBidz.Models.Concrete;
using MineBidz.Utility;
using Newtonsoft.Json;
using WebMatrix.WebData;

namespace MineBidz.Controllers
{
    [InitializeSimpleMembership]
    public class FormsController : Controller
    {
        Repository repository = new Repository(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);

        //
        // GET: /Forms/

        public ActionResult Index()
        {
            RequestForm form = new RequestForm();
            RequestInfo requestInfo = new RequestInfo();

            var countries = repository.ListCountries();
            var provincesCompany = new List<ProvinceState>();
            var provincesDelivery = new List<ProvinceState>();

            var categories = repository.ListCategory();
            var subcategories = repository.ListSubcategory();

            ViewBag.Countries = new SelectList(countries, "CountryCode", "CountryName");
            ViewBag.ProvincesCompany = new SelectList(provincesCompany, "ProvinceStateCode", "ProvinceStateName");
            ViewBag.ProvincesDelivery = new SelectList(provincesDelivery, "ProvinceStateCode", "ProvinceStateName");
            ViewBag.Categories = new SelectList(categories, "Id", "Title");

            CreateFormViewModel model = new CreateFormViewModel();
            SetEmptyModel(model);
            model.Categories = new SelectList(categories, "Id", "Title");
            model.Subcategories = new SelectList(subcategories, "Id", "Title");
            model.RequestForms = new SelectList(new List<RequestForm>(), "ClassName", "Title");

            return View(model);
        }

        //
        // GET: /Forms/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /Forms/Create

        public ActionResult Create(string formId, int? categoryId, int? subcategoryId, int? id)
        {
            RequestForm form = new RequestForm();
            RequestInfo requestInfo = new RequestInfo();

            var countries = repository.ListCountries();
            var provincesCompany = new List<ProvinceState>();
            var provincesDelivery = new List<ProvinceState>();

            if (id == null)
            {
                form = repository.GetForm(formId);
            }
            else
            {
                requestInfo = repository.GetRequestInfo(id.Value);
                form = repository.GetForm(requestInfo.ClassName);
                if (!String.IsNullOrEmpty(requestInfo.CompanyInfo.CountryCode))
                {
                    provincesCompany = repository.ListProvincesStates(requestInfo.CompanyInfo.CountryCode).ToList();
                }

                if (!String.IsNullOrEmpty(requestInfo.DeliveryInfo.CountryCode))
                {
                    provincesDelivery = repository.ListProvincesStates(requestInfo.DeliveryInfo.CountryCode).ToList();
                }
            }

            ViewBag.Countries = new SelectList(countries, "CountryCode", "CountryName");
            ViewBag.ProvincesCompany = new SelectList(provincesCompany, "ProvinceStateCode", "ProvinceStateName");
            ViewBag.ProvincesDelivery = new SelectList(provincesDelivery, "ProvinceStateCode", "ProvinceStateName");

            switch (form.FormName)
            {
                case "Adr":
                    CreateFormAdrViewModel adrModel = new CreateFormAdrViewModel();
                    adrModel.DetailsInfo = new Adr();
                    if (id == null)
                    {
                        SetEmptyModel(adrModel, categoryId, subcategoryId, formId, form);
                    }
                    else
                    {
                        SetModel(adrModel, requestInfo, form);
                        adrModel.DetailsInfo = Newtonsoft.Json.JsonConvert.DeserializeObject<Domain.Concrete.Adr>(requestInfo.DetailsInfoJson);
                    }
                    return View(String.Format("Create{0}", form.FormName), adrModel);
                case "Cip":
                    CreateFormCipViewModel cipModel = new CreateFormCipViewModel();
                    cipModel.DetailsInfo = new Cip();
                    if (id == null)
                    {
                        SetEmptyModel(cipModel, categoryId, subcategoryId, formId, form);
                    }
                    else
                    {
                        SetModel(cipModel, requestInfo, form);
                        cipModel.DetailsInfo = Newtonsoft.Json.JsonConvert.DeserializeObject<Domain.Concrete.Cip>(requestInfo.DetailsInfoJson);
                    }
                    return View(String.Format("Create{0}", form.FormName), cipModel);
                case "Classification":
                    CreateFormClassificationViewModel classificationModel = new CreateFormClassificationViewModel();
                    classificationModel.DetailsInfo = new Classification();
                    if (id == null)
                    {
                        SetEmptyModel(classificationModel, categoryId, subcategoryId, formId, form);
                    }
                    else
                    {
                        SetModel(classificationModel, requestInfo, form);
                        classificationModel.DetailsInfo = Newtonsoft.Json.JsonConvert.DeserializeObject<Domain.Concrete.Classification>(requestInfo.DetailsInfoJson);
                    }
                    return View(String.Format("Create{0}", form.FormName), classificationModel);
                case "Crushers":
                    CreateFormCrushersViewModel crushersModel = new CreateFormCrushersViewModel();
                    crushersModel.DetailsInfo = new Crushers();
                    if (id == null)
                    {
                        SetEmptyModel(crushersModel, categoryId, subcategoryId, formId, form);
                    }
                    else
                    {
                        SetModel(crushersModel, requestInfo, form);
                        crushersModel.DetailsInfo = Newtonsoft.Json.JsonConvert.DeserializeObject<Domain.Concrete.Crushers>(requestInfo.DetailsInfoJson);
                    }
                    return View(String.Format("Create{0}", form.FormName), crushersModel);
                case "Dewatering":
                    CreateFormDewateringViewModel dewateringModel = new CreateFormDewateringViewModel();
                    dewateringModel.DetailsInfo = new Dewatering();
                    if (id == null)
                    {
                        SetEmptyModel(dewateringModel, categoryId, subcategoryId, formId, form);
                    }
                    else
                    {
                        SetModel(dewateringModel, requestInfo, form);
                        dewateringModel.DetailsInfo = Newtonsoft.Json.JsonConvert.DeserializeObject<Domain.Concrete.Dewatering>(requestInfo.DetailsInfoJson);
                    }
                    return View(String.Format("Create{0}", form.FormName), dewateringModel);
                case "Excavator":
                    CreateFormExcavatorViewModel excavatorModel = new CreateFormExcavatorViewModel();
                    excavatorModel.DetailsInfo = new Excavator();
                    if (id == null)
                    {
                        SetEmptyModel(excavatorModel, categoryId, subcategoryId, formId, form);
                    }
                    else
                    {
                        SetModel(excavatorModel, requestInfo, form);
                        excavatorModel.DetailsInfo = Newtonsoft.Json.JsonConvert.DeserializeObject<Domain.Concrete.Excavator>(requestInfo.DetailsInfoJson);
                    }
                    return View(String.Format("Create{0}", form.FormName), excavatorModel);
                case "FilterCloths":
                    CreateFormFilterClothsViewModel filterClothsModel = new CreateFormFilterClothsViewModel();
                    filterClothsModel.DetailsInfo = new FilterCloths();
                    if (id == null)
                    {
                        SetEmptyModel(filterClothsModel, categoryId, subcategoryId, formId, form);
                    }
                    else
                    {
                        SetModel(filterClothsModel, requestInfo, form);
                        filterClothsModel.DetailsInfo = Newtonsoft.Json.JsonConvert.DeserializeObject<Domain.Concrete.FilterCloths>(requestInfo.DetailsInfoJson);
                    }
                    return View(String.Format("Create{0}", form.FormName), filterClothsModel);
                case "GenericECE":
                    CreateFormGenericECEViewModel genericECEModel = new CreateFormGenericECEViewModel();
                    genericECEModel.DetailsInfo = new GenericECE();
                    if (id == null)
                    {
                        SetEmptyModel(genericECEModel, categoryId, subcategoryId, formId, form);
                    }
                    else
                    {
                        SetModel(genericECEModel, requestInfo, form);
                        genericECEModel.DetailsInfo = Newtonsoft.Json.JsonConvert.DeserializeObject<Domain.Concrete.GenericECE>(requestInfo.DetailsInfoJson);
                    }
                    return View(String.Format("Create{0}", form.FormName), genericECEModel);
                case "GenericEngineering":
                    CreateFormGenericEngineeringViewModel genericEngineeringModel = new CreateFormGenericEngineeringViewModel();
                    genericEngineeringModel.DetailsInfo = new GenericEngineering();
                    if (id == null)
                    {
                        SetEmptyModel(genericEngineeringModel, categoryId, subcategoryId, formId, form);
                    }
                    else
                    {
                        SetModel(genericEngineeringModel, requestInfo, form);
                        genericEngineeringModel.DetailsInfo = Newtonsoft.Json.JsonConvert.DeserializeObject<Domain.Concrete.GenericEngineering>(requestInfo.DetailsInfoJson);
                    }
                    return View(String.Format("Create{0}", form.FormName), genericEngineeringModel);
                case "HaulTruck":
                    CreateFormHaulTruckViewModel haulTruckModel = new CreateFormHaulTruckViewModel();
                    haulTruckModel.DetailsInfo = new HaulTruck();
                    if (id == null)
                    {
                        SetEmptyModel(haulTruckModel, categoryId, subcategoryId, formId, form);
                    }
                    else
                    {
                        SetModel(haulTruckModel, requestInfo, form);
                        haulTruckModel.DetailsInfo = Newtonsoft.Json.JsonConvert.DeserializeObject<Domain.Concrete.HaulTruck>(requestInfo.DetailsInfoJson);
                    }
                    return View(String.Format("Create{0}", form.FormName), haulTruckModel);
                case "Loader":
                    CreateFormLoaderViewModel loaderModel = new CreateFormLoaderViewModel();
                    loaderModel.DetailsInfo = new Loader();
                    if (id == null)
                    {
                        SetEmptyModel(loaderModel, categoryId, subcategoryId, formId, form);
                    }
                    else
                    {
                        SetModel(loaderModel, requestInfo, form);
                        loaderModel.DetailsInfo = Newtonsoft.Json.JsonConvert.DeserializeObject<Domain.Concrete.Loader>(requestInfo.DetailsInfoJson);
                    }
                    return View(String.Format("Create{0}", form.FormName), loaderModel);
                case "Mill":
                    CreateFormMillViewModel millModel = new CreateFormMillViewModel();
                    millModel.DetailsInfo = new Mill();
                    if (id == null)
                    {
                        SetEmptyModel(millModel, categoryId, subcategoryId, formId, form);
                    }
                    else
                    {
                        SetModel(millModel, requestInfo, form);
                        millModel.DetailsInfo = Newtonsoft.Json.JsonConvert.DeserializeObject<Domain.Concrete.Mill>(requestInfo.DetailsInfoJson);
                    }
                    return View(String.Format("Create{0}", form.FormName), millModel);
                case "OtherConsumables":
                    CreateFormOtherConsumablesViewModel otherConsumablesModel = new CreateFormOtherConsumablesViewModel();
                    otherConsumablesModel.DetailsInfo = new OtherConsumables();
                    if (id == null)
                    {
                        SetEmptyModel(otherConsumablesModel, categoryId, subcategoryId, formId, form);
                    }
                    else
                    {
                        SetModel(otherConsumablesModel, requestInfo, form);
                        otherConsumablesModel.DetailsInfo = Newtonsoft.Json.JsonConvert.DeserializeObject<Domain.Concrete.OtherConsumables>(requestInfo.DetailsInfoJson);
                    }
                    return View(String.Format("Create{0}", form.FormName), otherConsumablesModel);
                case "OtherEquipment":
                    CreateFormOtherEquipmentViewModel otherEquipmentModel = new CreateFormOtherEquipmentViewModel();
                    otherEquipmentModel.DetailsInfo = new OtherEquipment();
                    if (id == null)
                    {
                        SetEmptyModel(otherEquipmentModel, categoryId, subcategoryId, formId, form);
                    }
                    else
                    {
                        SetModel(otherEquipmentModel, requestInfo, form);
                        otherEquipmentModel.DetailsInfo = Newtonsoft.Json.JsonConvert.DeserializeObject<Domain.Concrete.OtherEquipment>(requestInfo.DetailsInfoJson);
                    }
                    return View(String.Format("Create{0}", form.FormName), otherEquipmentModel);
                case "Pipe":
                    CreateFormPipeViewModel pipeModel = new CreateFormPipeViewModel();
                    pipeModel.DetailsInfo = new Pipe();
                    if (id == null)
                    {
                        SetEmptyModel(pipeModel, categoryId, subcategoryId, formId, form);
                    }
                    else
                    {
                        SetModel(pipeModel, requestInfo, form);
                        pipeModel.DetailsInfo = Newtonsoft.Json.JsonConvert.DeserializeObject<Domain.Concrete.Pipe>(requestInfo.DetailsInfoJson);
                    }
                    return View(String.Format("Create{0}", form.FormName), pipeModel);
                case "ProcessPlants":
                    CreateFormProcessPlantsViewModel processPlantsModel = new CreateFormProcessPlantsViewModel();
                    processPlantsModel.DetailsInfo = new ProcessPlants();
                    if (id == null)
                    {
                        SetEmptyModel(processPlantsModel, categoryId, subcategoryId, formId, form);
                    }
                    else
                    {
                        SetModel(processPlantsModel, requestInfo, form);
                        processPlantsModel.DetailsInfo = Newtonsoft.Json.JsonConvert.DeserializeObject<Domain.Concrete.ProcessPlants>(requestInfo.DetailsInfoJson);
                    }
                    return View(String.Format("Create{0}", form.FormName), processPlantsModel);
                case "Pump":
                    CreateFormPumpViewModel pumpModel = new CreateFormPumpViewModel();
                    pumpModel.DetailsInfo = new Pump();
                    if (id == null)
                    {
                        SetEmptyModel(pumpModel, categoryId, subcategoryId, formId, form);
                    }
                    else
                    {
                        SetModel(pumpModel, requestInfo, form);
                        pumpModel.DetailsInfo = Newtonsoft.Json.JsonConvert.DeserializeObject<Domain.Concrete.Pump>(requestInfo.DetailsInfoJson);
                    }
                    return View(String.Format("Create{0}", form.FormName), pumpModel);
                case "Screen":
                    CreateFormScreenViewModel screenModel = new CreateFormScreenViewModel();
                    screenModel.DetailsInfo = new Screen();
                    if (id == null)
                    {
                        SetEmptyModel(screenModel, categoryId, subcategoryId, formId, form);
                    }
                    else
                    {
                        SetModel(screenModel, requestInfo, form);
                        screenModel.DetailsInfo = Newtonsoft.Json.JsonConvert.DeserializeObject<Domain.Concrete.Screen>(requestInfo.DetailsInfoJson);
                    }
                    return View(String.Format("Create{0}", form.FormName), screenModel);
                case "Tractors":
                    CreateFormTractorsViewModel tractorsModel = new CreateFormTractorsViewModel();
                    tractorsModel.DetailsInfo = new Tractors();
                    if (id == null)
                    {
                        SetEmptyModel(tractorsModel, categoryId, subcategoryId, formId, form);
                    }
                    else
                    {
                        SetModel(tractorsModel, requestInfo, form);
                        tractorsModel.DetailsInfo = Newtonsoft.Json.JsonConvert.DeserializeObject<Domain.Concrete.Tractors>(requestInfo.DetailsInfoJson);
                    }
                    return View(String.Format("Create{0}", form.FormName), tractorsModel);
                case "Vehicle":
                    CreateFormVehicleViewModel Vehicle = new CreateFormVehicleViewModel();
                    Vehicle.DetailsInfo = new Vehicle();
                    if (id == null)
                    {
                        SetEmptyModel(Vehicle, categoryId, subcategoryId, formId, form);
                    }
                    else
                    {
                        SetModel(Vehicle, requestInfo, form);
                        Vehicle.DetailsInfo = Newtonsoft.Json.JsonConvert.DeserializeObject<Domain.Concrete.Vehicle>(requestInfo.DetailsInfoJson);
                    }
                    return View(String.Format("Create{0}", form.FormName), Vehicle);
                case "Ventilation":
                    CreateFormVentilationViewModel ventilationModel = new CreateFormVentilationViewModel();
                    ventilationModel.DetailsInfo = new Ventilation();
                    if (id == null)
                    {
                        SetEmptyModel(ventilationModel, categoryId, subcategoryId, formId, form);
                    }
                    else
                    {
                        SetModel(ventilationModel, requestInfo, form);
                        ventilationModel.DetailsInfo = Newtonsoft.Json.JsonConvert.DeserializeObject<Domain.Concrete.Ventilation>(requestInfo.DetailsInfoJson);
                    }
                    return View(String.Format("Create{0}", form.FormName), ventilationModel);

                default:
                    CreateFormViewModel model = new CreateFormViewModel();
                    SetEmptyModel(model, categoryId, subcategoryId, formId, form);
                    return View(String.Format("Create{0}", form.FormName), model);
            }
        }

        //
        // GET: /Forms/Edit/5

        public ActionResult Edit(int id)
        {
            RequestInfo requestInfo = repository.GetRequestInfo(id);
            RequestForm form = repository.GetForm(requestInfo.ClassName);

            var countries = repository.ListCountries();
            var provinces = new List<ProvinceState>();
            if (!String.IsNullOrEmpty(requestInfo.CompanyInfo.CountryCode))
            {
                provinces = repository.ListProvincesStates(requestInfo.CompanyInfo.CountryCode).ToList();
            }

            ViewBag.Countries = new SelectList(countries, "CountryCode", "CountryName");
            ViewBag.Provinces = new SelectList(provinces, "ProvinceStateCode", "ProvinceStateName");

            switch (form.FormName)
            {
                case "Mill":
                    CreateFormMillViewModel millModel = new CreateFormMillViewModel();
                    SetModel(millModel, requestInfo, form);
                    millModel.DetailsInfo = Newtonsoft.Json.JsonConvert.DeserializeObject<Domain.Concrete.Mill>(requestInfo.DetailsInfoJson);
                    return View(String.Format("Create{0}", form.FormName), millModel);
                case "OtherEquipment":
                    CreateFormOtherEquipmentViewModel otherEquipmentModel = new CreateFormOtherEquipmentViewModel();
                    SetModel(otherEquipmentModel, requestInfo, form);
                    otherEquipmentModel.DetailsInfo = Newtonsoft.Json.JsonConvert.DeserializeObject<Domain.Concrete.OtherEquipment>(requestInfo.DetailsInfoJson);
                    return View(String.Format("Create{0}", form.FormName), otherEquipmentModel);
                default:
                    return View("Index");
            }
        }


        public ActionResult Display(int formId)
        {
            //RequestInfo requestInfo = repository.GetRequestInfo(formId);
            //RequestForm form = repository.GetForm(requestInfo.ClassName);
            ////Type objType = typeof(Pump);
            ////// Print the qualified assembly name.
            ////string qName = objType.AssemblyQualifiedName.ToString();

            ////Type t = Type.GetType("Domain.Pump, Domain, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null");

            ////Type t = Type.GetType(String.Format("Domain.{0}, Domain", form.ClassName));

            ////Object o = Activator.CreateInstance(t);
            //CreateFormViewModel model = new CreateFormViewModel();
            //model.CompanyInfo = new ContactInfo();
            //model.DeliveryInfo = new ContactInfo();
            //model.BidInfo = new BidInfo();
            ////model.DetailsInfo = o;
            //return View(String.Format("Display{0}", form.FormName), model);

            RequestInfo requestInfo = repository.GetRequestInfo(formId);
            IEnumerable<Category> categoryList = repository.ListCategory();
            IEnumerable<Subcategory> subCategoryList = repository.ListSubcategory();
            IEnumerable<Country> countries = repository.ListCountries();
            IEnumerable<ProvinceState> provinces = repository.ListProvincesStates();
            RequestForm form = repository.GetForm(requestInfo.ClassName);

            string country = countries.Any(c => c.CountryCode == requestInfo.DeliveryInfo.CountryCode) ? countries.FirstOrDefault(c => c.CountryCode == requestInfo.DeliveryInfo.CountryCode).CountryName : "";

            DisplayBaseViewModel model = new DisplayBaseViewModel()
            {
                Category = categoryList.FirstOrDefault(c => c.Id == requestInfo.CategoryId).Title,
                Subcategory = subCategoryList.FirstOrDefault(s => s.Id == requestInfo.SubcategoryId).Title,
                Contry = country,
                Location = requestInfo.DeliveryInfo.ProvinceStateCode,
                FormTitle = form.Title,
                DetailsInfoJson = requestInfo.DetailsInfoJson,
                FormInfoId = formId,
                Description = requestInfo.Description,
                DocumentInfo = requestInfo.DocumentInfo
            };

            SetUserRights(model);

            return View(String.Format("Display/Display{0}", form.FormName), model);
        }

        //
        // POST: /Forms/Create

        [HttpPost]
        public ActionResult CreateADR(CreateFormAdrViewModel model)
        {            
            string details = JsonHelper.JsonSerialize<Adr>(model.DetailsInfo);
            return SaveRequest(model, details);
        }

        [HttpPost]
        public ActionResult CreateCIP(CreateFormCipViewModel model)
        {
            string details = JsonHelper.JsonSerialize<Cip>(model.DetailsInfo);
            return SaveRequest(model, details);
        }

        [HttpPost]
        public ActionResult CreateClassification(CreateFormClassificationViewModel model)
        {
            string details = JsonHelper.JsonSerialize<Classification>(model.DetailsInfo);
            return SaveRequest(model, details);
        }

        [HttpPost]
        public ActionResult CreateCrushers(CreateFormCrushersViewModel model)
        {
            string details = JsonHelper.JsonSerialize<Crushers>(model.DetailsInfo);
            return SaveRequest(model, details);
        }

        [HttpPost]
        public ActionResult CreateDewatering(CreateFormDewateringViewModel model)
        {
            string details = JsonHelper.JsonSerialize<Dewatering>(model.DetailsInfo);
            return SaveRequest(model, details);
        }

        [HttpPost]
        public ActionResult CreateExcavator(CreateFormExcavatorViewModel model)
        {
            string details = JsonHelper.JsonSerialize<Excavator>(model.DetailsInfo);
            return SaveRequest(model, details);
        }

        [HttpPost]
        public ActionResult CreateFilterCloths(CreateFormFilterClothsViewModel model)
        {
            string details = JsonHelper.JsonSerialize<FilterCloths>(model.DetailsInfo);
            return SaveRequest(model, details);
        }

        [HttpPost]
        public ActionResult CreateGenericECE(CreateFormGenericECEViewModel model)
        {
            string details = JsonHelper.JsonSerialize<GenericECE>(model.DetailsInfo);
            return SaveRequest(model, details);
        }

        [HttpPost]
        public ActionResult CreateGenericEngineering(CreateFormGenericEngineeringViewModel model)
        {
            string details = JsonHelper.JsonSerialize<GenericEngineering>(model.DetailsInfo);
            return SaveRequest(model, details);
        }

        [HttpPost]
        public ActionResult CreatePump(CreateFormPumpViewModel model)
        {
            string details = JsonHelper.JsonSerialize<Pump>(model.DetailsInfo);
            return SaveRequest(model, details);
        }

        [HttpPost]
        public ActionResult CreateLoader(CreateFormLoaderViewModel model)
        {
            string details = JsonHelper.JsonSerialize<Loader>(model.DetailsInfo);
            return SaveRequest(model, details);
        }

        [HttpPost]
        public ActionResult CreateMill(CreateFormMillViewModel model)
        {
            string details = JsonHelper.JsonSerialize<Mill>(model.DetailsInfo);
            return SaveRequest(model, details);
        }
        [HttpPost]
        public ActionResult CreateScreen(CreateFormScreenViewModel model)
        {
            string details = JsonHelper.JsonSerialize<Screen>(model.DetailsInfo);
            return SaveRequest(model, details);
        }

        [HttpPost]
        public ActionResult CreatePipe(CreateFormPipeViewModel model)
        {
            string details = JsonHelper.JsonSerialize<Pipe>(model.DetailsInfo);
            return SaveRequest(model, details);
        }

        [HttpPost]
        public ActionResult CreateProcessPlants(CreateFormProcessPlantsViewModel model)
        {
            string details = JsonHelper.JsonSerialize<ProcessPlants>(model.DetailsInfo);
            return SaveRequest(model, details);
        }

        [HttpPost]
        public ActionResult CreateOtherConsumables(CreateFormOtherConsumablesViewModel model)
        {
            string details = JsonHelper.JsonSerialize<OtherConsumables>(model.DetailsInfo);
            return SaveRequest(model, details);
        }

        [HttpPost]
        public ActionResult CreateOtherEquipment(CreateFormOtherEquipmentViewModel model)
        {
            string details = JsonHelper.JsonSerialize<OtherEquipment>(model.DetailsInfo);
            return SaveRequest(model, details);
        }

        [HttpPost]
        public ActionResult CreateVehicle(CreateFormVehicleViewModel model)
        {
            string details = JsonHelper.JsonSerialize<Vehicle>(model.DetailsInfo);
            return SaveRequest(model, details);
        }

        [HttpPost]
        public ActionResult CreateTractors(CreateFormTractorsViewModel model)
        {
            string details = JsonHelper.JsonSerialize<Tractors>(model.DetailsInfo);
            return SaveRequest(model, details);
        }

        [HttpPost]
        public ActionResult Create(CreateFormViewModel model)
        {
            var requestForm = Request.Form;
            var keys = requestForm.AllKeys.Where(x => x.StartsWith("DetailsInfo")).ToList();
            var form = repository.GetForm(model.ClassName);

            if (form.FormName == "Ventilation")
            {
                var myClass = MapForm<Ventilation>(requestForm);
                string details = JsonHelper.JsonSerialize<Ventilation>(myClass);

            }
            //
            return SaveRequest(null, "");
        }

        private T MapForm<T>(System.Collections.Specialized.NameValueCollection requestForm) where T : new()
        {
            var myClass = new T();
            var hhh = myClass.GetType();
            var props = hhh.GetProperties();
            foreach (var prop in props)
            {
                var key = requestForm.AllKeys.FirstOrDefault(x => x.Contains(prop.Name));
                if (key == null)
                {
                    continue;
                }
                var value = requestForm[key];
                if (prop.PropertyType.Name == "Boolean")
                {
                    prop.SetValue(myClass, bool.Parse(value));
                    continue;
                }
                if (prop.PropertyType.Name == "Int32")
                {
                    prop.SetValue(myClass,(value == null || value == String.Empty)? 0 : Int32.Parse(value));
                    continue;
                }
                if (prop.PropertyType.Name == "Double")
                {
                    prop.SetValue(myClass, (value == null || value == String.Empty) ? 0D : Double.Parse(value));
                    continue;
                }

                if (prop.PropertyType.Name == "Motor")
                {
                    var subClass = MapForm<Domain.Concrete.Common.Motor>(requestForm);
                    prop.SetValue(myClass, subClass);
                    continue;
                }
                if (prop.PropertyType.Name == "Product")
                {
                    var subClass = MapForm<Domain.Concrete.Common.Product>(requestForm);
                    prop.SetValue(myClass, subClass);
                    continue;
                }
                if (prop.PropertyType.Name == "TankScreenCommon")
                {
                    var subClass = MapForm<Domain.Concrete.Common.TankScreenCommon>(requestForm);
                    prop.SetValue(myClass, subClass);
                    continue;
                }
                prop.SetValue(myClass, value);
            }
            return myClass;
        }


        [HttpPost]
        public ActionResult CreateHaulTruck(CreateFormHaulTruckViewModel model)
        {
            string details = JsonHelper.JsonSerialize<HaulTruck>(model.DetailsInfo);
            return SaveRequest(model, details);
        }

        [HttpPost]
        public ActionResult CreateVentilation(CreateFormVentilationViewModel model)
        {
            string details = JsonHelper.JsonSerialize<Ventilation>(model.DetailsInfo);
            return SaveRequest(model, details);
        }

        private void SetModel(CreateFormViewModel model, RequestInfo requestInfo, RequestForm form)
        {
            model.CompanyInfo = requestInfo.CompanyInfo;
            model.DeliveryInfo = requestInfo.DeliveryInfo;
            model.BidInfo = requestInfo.BidInfo;
            model.CategoryId = requestInfo.CategoryId;
            model.SubCategoryId = requestInfo.SubcategoryId;
            model.Repository = repository;
            model.ClassName = requestInfo.ClassName;
            model.FormTitle = form.Title;
            model.FormName = form.FormName;
            model.RequestInfoId = requestInfo.Id;
            model.New = requestInfo.ConditionList.Any(c => c.Id == (int)ConditionEnum.New);
            model.Rental = requestInfo.ConditionList.Any(c => c.Id == (int)ConditionEnum.Rental);
            model.Used = requestInfo.ConditionList.Any(c => c.Id == (int)ConditionEnum.Used);
            model.Description = requestInfo.Description;
        }

        private void SetEmptyModel(CreateFormViewModel model, int? categoryId, int? subcategoryId, string formId, RequestForm form)
        {
            model.CompanyInfo = new ContactInfo();
            model.DeliveryInfo = new ContactInfo();
            model.BidInfo = new BidInfo();
            model.CategoryId = categoryId.Value;
            model.SubCategoryId = subcategoryId.Value;
            model.Repository = repository;
            model.ClassName = formId;
            model.FormTitle = form.Title;
            model.FormName = form.FormName;
        }

        private void SetEmptyModel(CreateFormViewModel model)
        {
            model.CompanyInfo = new ContactInfo();
            model.DeliveryInfo = new ContactInfo();
            model.BidInfo = new BidInfo();
            model.Repository = repository;
        }


        //
        // POST: /Forms/Edit/5

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
        // GET: /Forms/Delete/5

        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /Forms/Delete/5

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

        public JsonResult ProvincesStates(string countryCode)
        {
            var provincesStates = repository.ListProvincesStates(countryCode);
            return Json(provincesStates, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetRequestForms(int categoryId, int subcategoryId)
        {
            var requests = repository.GetRequestForms(categoryId, subcategoryId);
            return Json(requests, JsonRequestBehavior.AllowGet);
        }



        [HttpPost]
        public ActionResult ShowForm(string className)
        {

            var requestForm = Request.Form;
            var form = repository.GetForm(className);
            //CreateFormScreenViewModel screenModel = new CreateFormScreenViewModel();
            //screenModel.DetailsInfo = new Screen();
            //SetEmptyModel(screenModel, 1, 1, "", form);
            //return PartialView("Screen", screenModel);


            switch (form.FormName)
            {
                case "Adr":
                    var adrModel = new CreateFormAdrViewModel();
                    adrModel.DetailsInfo = new Adr();
                    SetEmptyModel(adrModel, 0, 0, className, form);
                    return PartialView("ADR", adrModel);
                case "Cip":
                    var cipModel = new CreateFormCipViewModel();
                    cipModel.DetailsInfo = new Cip();
                    SetEmptyModel(cipModel, 0, 0, className, form);
                    return PartialView("CIP", cipModel);
                case "Classification":
                    CreateFormClassificationViewModel classificationModel = new CreateFormClassificationViewModel();
                    classificationModel.DetailsInfo = new Classification();
                    SetEmptyModel(classificationModel, 1, 1, className, form);
                    return PartialView("Classification", classificationModel);
                case "Crushers":
                    CreateFormCrushersViewModel crushersModel = new CreateFormCrushersViewModel();
                    SetEmptyModel(crushersModel, 1, 1, className, form);
                    return PartialView("Crushers", crushersModel);
                case "Dewatering":
                    CreateFormDewateringViewModel dewateringModel = new CreateFormDewateringViewModel();
                    dewateringModel.DetailsInfo = new Dewatering();
                    SetEmptyModel(dewateringModel, 1, 1, className, form);
                    return PartialView("Dewatering", dewateringModel);
                case "Excavator":
                    CreateFormExcavatorViewModel excavatorModel = new CreateFormExcavatorViewModel();
                    excavatorModel.DetailsInfo = new Excavator();
                    SetEmptyModel(excavatorModel, 1, 1, className, form);
                    return PartialView("Excavator", excavatorModel);
                case "FilterCloths":
                    CreateFormFilterClothsViewModel filterClothsModel = new CreateFormFilterClothsViewModel();
                    filterClothsModel.DetailsInfo = new FilterCloths();
                    SetEmptyModel(filterClothsModel, 1, 1, className, form);
                    return PartialView("FilterCloths", filterClothsModel);
                case "GenericECE":
                    CreateFormGenericECEViewModel genericECEModel = new CreateFormGenericECEViewModel();
                    genericECEModel.DetailsInfo = new GenericECE();
                    SetEmptyModel(genericECEModel, 1, 1, className, form);
                    return PartialView("GenericECE", genericECEModel);
                case "GenericEngineering":
                    CreateFormGenericEngineeringViewModel genericEngineeringModel = new CreateFormGenericEngineeringViewModel();
                    genericEngineeringModel.DetailsInfo = new GenericEngineering();
                    SetEmptyModel(genericEngineeringModel, 1, 1, className, form);
                    return PartialView("GenericEngineering", genericEngineeringModel);
                case "HaulTruck":
                    CreateFormHaulTruckViewModel haulTruckModel = new CreateFormHaulTruckViewModel();
                    haulTruckModel.DetailsInfo = new HaulTruck();
                    SetEmptyModel(haulTruckModel, 1, 1, className, form);
                    return PartialView("HaulTruck", haulTruckModel);
                case "Loader":
                    CreateFormLoaderViewModel loaderModel = new CreateFormLoaderViewModel();
                    loaderModel.DetailsInfo = new Loader();
                    SetEmptyModel(loaderModel, 1, 1, className, form);
                    return PartialView("Loader", loaderModel);
                case "Mill":
                    CreateFormMillViewModel millModel = new CreateFormMillViewModel();
                    millModel.DetailsInfo = new Mill();
                    SetEmptyModel(millModel, 1, 1, className, form);
                    return PartialView("Mill", millModel);
                case "OtherConsumables":
                    CreateFormOtherConsumablesViewModel otherConsumablesModel = new CreateFormOtherConsumablesViewModel();
                    otherConsumablesModel.DetailsInfo = new OtherConsumables();
                    SetEmptyModel(otherConsumablesModel, 1, 1, className, form);
                    return PartialView("OtherConsumables", otherConsumablesModel);
                case "OtherEquipment":
                    CreateFormOtherEquipmentViewModel otherEquipmentModel = new CreateFormOtherEquipmentViewModel();
                    otherEquipmentModel.DetailsInfo = new OtherEquipment();
                    SetEmptyModel(otherEquipmentModel, 1, 1, className, form);
                    return PartialView("OtherEquipment", otherEquipmentModel);
                case "Pipe":
                    CreateFormPipeViewModel pipeModel = new CreateFormPipeViewModel();
                    pipeModel.DetailsInfo = new Pipe();
                    SetEmptyModel(pipeModel, 1, 1, className, form);
                    return PartialView("Pipe", pipeModel);
                case "ProcessPlants":
                    CreateFormProcessPlantsViewModel processPlantsModel = new CreateFormProcessPlantsViewModel();
                    processPlantsModel.DetailsInfo = new ProcessPlants();
                    SetEmptyModel(processPlantsModel, 1, 1, className, form);
                    return PartialView("ProcessPlants", processPlantsModel);
                case "Pump":
                    CreateFormPumpViewModel pumpModel = new CreateFormPumpViewModel();
                    pumpModel.DetailsInfo = new Pump();
                    SetEmptyModel(pumpModel, 1, 1, className, form);
                    return PartialView(String.Format("Create{0}", form.FormName), pumpModel);
                case "Screen":
                    CreateFormScreenViewModel screenModel = new CreateFormScreenViewModel();
                    screenModel.DetailsInfo = new Screen();
                    SetEmptyModel(screenModel, 1, 1, className, form);
                    return PartialView("Screen", screenModel);
                case "Tractors":
                    CreateFormTractorsViewModel tractorsModel = new CreateFormTractorsViewModel();
                    tractorsModel.DetailsInfo = new Tractors();
                    SetEmptyModel(tractorsModel, 1, 1, className, form);
                    return PartialView("Tractors", tractorsModel);
                case "Vehicle":
                    CreateFormVehicleViewModel Vehicle = new CreateFormVehicleViewModel();
                    Vehicle.DetailsInfo = new Vehicle();
                    SetEmptyModel(Vehicle, 1, 1, className, form);
                    return PartialView("Vehicle", Vehicle);
                case "Ventilation":
                    CreateFormVentilationViewModel ventilationModel = new CreateFormVentilationViewModel();
                    ventilationModel.DetailsInfo = new Ventilation();
                    SetEmptyModel(ventilationModel, 1, 1, className, form);
                    return PartialView("Ventilation", ventilationModel);
                default:
                    CreateFormViewModel model = new CreateFormViewModel();
                    SetEmptyModel(model, 1, 1, className, form);
                    return PartialView(form.FormName, model);
            }


        }

        private void SetUserRights(DisplayBaseViewModel model)
        {
            model.UserIsAuthenicated = WebSecurity.IsAuthenticated;

            if (model.UserIsAuthenicated)
            {
                model.UserCanBid = repository.UserCanBid(WebSecurity.CurrentUserId);
            }
            else
            {
                model.UserCanBid = false;
            }
        }

        private string SaveDocument(CreateFormViewModel model)
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

        private void SaveBidRequest(CreateFormViewModel model, string details, string fileName)
        {
            IEnumerable<Condition> condList = repository.ListCondition();
            List<Condition> conditionList = new List<Condition>();
            if (model.New)
            {
                conditionList.Add(condList.First(p => p.Id == (int)ConditionEnum.New));
            }
            if (model.Rental)
            {
                conditionList.Add(condList.First(p => p.Id == (int)ConditionEnum.Rental));
            }
            if (model.Used)
            {
                conditionList.Add(condList.First(p => p.Id == (int)ConditionEnum.Used));
            }

            RequestInfo requestInfo = new RequestInfo()
            {
                Approved = false,
                BidInfo = model.BidInfo,
                CategoryId = model.CategoryId,
                CompanyInfo = model.CompanyInfo,
                DeliveryInfo = model.DeliveryInfo,
                DetailsInfoJson = details,
                SubcategoryId = model.SubCategoryId,
                UserId = WebSecurity.CurrentUserId,
                Description = model.Description,
                ConditionList = conditionList,
                DocumentInfo = fileName,
                ClassName = model.ClassName,
                Id = model.RequestInfoId
            };

            requestInfo.BidInfo.BidStartDate = DateTime.Now;

            if (model.RequestInfoId == 0)
            {
                int requestInfoId = repository.SaveForm(requestInfo);
                string body = String.Format("A Bid Request has been entered with title:{2}, type:{4}, end date: {3}. Company Name: {0}, contact email: {1}, please review", requestInfo.CompanyInfo.CompanyName, requestInfo.CompanyInfo.Email, requestInfo.BidInfo.BidName, requestInfo.BidInfo.BidEndDate.ToShortDateString(), requestInfo.ClassName);
                try
                {
                    Utilities.SendMail("info@minebidz.com", String.Format("Bid Request MBR#{0}", requestInfoId), body);
                    Utilities.SendMail("serguei.razykov@gmail.com", String.Format("Bid Request MBR#{0}", requestInfoId), body);
                    Utilities.SendMail("mike@iias.com", String.Format("Bid Request MBR#{0}", requestInfoId), body);
                }
                catch { }
            }
            else
            {
                repository.UpdateForm(requestInfo);
            }
        }

        private ActionResult SaveRequest(CreateFormViewModel model, string details)
        {

            if (!model.Acknowledged)
            {
                return SetCreateModelError(model, "You must agree with terms and conditions");
            }

            try
            {
                //save file
                string fileName = SaveDocument(model);
                SaveBidRequest(model, details, fileName);
                return RedirectToAction("Index", "Home");
            }
            catch(Exception ex)
            {
                //try to log the error here
                return SetCreateModelError(model, "Error saving the request");
            }
        }

        private ActionResult SetCreateModelError(CreateFormViewModel model, string message)
        {
            foreach (var modelValue in ModelState.Values)
            {
                modelValue.Errors.Clear();
            }
            ModelState.AddModelError("", message);
            return Create(model.ClassName, model.CategoryId, model.SubCategoryId, model.RequestInfoId);
        }
    }
}
