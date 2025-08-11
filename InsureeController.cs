using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using YourNamespace.Models; // Change to your actual namespace

namespace YourNamespace.Controllers
{
    public class InsureeController : Controller
    {
        private InsuranceEntities db = new InsuranceEntities();

        // GET: Insuree
        public ActionResult Index()
        {
            return View(db.Insurees.ToList());
        }

        // GET: Insuree/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Insuree insuree = db.Insurees.Find(id);
            if (insuree == null)
                return HttpNotFound();

            return View(insuree);
        }

        // GET: Insuree/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Insuree/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,FirstName,LastName,EmailAddress,DateOfBirth,CarYear,CarMake,CarModel,SpeedingTickets,DUI,CoverageType")] Insuree insuree)
        {
            if (ModelState.IsValid)
            {
                decimal monthlyTotal = 50m; // Base price

                // Age calculation
                int age = DateTime.Now.Year - insuree.DateOfBirth.Year;
                if (insuree.DateOfBirth > DateTime.Now.AddYears(-age)) age--;

                if (age <= 18)
                {
                    monthlyTotal += 100m;
                }
                else if (age >= 19 && age <= 25)
                {
                    monthlyTotal += 50m;
                }
                else
                {
                    monthlyTotal += 25m;
                }

                // Car year checks
                if (insuree.CarYear < 2000)
                {
                    monthlyTotal += 25m;
                }
                else if (insuree.CarYear > 2015)
                {
                    monthlyTotal += 25m;
                }

                // Car make/model checks
                if (!string.IsNullOrEmpty(insuree.CarMake) && insuree.CarMake.ToLower() == "porsche")
                {
                    monthlyTotal += 25m;

                    if (!string.IsNullOrEmpty(insuree.CarModel) && insuree.CarModel.ToLower() == "911 carrera")
                    {
                        monthlyTotal += 25m; // Additional Porsche 911 Carrera charge
                    }
                }

                // Speeding tickets
                monthlyTotal += insuree.SpeedingTickets * 10m;

                // DUI check
                if (insuree.DUI)
                {
                    monthlyTotal += monthlyTotal * 0.25m;
                }

                // Coverage type check
                if (insuree.CoverageType) // Assuming true = full coverage
                {
                    monthlyTotal += monthlyTotal * 0.50m;
                }

                // Save quote
                insuree.Quote = monthlyTotal;

                db.Insurees.Add(insuree);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(insuree);
        }

        // GET: Insuree/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Insuree insuree = db.Insurees.Find(id);
            if (insuree == null)
                return HttpNotFound();

            return View(insuree);
        }

        // POST: Insuree/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,FirstName,LastName,EmailAddress,DateOfBirth,CarYear,CarMake,CarModel,SpeedingTickets,DUI,CoverageType")] Insuree insuree)
        {
            if (ModelState.IsValid)
            {
                // Recalculate the quote on edit
                decimal monthlyTotal = 50m;

                int age = DateTime.Now.Year - insuree.DateOfBirth.Year;
                if (insuree.DateOfBirth > DateTime.Now.AddYears(-age)) age--;

                if (age <= 18)
                    monthlyTotal += 100m;
                else if (age >= 19 && age <= 25)
                    monthlyTotal += 50m;
                else
                    monthlyTotal += 25m;

                if (insuree.CarYear < 2000)
                    monthlyTotal += 25m;
                else if (insuree.CarYear > 2015)
                    monthlyTotal += 25m;

                if (!string.IsNullOrEmpty(insuree.CarMake) && insuree.CarMake.ToLower() == "porsche")
                {
                    monthlyTotal += 25m;
                    if (!string.IsNullOrEmpty(insuree.CarModel) && insuree.CarModel.ToLower() == "911 carrera")
                        monthlyTotal += 25m;
                }

                monthlyTotal += insuree.SpeedingTickets * 10m;

                if (insuree.DUI)
                    monthlyTotal += monthlyTotal * 0.25m;

                if (insuree.CoverageType)
                    monthlyTotal += monthlyTotal * 0.50m;

                insuree.Quote = monthlyTotal;

                db.Entry(insuree).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(insuree);
        }

        // GET: Insuree/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Insuree insuree = db.Insurees.Find(id);
            if (insuree == null)
                return HttpNotFound();

            return View(insuree);
        }

        // POST: Insuree/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Insuree insuree = db.Insurees.Find(id);
            db.Insurees.Remove(insuree);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: Admin View
        public ActionResult Admin()
        {
            var insurees = db.Insurees.ToList();
            return View(insurees);
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
