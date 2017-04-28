using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Demo;

namespace Demo.Views
{
    public class discoveriesController : Controller
    {
        private db_a094d4_demoEntities db = new db_a094d4_demoEntities();

        // GET: discoveries
        public ActionResult Index()
        {
            return View(db.discovery.ToList());
        }

        // GET: discoveries/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            discovery discovery = db.discovery.Find(id);
            if (discovery == null)
            {
                return HttpNotFound();
            }
            return View(discovery);
        }

        // GET: discoveries/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: discoveries/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ictag,time,serial,brand,model,cpu,hdd,ram,optical_drive,location")] discovery discovery)
        {
            if (ModelState.IsValid)
            {
                db.discovery.Add(discovery);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(discovery);
        }

        // GET: discoveries/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            discovery discovery = db.discovery.Find(id);
            if (discovery == null)
            {
                return HttpNotFound();
            }
            return View(discovery);
        }

        // POST: discoveries/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ictag,time,serial,brand,model,cpu,hdd,ram,optical_drive,location")] discovery discovery)
        {
            if (ModelState.IsValid)
            {
                db.Entry(discovery).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(discovery);
        }

        // GET: discoveries/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            discovery discovery = db.discovery.Find(id);
            if (discovery == null)
            {
                return HttpNotFound();
            }
            return View(discovery);
        }

        // POST: discoveries/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        
       
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
