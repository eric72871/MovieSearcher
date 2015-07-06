using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using MovieSPA.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Web.Http;


namespace MovieSPA.Controllers
{
    public class MoviesController : ApiController
    {
        //// GET api/Movies
        //public List<OmdbSearchResultMovie> GetMovies()
        //{
        //    var client = new WebClient();
        //    var json = client.DownloadString("http://www.omdbapi.com/?s=Game&type=movie");
        //    var search = JsonConvert.DeserializeObject <OmdbSearchResult>(json);

        //    return search.Movie;
        //}

        // GET api/Movies/searchTerm
        //[ResponseType(typeof(List<OmdbSearchResultMovie>))]
        public List<Movie> GetMovies(string searchTerm)
        {
            var results = new List<Movie>();

            results.AddRange(GetOmdbApiMovies(searchTerm));
            results.AddRange(GetYoutubeVideos(searchTerm));

            return results;
        }


        private IEnumerable<Movie> GetOmdbApiMovies(string searchTerm)
        {
            var movies = new List<Movie>();

            try
            {
                var client = new WebClient();
                var json = client.DownloadString("http://www.omdbapi.com/?s=" + searchTerm + "&type=movie");

                if (!string.IsNullOrEmpty(json))
                {
                    var omdbSearchResults = JsonConvert.DeserializeObject<OmdbSearchResult>(json);

                    foreach (var m in omdbSearchResults.Movie)
                    {
                        json = client.DownloadString("http://www.omdbapi.com/?i=" + m.Id);
                        var omdb = JsonConvert.DeserializeObject<OmdbMovie>(json);

                        if (string.IsNullOrEmpty(json)) continue;

                        movies.Add(new Movie
                        {
                            Id = omdb.ImdbId,
                            Title = omdb.Title,
                            Thumbnail = omdb.Poster,
                            Link = "http://www.imdb.com/title/" + omdb.ImdbId + "/"
                        });
                    }
                }
            }
            catch (WebException)
            {
                //exception handling here
            }
            catch (Exception)
            {
                //exception handling here
            }

            return movies;
        }



        private IEnumerable<Movie> GetYoutubeVideos(string searchTerm)
        {
            var movies = new List<Movie>();

            try
            {
                var youtubeService = new YouTubeService(new BaseClientService.Initializer()
                {
                    ApiKey = "AIzaSyBOqb0Fb2X3qvqcV6y2Wpn_jZWlh8lGSD8",
                    ApplicationName = this.GetType().ToString()
                });

                var searchListRequest = youtubeService.Search.List("snippet");
                searchListRequest.Q = searchTerm;
                searchListRequest.MaxResults = 10;

                // Call the search.list method to retrieve results matching the specified query term.
                var searchListResponse = searchListRequest.Execute();

                foreach (var searchResult in searchListResponse.Items)
                {
                    if (searchResult.Id.Kind != "youtube#video") continue;

                    var movie = new Movie { Id = searchResult.Id.VideoId, Title = searchResult.Snippet.Title, Link = "https://www.youtube.com/watch?v=" + searchResult.Id.VideoId };

                    if (searchResult.Snippet.Thumbnails.Standard != null)
                    {
                        movie.Thumbnail = searchResult.Snippet.Thumbnails.Standard.Url;
                    }
                    else if (searchResult.Snippet.Thumbnails.Medium != null)
                    {
                        movie.Thumbnail = searchResult.Snippet.Thumbnails.Medium.Url;
                    }
                    else if (searchResult.Snippet.Thumbnails.High != null)
                    {
                        movie.Thumbnail = searchResult.Snippet.Thumbnails.High.Url;
                    }
                    else if (searchResult.Snippet.Thumbnails.Maxres != null)
                    {
                        movie.Thumbnail = searchResult.Snippet.Thumbnails.Maxres.Url;
                    }

                    movies.Add(movie);
                }
            }
            catch (Exception)
            {
                //exception handling here
            }

            return movies;
        }
    }
}