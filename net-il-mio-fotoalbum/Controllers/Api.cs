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
using net_il_mio_fotoalbum.Models;

namespace net_il_mio_fotoalbum.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class Api : Controller
    {
        [HttpGet]
        public IActionResult GetAllPictures(string name = "")
        {
            using(var db = new PictureContext())
            {
                if (name == "")
                {
                    IQueryable<Picture> pics = db.Pictures.Where(p => p.Visible == true);
                    return Ok(pics.ToList());
                }
                IQueryable<Picture> filteredPics = db.Pictures.Where(p => p.Title.Contains(name)).Where(p => p.Visible == true);
                var pic = filteredPics.ToList();
                return Ok(filteredPics.ToList());
            }
        }

        [HttpGet]
        public IActionResult GetPictureById(int id)
        {
            using (var db = new PictureContext())
            {
                IQueryable<Picture> pic = db.Pictures.Where(p => p.Id == id);
                return Ok(pic.ToList());
            }
        }

        [HttpPost]
        public IActionResult saveMessage([FromBody] Message data)
        {
            using(var db = new PictureContext())
            {
                Message newMessage = new Message()
                {
                    Text = data.Text,
                    Email = data.Email,
                };

                db.Messages.Add(newMessage);
                db.SaveChanges();
            }
            return Ok();
        }
    }
}
