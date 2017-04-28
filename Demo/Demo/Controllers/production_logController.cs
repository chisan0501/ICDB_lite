using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Demo;

namespace Demo.Controllers
{
    public class production_logController : Controller
    {
        private db_a094d4_demoEntities db = new db_a094d4_demoEntities();

        // GET: production_log
        public ActionResult Index()
        {
            return View();
        }

        // GET: production_log/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            production_log production_log = db.production_log.Find(id);
            if (production_log == null)
            {
                return HttpNotFound();
            }
            return View(production_log);
        }

        // GET: production_log/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: production_log/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "time,ictags,wcoa,ocoa,Manufacture,Model,CPU,RAM,HDD,serial,channel,pre_coa,location,video_card,screen_size")] production_log production_log)
        {
            if (ModelState.IsValid)
            {
                db.production_log.Add(production_log);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(production_log);
        }

        // GET: production_log/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            production_log production_log = db.production_log.Find(id);
            if (production_log == null)
            {
                return HttpNotFound();
            }
            return View(production_log);
        }

        // POST: production_log/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "time,ictags,wcoa,ocoa,Manufacture,Model,CPU,RAM,HDD,serial,channel,pre_coa,location,video_card,screen_size")] production_log production_log)
        {
            if (ModelState.IsValid)
            {
                db.Entry(production_log).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(production_log);
        }

        // GET: production_log/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            production_log production_log = db.production_log.Find(id);
            if (production_log == null)
            {
                return HttpNotFound();
            }
            return View(production_log);
        }

        // POST: production_log/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            production_log production_log = db.production_log.Find(id);
            db.production_log.Remove(production_log);
            db.SaveChanges();
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
