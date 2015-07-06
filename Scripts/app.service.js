window.app = window.todoApp || {};

window.app.service = (function () {
    var baseUri = '/api/movies/';
    var serviceUrls = {
        movies: function () { return baseUri; },
        bySearchTerm: function (searchTerm) { return baseUri + searchTerm; }
        //byId: function (id) { return baseUri + id; }
    }

    function ajaxRequest(type, url, data) {
        var options = {
            url: url,
            headers: {
                Accept: "application/json"
            },
            contentType: "application/json",
            cache: false,
            type: type,
            data: data ? ko.toJSON(data) : null
        };
        return $.ajax(options);
    }

    return {
        allMovies: function () {
            return ajaxRequest('get', serviceUrls.movies());
        },
        searchMovies: function (searchTerm) {
            return ajaxRequest('get', serviceUrls.bySearchTerm(searchTerm));
        }
    };
})();
