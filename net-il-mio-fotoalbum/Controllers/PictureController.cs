using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using la_mia_pizzeria_static;
using la_mia_pizzeria_static.Models;
using Microsoft.AspNetCore.Authorization;
using Azure;
using Microsoft.Extensions.Hosting;

namespace net_il_mio_fotoalbum.Controllers
{
    [Authorize]
    public class PictureController : Controller
    {
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            using (PictureContext db = new PictureContext()) {
                List <Picture> pictureList = db.Pictures.ToList();
            return View(pictureList);
            }
            
        }

        [HttpGet]
        public IActionResult Show(int id)
        {
            using(PictureContext db = new PictureContext())
            {
                Picture pic = db.Pictures.Where(p => p.Id == id).Include(p => p.Categories).FirstOrDefault();
                return View(pic);
            }
        }

        [HttpGet]
        public IActionResult Create() 
        { 
            PictureFormModel model = new PictureFormModel();
            model.Picture = new Picture();
            model.CreateCategories();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult Create(PictureFormModel data)
        {
            // Aggiungere log per vedere se il file viene caricato
            if (data.ImageFormFile != null)
            {
                Console.WriteLine($"File size: {data.ImageFormFile.Length}");
            }
            else
            {
                Console.WriteLine("No file uploaded.");
            }

            if (!ModelState.IsValid)
            {
                // Log degli errori del ModelState
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine(error.ErrorMessage);
                }

                // Ritorniamo "data" alla view così che la form abbia di nuovo i dati inseriti
                using (PictureContext db = new PictureContext())
                {
                    data.CreateCategories();
                    return View("Create", data);
                }
            }

            // Gestione del file caricato
            if (data.ImageFormFile != null && data.ImageFormFile.Length > 0)
            {
                using (var memoryStream = new MemoryStream())
                {
                    data.ImageFormFile.CopyTo(memoryStream);
                    data.Picture.Image = memoryStream.ToArray();
                }
            }
            else
            {
                ModelState.AddModelError("ImageFormFile", "Per favore carica un'immagine.");
                using (PictureContext db = new PictureContext())
                {
                    data.CreateCategories();
                    return View("Create", data);
                }
            }

            using (var db = new PictureContext())
            {
                if (data.SelectedCategories != null)
                {
                    data.Picture.Categories = new List<Category>();
                    foreach (var tagId in data.SelectedCategories)
                    {
                        int id = int.Parse(tagId);
                        var tag = db.Categories.FirstOrDefault(t => t.Id == id); 
                        if (tag != null)
                        {
                            data.Picture.Categories.Add(tag);
                        }
                    }
                }
                db.Pictures.Add(data.Picture);
                db.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            using (var db = new PictureContext())
            {
                Picture picToEdit = db.Pictures.Where(p => p.Id == id).Include(p => p.Categories).FirstOrDefault();
                if (picToEdit == null)
                {
                    return NotFound();
                }
                else
                {
                    PictureFormModel model = new PictureFormModel();
                    model.Picture = picToEdit;
                    model.CreateCategories();

                    return View(model);
                }
            }
        }

        [HttpPost]
        public IActionResult Edit(int id, PictureFormModel data)
        {
            if (!ModelState.IsValid)
            {
                using (PictureContext db = new PictureContext())
                {
                    data.CreateCategories();
                    return View("Edit", data);
                }
            }

            using (var db = new PictureContext())
            {
                Picture p = db.Pictures.Where(p => p.Id == id).Include(i => i.Categories).FirstOrDefault();

                if (p != null)
                {
                    p.Title = data.Picture.Title;
                    p.Description = data.Picture.Description;
                    if (data.ImageFormFile != null && data.ImageFormFile.Length > 0)
                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            data.ImageFormFile.CopyTo(memoryStream);
                            data.Picture.Image = memoryStream.ToArray();
                        }
                    }
                    if(data.Picture.Image != null)
                         p.Image = data.Picture.Image;

                    p.Visible = data.Picture.Visible;
                    p.Categories.Clear();
                    if (data.SelectedCategories != null)
                    {
                        foreach (var ingId in data.SelectedCategories)
                        {
                            int selectedId = int.Parse(ingId);
                            Category newIng = db.Categories.Where(i => i.Id == selectedId).FirstOrDefault();
                            p.Categories.Add(newIng);
                        }
                    }

                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

            }
            return NotFound();
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            using (var db = new PictureContext())
            {
                Picture toDelete = db.Pictures.Where(p => p.Id == id).FirstOrDefault();

                if (toDelete != null)
                {
                    db.Pictures.Remove(toDelete);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

                return NotFound();
            }
        }
    }
}
