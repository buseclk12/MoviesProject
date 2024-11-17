using System.Collections.Generic;

namespace BLL.DAL
{
    public class Director
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public bool IsRetired { get; set; }
        public ICollection<Movie> Movies { get; set; }
    }
}