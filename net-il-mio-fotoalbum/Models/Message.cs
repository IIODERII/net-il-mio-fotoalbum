using System.ComponentModel.DataAnnotations;

namespace net_il_mio_fotoalbum.Models
{
    public class Message
    {
        [Key]
        public int Id { get; set; }
        public string Text { get; set; }
        public string Email { get; set; }

        public Message() { }
    }
}
