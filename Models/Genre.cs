using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MoviesApi.Models
{
    public class Genre
    {

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Byte Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        //public List<Movie> Movies { get; set; }

    }
}
