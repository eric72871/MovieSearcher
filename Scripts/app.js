(function () {
    ko.observable.fn.store = function () {
        var self = this;
        var oldValue = self();

        this.revert = function () {
            self(oldValue);
        }
        this.commit = function () {
            oldValue = self();
        }
        return this;
    }

    // Creates an observable version of the movie model.
    // Initialize with a JSON object fetched from the server.
    function movie(data) {
        var self = this;
        data = data || {};

        // Data from model
        self.Id = data.Id;
        self.Title = data.Title;
        self.Thumbnail = data.Thumbnail;
        self.Link = data.Link;

        // Local (client) data
        //self.editing = ko.observable(false);
    };

    var ViewModel = function () {
        var self = this;

        // View-model observables
        self.movies = ko.observableArray();
        self.error = ko.observable();
        self.searchTerm = ko.observable();

        // Adds a JSON array of movies to the view model.
        function addMovies(data) {
            var mapped = ko.utils.arrayMap(data, function (item) {
                return new movie(item);
            });
            self.movies(mapped);
        }

        // Callback for error responses from the server.
        function onError(error) {
            self.error('Error: ' + error.status + ' ' + error.statusText);
        }

        // Fetches a list of movies and updates the view model.
        self.getMovies = function () {
            self.searchTerm = $('#term').val();
            self.error(''); // Clear the error
            app.service.searchMovies(escape(self.searchTerm)).then(addMovies, onError);
        };

        // Initialize the app.
        //self.getMovies();
    }

    // Create the view model and tell Knockout to apply the data-bindings.
    ko.applyBindings(new ViewModel());
})();
