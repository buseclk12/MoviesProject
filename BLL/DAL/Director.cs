namespace BLL.DAL
{
    public class Director
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        public ICollection<Movie> Movies { get; set; } = new List<Movie>();

        public bool IsRetired { get; set; }
    }
}