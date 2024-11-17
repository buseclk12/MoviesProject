using System.Collections.Generic;
using System.Linq;
using BLL.DAL;
using BLL.Models;
using Microsoft.EntityFrameworkCore;

namespace BLL.Services
{
    public class MovieService
    {
        private readonly AppDbContext _context;

        public MovieService(AppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<MovieModel> GetAllMovies()
        {
            return _context.Movies
                .Select(m => new MovieModel
                {
                    Id = m.Id,
                    Name = m.Name,
                    DirectorId = m.DirectorId
                }).ToList();
        }

        public void AddMovie(MovieModel model)
        {
            var movie = new Movie
            {
                Name = model.Name,
                DirectorId = model.DirectorId
            };
            _context.Movies.Add(movie);
            _context.SaveChanges();
        }
    }
}