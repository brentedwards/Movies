using System;
using System.Collections.Generic;
using Movies.Core.Models;

namespace Movies.Core.Repositories
{
	public interface IMovieRepository
	{
		IList<Movie> Load();
		IList<Movie> Search(String keywords, Genres? genre, Ratings? rating);
		Movie Save(Movie movie);
	}
}
