using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AttributeRoutingSample.Models;
using System.Net.Http;
using Newtonsoft.Json.Linq;

namespace AttributeRoutingSample.Controllers
{
    /// <summary>
    /// This controller mixes conventional and attribute routing in a single controller. The 'Professor'
    /// action uses attribute routing while rest of the actions are accessible by conventional routes.
    /// 
    /// The comments by each action detail example URLs that could reach the action.
    /// 
    /// NOTE: Conventional routes can never access attributed controller/actions
    /// </summary>
    public class CountriesController : Controller
    {
        private IcountryService _CountryService { get; set; }

        public CountriesController()
        {
            _CountryService = new countryService();
        }

        private SchoolContext db = new SchoolContext();

        // GET: /Countries/Details/5/Professor
        [Route("Countries/Details/{id}/Professor")]
        public async Task<ActionResult> Professor(int id)
        {
            Course course = await db.Courses.Include(c => c.Professor).Where(c => c.Id == id).FirstOrDefaultAsync();
            if (course == null)
            {
                return HttpNotFound();
            }

            return View(course.Professor);
        }

        // GET: /Countries/
        public async Task<ActionResult> Index()
        {
            var CountryList = await _CountryService.getWorldBankCountryList();
            //var CountryList = await WorldBankCountryList();
            return View(CountryList);
            //return View(await db.Courses.ToListAsync());
        }

        private static string _address = "http://api.worldbank.org/countries?format=json";

        private static async Task<List<Country>> WorldBankCountryList()
        {
            // Create an HttpClient instance
            HttpClient client = new HttpClient();

            // Send a request asynchronously and continue when complete
            HttpResponseMessage response = await client.GetAsync(_address);

            // Check that response was successful or throw exception
            response.EnsureSuccessStatusCode();

            // Read response asynchronously as JToken and write out top facts for each country
            JArray content = await response.Content.ReadAsAsync<JArray>();

            Console.WriteLine("First 50 countries listed by The World Bank...");
            var cList = new List<Country>();

            foreach (var iCountry in content[1])
            {
                var country = new Country() { Name = iCountry.Value<string>("name") };
                cList.Add(country);

                Console.WriteLine("   {0}, Country Code: {1}, Capital: {2}, Latitude: {3}, Longitude: {4}",
                    iCountry.Value<string>("name"),
                    iCountry.Value<string>("iso2Code"),
                    iCountry.Value<string>("capitalCity"),
                    iCountry.Value<string>("latitude"),
                    iCountry.Value<string>("longitude"));
            }
            return cList;
        }

        // GET: /Courses/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Course course = await db.Courses.FindAsync(id);
            if (course == null)
            {
                return HttpNotFound();
            }
            return View(course);
        }

        // GET: /Courses/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /Courses/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include="Id,Name")] Course course)
        {
            if (ModelState.IsValid)
            {
                db.Courses.Add(course);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(course);
        }

        // GET: /Courses/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Course course = await db.Courses.FindAsync(id);
            if (course == null)
            {
                return HttpNotFound();
            }
            return View(course);
        }

        // POST: /Courses/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include="Id,Name")] Course course)
        {
            if (ModelState.IsValid)
            {
                db.Entry(course).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(course);
        }

        // GET: /Courses/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Course course = await db.Courses.FindAsync(id);
            if (course == null)
            {
                return HttpNotFound();
            }
            return View(course);
        }

        // POST: /Courses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Course course = await db.Courses.FindAsync(id);
            db.Courses.Remove(course);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
