using BLL.DAL;
using BLL.Models;

namespace BLL.Services;

public class DirectorService
{
    private readonly AppDbContext _context;

    public DirectorService(AppDbContext context)
    {
        _context = context;
    }

    public IEnumerable<DirectorModel> GetAllDirectors()
    {
        return _context.Directors
            .Select(d => new DirectorModel
            {
                Id = d.Id,
                Name = d.Name,
                Surname = d.Surname
            }).ToList();
    }

    public void AddDirector(DirectorModel model)
    {
        var director = new Director
        {
            Name = model.Name,
            Surname = model.Surname
        };
        _context.Directors.Add(director);
        _context.SaveChanges();
    }
}
