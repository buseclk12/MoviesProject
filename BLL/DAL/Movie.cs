using System;
using System.Collections.Generic;
using Microsoft.Build.Framework;

namespace BLL.DAL
{
    public class Movie
    {
        public int Id { get; set; }
        
        public string Name { get; set; }
        public DateTime? ReleaseDate { get; set; } 
        public decimal TotalRevenue { get; set; }
       
        public int DirectorId { get; set; }
        public Director Director { get; set; }
        
        public ICollection<MovieGenre>? MovieGenres { get; set; }
    }
}