﻿namespace MoviesApi.DTO
{
    public class MovieCastDTO
    {
        public string Title { get; set; }

        public int Year { get; set; }

        public double Rate { get; set; }

        [MaxLength(2500)]
        public string Storeline { get; set; }
    }
}
