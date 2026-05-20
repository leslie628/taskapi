using System.ComponentModel.DataAnnotations.Schema;

namespace TaskManagerApi.Model
{
    public class AppUser
    {
        [Column("id")]
        public int Id { get; set;  }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
    }
}
