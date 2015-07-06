using System.Collections.Generic;
using Newtonsoft.Json;

namespace MovieSPA.Models
{
    public class OmdbSearchResult
    {
        [JsonProperty("Search")]
        public List<OmdbSearchResultMovie> Movie { get; set; }
    }

    [JsonObject(MemberSerialization.OptIn)]
    public class OmdbSearchResultMovie
    {
        [JsonProperty("imdbID")]
        public string Id { get; set; }
        [JsonProperty("Title")]
        public string Title { get; set; }
        [JsonProperty("Year")]
        public string Year { get; set; }
        [JsonProperty("Type")]
        public string Type { get; set; }
    }
}