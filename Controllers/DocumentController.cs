using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Apache.NMS;
using Apache.NMS.ActiveMQ;
using DocumentsManagment.Dao;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DocumentsManagment.Controllers
{
    public class DocumentController : Controller
    {
        MyDbContext dbManager;

        public DocumentController(MyDbContext dbManager)
        {
            this.dbManager = dbManager;
        }
        // GET: /<controller>/
        public IActionResult Index(string keyword = "", int page = 0, int size = 5)
        {
            int ItemsToSkip = page * size;
            if (keyword == null) keyword = "";
            IEnumerable<Document> documents = dbManager.Documents.Include(Doc => Doc.Famille).Where(Doc => Doc.Nom.Contains(keyword)).Skip(ItemsToSkip).Take(size).ToList();
            ViewBag.CurrentPage = page;
            int TotalItems = dbManager.Documents.Where(Doc => Doc.Nom.Contains(keyword)).Count();
            if (TotalItems % size == 0 || TotalItems >= size)
                ViewBag.TotalPages = TotalItems / size - 1;
            else
                ViewBag.TotalPages = TotalItems / size;
            ViewBag.keyword = keyword;
            return View(documents);
        }
        public IActionResult AddDocument()
        {
            IEnumerable<Famille> familles = dbManager.Familles.ToList();
            Document document = new Document();
            ViewBag.Familles = familles;
            return View(document);
        }
        [HttpPost]
        public IActionResult SaveDocument(Document document)
        {
            if (ModelState.IsValid)
            {
                dbManager.Documents.Add(document);
                dbManager.SaveChanges();
                String message = "{"
                       + "  \"nom\"    : " + " \"" + document.Nom + " \", "
                       + "  \"format\" : " + " \"" + document.Format + " \", "
                       + "  \"taille\" : " + document.Taille
                       + "}";
                try
                {

                    IConnectionFactory connectionFactory = new ConnectionFactory("tcp://localhost:61616");
                    IConnection _connection = connectionFactory.CreateConnection();
                    _connection.Start();
                    ISession _session = _connection.CreateSession();

                    IDestination destination = _session.GetDestination("topic://jmsapp");
                    using (IMessageProducer producer = _session.CreateProducer(destination))
                    {
                        var textMessage = producer.CreateTextMessage(message);
                        textMessage.Properties.SetString("code", "C1");
                        producer.Send(textMessage);
                    }

                }
                catch (Exception exception )
                {

                    Console.WriteLine(exception.ToString());
                }
               
            return RedirectToAction("Index");
            }
            IEnumerable<Famille> familles = dbManager.Familles.ToList();
            ViewBag.Familles = familles;
            return View("AddDocument", document);
        }
        [HttpPost]
        public IActionResult FindProduct(String keyword)
        {
            //ViewBag.keyword = keyword;
            return RedirectToAction("Index", keyword);
        }
        
        public IActionResult DocumentsByFamilles(int FamilleID = -1)
        {
            List<SelectListItem> ListFamilles = new List<SelectListItem>();
            IEnumerable<Famille> familles = dbManager.Familles.ToList();
            foreach (var item in familles)
            {
                ListFamilles.Add(new SelectListItem
                {
                    Text = item.Nom,
                    Value = item.FamilleID.ToString()
                });
            }
            ViewBag.ListFamilles = ListFamilles;
            if (FamilleID != -1)
            {
                IEnumerable<Document> docs = dbManager.Documents.Include(Doc => Doc.Famille).Where(Doc => Doc.FamilleID == FamilleID).ToList();
                ViewBag.Documents = docs;
            }
            else
            {
                IEnumerable<Document> docs = new List<Document>();
                ViewBag.Documents = docs;
            }
            return View("DocumentsByFamilles");
        }
    }
}
