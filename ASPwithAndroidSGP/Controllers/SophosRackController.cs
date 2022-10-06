using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ASPwithAndroidSGP.Models;
using FireSharp.Response;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace ASPwithAndroidSGP.Controllers
{
    public class SophosRackController : Controller
    {
        string MainPath = "Labs/Sophos Rack";
        // GET: SophosRackController
        IFirebaseConfig firebaseConfig = new FirebaseConfig
        {
            AuthSecret = "8SLRupkYtIB76ne19Baq6MfFutqoANgegQQ9rcS1",
            BasePath = "https://asp-with-firebase-default-rtdb.firebaseio.com/"
        };
        FirebaseClient? firebaseClient;

        // GET: SophosRackController
        public ActionResult Index()
        {
            firebaseClient = new FireSharp.FirebaseClient(firebaseConfig);
            FirebaseResponse firebaseResponse = firebaseClient.Get(MainPath);
            dynamic data = JsonConvert.DeserializeObject<dynamic>(firebaseResponse.Body);
            var list = new List<Lab>();
            foreach (var item in data)
            {
                list.Add(JsonConvert.DeserializeObject<Lab>(((JProperty)item).Value.ToString()));
            }
            return View(list);
        }

        // GET: SophosRackController/Details/5
        public ActionResult Details(string id)
        {
            firebaseClient = new FireSharp.FirebaseClient(firebaseConfig);
            FirebaseResponse firebaseResponse = firebaseClient.Get(MainPath + "/" + id);
            Lab lab = JsonConvert.DeserializeObject<Lab>(firebaseResponse.Body);
            return View(lab);
        }

        // GET: SophosRackController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: SophosRackController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Lab lab)
        {
            try
            {
                firebaseClient = new FireSharp.FirebaseClient(firebaseConfig);
                var data = lab;
                PushResponse pushResponse = firebaseClient.Push(MainPath + "/", data);
                data.labId = pushResponse.Result.name;
                SetResponse setResponse = firebaseClient.Set(MainPath + "/" + data.labId, data);
                ModelState.AddModelError(String.Empty, "Added Successfully");
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(String.Empty, ex.Message);
                return View();
            }
        }

        // GET: SophosRackController/Edit/5
        public ActionResult Edit(string id)
        {
            firebaseClient = new FireSharp.FirebaseClient(firebaseConfig);
            FirebaseResponse firebaseResponse = firebaseClient.Get(MainPath + "/" + id);
            Lab lab = JsonConvert.DeserializeObject<Lab>(firebaseResponse.Body);
            return View(lab);
        }

        // POST: SophosRackController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Lab lab)
        {
            try
            {
                firebaseClient = new FireSharp.FirebaseClient(firebaseConfig);
                SetResponse setResponse = firebaseClient.Set(MainPath + "/" + lab.labId, lab);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: SophosRackController/Delete/5
        [HttpGet]
        public ActionResult Delete(string id)
        {
            firebaseClient = new FireSharp.FirebaseClient(firebaseConfig);
            FirebaseResponse firebase = firebaseClient.Delete(MainPath + "/" + id);
            return RedirectToAction("Index");
        }
    }
}
