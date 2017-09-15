using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Domain;

namespace MineBidz.Controllers
{
    public class NavController : Controller
    {
        Repository repository = new Repository(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        //
        // GET: /Nav/

        public PartialViewResult Menu(int? categoryId = null)
        {
            IEnumerable<Category> categories = repository.ListCategory();

            if (categoryId.HasValue)
            {
                categories.First(c => c.Id == categoryId.Value).Selected = true;
            }

            return PartialView(categories);
        }
    }
}
