using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Demo;
using System.Transactions;

namespace Demo.Controllers
{
    public class palletsController : Controller
    {
        private db_a094d4_demoEntities1 db = new db_a094d4_demoEntities1();

        // GET: pallets
        public ActionResult Index()
        {
            return View(db.pallet.ToList());
        }

        // GET: pallets/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            pallet pallet = db.pallet.Find(id);
            if (pallet == null)
            {
                return HttpNotFound();
            }
            return View(pallet);
        }

        // GET: pallets/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: pallets/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ictags,pallet_name,type,note")] pallet pallet)
        {
            if (ModelState.IsValid)
            {
                db.pallet.Add(pallet);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(pallet);
        }

        // GET: pallets/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            pallet pallet = db.pallet.Find(id);
            if (pallet == null)
            {
                return HttpNotFound();
            }
            return View(pallet);
        }

        // POST: pallets/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ictags,pallet_name,type,note")] pallet pallet)
        {
            if (ModelState.IsValid)
            {
                db.Entry(pallet).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(pallet);
        }

        // GET: pallets/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            pallet pallet = db.pallet.Find(id);
            if (pallet == null)
            {
                return HttpNotFound();
            }
            return View(pallet);
        }

        public JsonResult validate(int[] input, string pallet_name)
        {
         
            bool valid = false;
            List<string> message = new List<string>();
            var validate_pallet_name = (from t in db.pallet where t.pallet_name == pallet_name select t).Count();
            if(validate_pallet_name != 0)
            {
                message.Add("Pallet Name Already Exist");
            }
            var server_asset = (from t in db.pallet select t.ictags).ToArray();
            var client_asset = input;
            var duplicate = server_asset.Intersect(client_asset).ToList();
            
            
            if (duplicate.Count() == 0 && validate_pallet_name == 0)
            {
                valid = true;
            }
            else
            {
                for(int i= 0; i< duplicate.Count(); i++)
                {
                    message.Add("Asset " +duplicate[i].ToString() + " Already Exist");
                }
            }
        
            

            return Json(new { valid = valid, duplicate = duplicate ,message = message}, JsonRequestBehavior.AllowGet);
        }

        public JsonResult get_pallet_data(string pallet) {

            var result = (from p in db.pallet where p.pallet_name == pallet select p.ictags).ToList();

            return Json(new { pallet= result},JsonRequestBehavior.AllowGet);
        }

        public JsonResult edit_pallet(List<int> pallet,string pallet_name)
        {
            List<string> message = new List<string>();
            var server_pallet = (from p in db.pallet where p.pallet_name == pallet_name select p.ictags).ToList();
            var difference = pallet.Except(server_pallet).ToList();
            var delete = server_pallet.Except(pallet).ToList();

            foreach (var item in delete) {
                try
                {
                    using (var remove = new db_a094d4_demoEntities1())
                    {
                        remove.Database.ExecuteSqlCommand(
                        "Delete from pallet where ictags = '" + item + "'");
                    }


                    message.Add( item+ " Has Been Deleted from Pallet : " + pallet_name);
                }
                catch (Exception e)
                {
                    message.Add(e.InnerException.InnerException.Message);
                    continue;
                }
            }
           
            foreach (var item in difference) {

                if (item != 0) {
                    try
                    {
                        var entry = new pallet();

                        entry.ictags = item;
                        entry.pallet_name = pallet_name;

                        db.pallet.Add(entry);

                        message.Add(item.ToString() + "Successfully Added to Pallet: " + pallet_name);
                        db.SaveChanges();
                    }
                    catch(Exception e)
                    {
                        message.Add(e.InnerException.InnerException.Message);
                        continue;
                    }
                }




                message.Add("Task Complete");

            
            }

            return Json( new { message=message},JsonRequestBehavior.AllowGet);

        }

        public JsonResult remove_pallet(string pallet_name)
        {
            string message = "";
            //remove any pallet name from para
            try
            {
                using (var remove = new db_a094d4_demoEntities1())
                {
                    remove.Database.ExecuteSqlCommand(
                    "Delete from pallet where pallet_name = '" + pallet_name + "'");
                }


               message = pallet_name + " Has Been Deleted";
            }
            catch(Exception e)
            {
               message = e.Message;
            }

            return Json(message,JsonRequestBehavior.AllowGet);

        }


        public JsonResult submit_data(int[] input, string pallet_name)
        {
            List<string> message = new List<string>();

            for (int i = 0; i < input.Length; i++)
            {
                try
                {
                    using (var tran = new TransactionScope())
                    {
                        var entry = new pallet();

                        entry.ictags = input[i];
                        entry.pallet_name = pallet_name;
                       
                        db.pallet.Add(entry);
                        tran.Complete();
                        message.Add(input[i] + "Successfully Added to Pallet: " + pallet_name);

                    }

                    db.SaveChanges();
                }
                catch (Exception e)
                {
                    message.Add( input[i] + ":" + e.InnerException.InnerException.Message );
                    continue;
                }
            }
            

            return Json(new { message = message }, JsonRequestBehavior.AllowGet);
        }


       public JsonResult get_pallet(string pallet_name)
        {

            var result = (from t in db.pallet where t.pallet_name == pallet_name select new { t.ictags, t.pallet_name }).ToList();

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult get_data ()
        {
            //gather all pallet infomation for the pallet index page
            var data = db.Database.SqlQuery<Models.SQLModel.PalletData>("select pallet_name, count(*) as num from pallet group by pallet_name").ToList<Models.SQLModel.PalletData>();
            return Json(data,JsonRequestBehavior.AllowGet);
        }



        // POST: pallets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            pallet pallet = db.pallet.Find(id);
            db.pallet.Remove(pallet);
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
