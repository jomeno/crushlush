var DataService = (function () {
    function DataService($http, $q, $sce, toaster) {
        this.http = $http;
        this.q = $q;
        this.sce = $sce;
        this.toast = new Toast(toaster);
        this.initApi();
    }
    DataService.prototype.get = function (url) {
        var _this = this;
        // show loader
        this.toast.loader(true);
        var defer = this.q.defer();
        this.http.get(url).then(function (response) {
            _this.toast.loader(false);
            var operation = response.data;
            if (operation.Succeeded == true) {
                _this.toast.pop(true, operation);
                defer.resolve(operation.Result);
            }
            else {
                _this.toast.pop(true, operation);
                defer.reject(operation.Message);
            }
        }).catch(function (e) {
            _this.toast.loader(false);
            _this.toast.pop(true, e);
            defer.reject(e);
        }).finally(function () {
            _this.toast.loader(false);
        });
        return defer.promise;
    };
    DataService.prototype.post = function (url, model) {
        var _this = this;
        // show loader
        this.toast.loader(true);
        var defer = this.q.defer();
        this.http.post(url, model).then(function (response) {
            _this.toast.loader(false);
            var operation = response.data;
            if (operation.Succeeded == true) {
                _this.toast.pop(true, operation);
                defer.resolve(response.data.Result);
            }
            else {
                _this.toast.pop(true, operation);
                defer.reject(response.data.Message);
            }
        }).catch(function (e) {
            _this.toast.loader(false);
            _this.toast.pop(true, e);
            defer.reject(e);
        }).finally(function () {
            // hide loader
            _this.toast.loader(false);
        });
        return defer.promise;
    };
    DataService.prototype.put = function (url, model) {
        var _this = this;
        // show loader
        this.toast.loader(true);
        var defer = this.q.defer();
        this.http.put(url, model).then(function (response) {
            _this.toast.loader(false);
            var operation = response.data;
            if (operation.Succeeded == true) {
                _this.toast.pop(true, operation);
                defer.resolve(response.data.Result);
            }
            else {
                _this.toast.pop(true, operation);
                defer.reject(response.data.Message);
            }
        }).catch(function (e) {
            _this.toast.loader(false);
            _this.toast.pop(true, e);
            defer.reject(e);
        }).finally(function () {
            // hide loader
            _this.toast.loader(false);
        });
        return defer.promise;
    };
    DataService.prototype.delete = function (url) {
        var _this = this;
        // show loader
        this.toast.loader(true);
        var defer = this.q.defer();
        this.http.delete(url).then(function (response) {
            _this.toast.loader(false);
            var operation = response.data;
            if (operation.Succeeded == true) {
                _this.toast.pop(true, operation);
                defer.resolve(response.data.Result);
            }
            else {
                _this.toast.pop(true, operation);
                defer.reject(response.data.Message);
            }
        }).catch(function (e) {
            _this.toast.loader(false);
            _this.toast.pop(true, e);
            defer.reject(e);
        }).finally(function () {
            // hide loader
            _this.toast.loader(false);
        });
        return defer.promise;
    };
    DataService.prototype.deletebatch = function (url, model) {
        var _this = this;
        // config contains a data property for the model
        var config = {
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            },
            'type': 'POST',
            'data': model,
        };
        // show loader
        this.toast.loader(true);
        var defer = this.q.defer();
        this.http.delete(url, config).then(function (response) {
            _this.toast.loader(false);
            var operation = response.data;
            if (operation.Succeeded == true) {
                _this.toast.pop(true, operation);
                defer.resolve(response.data.Result);
            }
            else {
                _this.toast.pop(true, operation);
                defer.reject(response.data.Message);
            }
        }).catch(function (e) {
            _this.toast.loader(false);
            _this.toast.pop(true, e);
            defer.reject(e);
        }).finally(function () {
            // hide loader
            _this.toast.loader(false);
        });
        return defer.promise;
    };
    DataService.prototype.initApi = function () {
        this.Api = {};
        var base = "/api/";
        this.Api.Playlists = base + "playlists";
        this.Api.Playlist = function (params) { return base + "playlists/" + params[0]; };
        this.Api.PlaylistTracks = base + "playlisttracks";
        this.Api.DeletePlaylist = function (params) { return base + "playlists/" + params[0]; };
    };
    return DataService;
})();
var Toast = (function () {
    function Toast(toaster) {
        this.toast = toaster;
        this.delay = 10000;
    }
    Toast.prototype.loader = function (visible) {
        if (visible) {
            jQuery(".preloader").css("visibility", "visible");
        }
        else {
            jQuery(".preloader").css("visibility", "hidden");
        }
    };
    Toast.prototype.pop = function (visible, operation) {
        if (operation != null && operation.Message != null) {
            if (operation.Succeeded) {
                this.toast.pop('success', '', operation.Message);
            }
            else {
                this.toast.pop('error', '', operation.Message);
            }
        }
    };
    return Toast;
})();
app.service("_data", DataService);
//# sourceMappingURL=DataService.js.map