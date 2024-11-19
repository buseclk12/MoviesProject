using System;
using System.Collections.Generic;
using Microsoft.Build.Framework;

namespace BLL.DAL
{
    public class Movie
    {
        public int Id { get; set; }
        
        public string Name { get; set; }
        public DateTime? ReleaseDate { get; set; } // Nullable to allow no value
        public decimal TotalRevenue { get; set; }
       
        //fOREIGN KEY
        public int DirectorId { get; set; }
        public Director Director { get; set; }
        
        public ICollection<MovieGenre>? MovieGenres { get; set; }
    }
}