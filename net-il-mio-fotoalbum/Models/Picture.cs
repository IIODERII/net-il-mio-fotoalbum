using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Hosting;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace la_mia_pizzeria_static.Models
{
    public class PictureFormModel
    {
        public Picture Picture { get; set; }
        public List<SelectListItem>? Categories { get; set; } // Gli elementi dei tag selezionabili per la select (analogo a Categories)
        public List<string>? SelectedCategories { get; set; } // Gli ID degli elementi effettivamente selezionati
        [Required(ErrorMessage = "Per favore carica un'immagine.")]
        public IFormFile ImageFormFile { get; set; }

        public PictureFormModel() { }

        public void CreateCategories()
        {
            this.Categories = new List<SelectListItem>();
            this.SelectedCategories = new List<string>();
            var categosFromDB = new List<Category>();
            using(PictureContext db = new PictureContext())
            {
                categosFromDB = db.Categories.ToList();
            }
            foreach (var c in categosFromDB) // es. tag1, tag2, tag3... tag10
            {
                if(this.Picture.Categories != null)
                {
                    bool isSelected = this.Picture.Categories.Any(t => t.Id == c.Id) == true;
                    this.Categories.Add(new SelectListItem() // lista degli elementi selezionabili
                    {
                        Text = c.Name, // Testo visualizzato
                        Value = c.Id.ToString(), // SelectListItem vuole una generica stringa, non un int
                        Selected = isSelected // es. tag1, tag5, tag9
                    });
                    if (isSelected)
                        this.SelectedCategories.Add(c.Id.ToString()); // lista degli elementi selezionati
                }
                else
                {
                    this.Categories.Add(new SelectListItem() // lista degli elementi selezionabili
                    {
                        Text = c.Name, // Testo visualizzato
                        Value = c.Id.ToString(), // SelectListItem vuole una generica stringa, non un int
                        Selected = false // es. tag1, tag5, tag9
                    });
                }
            }
        }

        public void SetImageFileFromFormFile()
        {
            if (ImageFormFile != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    ImageFormFile.CopyTo(memoryStream);
                    Picture.Image = memoryStream.ToArray();
                }
            }
        }
    }
    public class Picture
    {
        [Key] public int Id { get; set; }

        [Required(ErrorMessage = "Non è possibile creare una foto senza titolo!")]
        [StringLength(50, ErrorMessage = "Il titolo non può essere più di 50 caratteri")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Non è possibile creare una foto senza descrizione")]
        public string Description { get; set; }

        public bool Visible { get; set; }

        public Byte[]? Image { get; set; }
        public string ImgSrc => Image != null ? $"data:image/jpg;base64,{Convert.ToBase64String(Image)}" : "";

        public List<Category>? Categories { get; set; }

        public Picture() { }
    }
}
