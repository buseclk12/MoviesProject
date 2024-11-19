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

        public async Task AddMovie(MovieModel model)
        {
            var director = new Director
            {
                Name = "Christopher",
                Surname = "Nolan",
                IsRetired = false
            };

            _context.Directors.Add(director);
            await _context.SaveChangesAsync();

            var movie = new Movie
            {
                Name = "Inception",
                ReleaseDate = new DateTime(2010, 7, 16),
                TotalRevenue = 829895144,
                DirectorId = director.Id // Bu ID, mevcut bir Director kaydını referans almalı
            };

            _context.Movies.Add(movie);
            await _context.SaveChangesAsync();

        }
    }
}