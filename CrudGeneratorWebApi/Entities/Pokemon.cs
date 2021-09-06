using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CrudGenerator;

namespace CrudGeneratorWebApi.Entities
{
    [CrudEntity]
    [Table("Pokemons")]
    public class Pokemon
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
    }
}