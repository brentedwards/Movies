using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Movies.Core.Messaging;
using Movies.Core.Models;

namespace Movies.Core.Tests.Messaging
{
	[TestClass()]
	public sealed class SearchMessageTests
	{
		[TestMethod()]
		public void CreateWithKeywords()
		{
			var keywords = Guid.NewGuid().ToString();

			var message = new SearchMessage(keywords);

			Assert.AreEqual(keywords, message.Keywords);
			Assert.IsNull(message.Genre);
			Assert.IsNull(message.Rating);
		}

		[TestMethod()]
		public void Create()
		{
			var keywords = Guid.NewGuid().ToString();
			var genre = Genres.Action;
			var rating = Ratings.G;

			var message = new SearchMessage(keywords, genre, rating);

			Assert.AreEqual(keywords, message.Keywords);
			Assert.AreEqual(genre, message.Genre);
			Assert.AreEqual(rating, message.Rating);
		}
	}
}
