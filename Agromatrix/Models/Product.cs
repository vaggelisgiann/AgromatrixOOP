using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Agromatrix.Models
{
    [Table("products", Schema = "public")] // Ορίζουμε ρητά το όνομα του πίνακα και το schema
    public class Product
    {
        [Key]
        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Λέει στο EF: "Μην στέλνεις τιμή, άσε τη βάση"
        public int Id { get; set; }

        [Column("name")]
        public string Name { get; set; } = string.Empty;

        [Column("descr")]
        public string Description { get; set; } = string.Empty;

        [Column("categ")]
        public string Category { get; set; } = string.Empty;

        [Column("price")]
        public decimal Price { get; set; }

        [Column("image_url")]
        public string ImageUrl { get; set; } = string.Empty;
    }
}