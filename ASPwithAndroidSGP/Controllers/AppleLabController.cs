using ASPwithAndroidSGP.Models;
using FireSharp;
using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ASPwithAndroidSGP.Controllers
{
    public class AppleLabController : Controller
    {
        IFirebaseConfig firebaseConfig = new FirebaseConfig
        {
            AuthSecret = "8SLRupkYtIB76ne19Baq6MfFutqoANgegQQ9rcS1",
            BasePath = "https://asp-with-firebase-default-rtdb.firebaseio.com/"
        };
        FirebaseClient? firebaseClient;
        // GET: AppleLabController
        public ActionResult Index()
        {
            firebaseClient=new FireSharp.FirebaseClient(firebaseConfig);
            FirebaseResponse firebaseResponse = firebaseClient.Get("Labs/Apple Lab");
            dynamic data = JsonConvert.DeserializeObject<dynamic>(firebaseResponse.Body);
            var list = new List<Lab>();
            foreach (var item in data)
            {
                list.Add(JsonConvert.DeserializeObject<Lab>(((JProperty)item).Value.ToString()));
            }
            return View(list);
        }

        // GET: AppleLabController/Details/5
        public ActionResult Details(string id)
        {
            firebaseClient = new FireSharp.FirebaseClient(firebaseConfig);
            FirebaseResponse firebaseResponse = firebaseClient.Get("Labs/Apple Lab/" + id);
            Lab lab= JsonConvert.DeserializeObject<Lab>(firebaseResponse.Body);
            return View(lab);
        }

        // GET: AppleLabController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AppleLabController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Lab lab)
        {
            try
            {
                firebaseClient = new FireSharp.FirebaseClient(firebaseConfig);
                var data = lab;
                PushResponse pushResponse= firebaseClient.Push("Labs/Apple Lab/", data);
                data.labId = pushResponse.Result.name;
                SetResponse setResponse = firebaseClient.Set("Labs/Apple Lab/" + data.labId, data);
                ModelState.AddModelError(String.Empty, "Added Successfully");
                return RedirectToAction("Index");
            }
            catch(Exception ex)
            {
                ModelState.AddModelError(String.Empty, ex.Message);
                return View();
            }
        }

        // GET: AppleLabController/Edit/5
        public ActionResult Edit(string id)
        {
            firebaseClient = new FireSharp.FirebaseClient(firebaseConfig);
            FirebaseResponse firebaseResponse = firebaseClient.Get("Labs/Apple Lab/" + id);
            Lab lab = JsonConvert.DeserializeObject<Lab>(firebaseResponse.Body);
            return View(lab);
        }

        // POST: AppleLabController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Lab lab)
        {
            try
            {
                firebaseClient = new FireSharp.FirebaseClient(firebaseConfig);
                SetResponse setResponse = firebaseClient.Set("Labs/Apple Lab/" + lab.labId, lab);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: AppleLabController/Delete/5
        [HttpGet]
        public ActionResult Delete(string id)
        {
            firebaseClient = new FireSharp.FirebaseClient(firebaseConfig);
            FirebaseResponse firebase = firebaseClient.Delete("Labs/Apple Lab/" + id);
            return RedirectToAction("Index");
        }
    }
}
