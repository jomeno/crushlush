class DataService {
    private http: ng.IHttpService;
    public q: ng.IQService;
    public toast: Toast;
    public sce: ng.ISCEService;
    Api: Api;

    constructor($http: ng.IHttpService, $q: ng.IQService, $sce: ng.ISCEService, toaster) {
        this.http = $http;
        this.q = $q;
        this.sce = $sce;
        this.toast = new Toast(toaster);
        this.initApi();
    }

    get<T>(url: string) {
        // show loader
        this.toast.loader(true);
        var defer = this.q.defer<T>();

        this.http.get<Operation<T>>(url).then(response=> {
            this.toast.loader(false);
            var operation: Operation<T> = response.data;
            if (operation.Succeeded == true) {
                this.toast.pop(true, operation);
                defer.resolve(operation.Result);
            } else {
                this.toast.pop(true, operation);
                defer.reject(operation.Message);
            }
        }).catch(e=> {
            this.toast.loader(false);
            this.toast.pop(true, e);
            defer.reject(e);
        }).finally(() => {
            this.toast.loader(false);
        });

        return defer.promise;
    }

    post<T>(url: string, model: any) {
        // show loader
        this.toast.loader(true);
        var defer = this.q.defer<T>();

        this.http.post<Operation<T>>(url, model).then(response => {
            this.toast.loader(false);

            var operation: Operation<T> = response.data;
            if (operation.Succeeded == true) {
                this.toast.pop(true, operation);
                defer.resolve(response.data.Result);
            } else {
                this.toast.pop(true, operation);
                defer.reject(response.data.Message);
            }

        }).catch(e => {
            this.toast.loader(false);
            this.toast.pop(true, e);
            defer.reject(e);

        }).finally(() => {
            // hide loader
            this.toast.loader(false);
        });

        return defer.promise;
    }

    put<T>(url: string, model: any) {
        // show loader
        this.toast.loader(true);
        var defer = this.q.defer<T>();

        this.http.put<Operation<T>>(url, model).then(response => {
            this.toast.loader(false);
            var operation: Operation<T> = response.data;

            if (operation.Succeeded == true) {
                this.toast.pop(true, operation);
                defer.resolve(response.data.Result);
            } else {
                this.toast.pop(true, operation);
                defer.reject(response.data.Message);
            }

        }).catch(e => {
            this.toast.loader(false);
            this.toast.pop(true, e);
            defer.reject(e);

        }).finally(() => {
            // hide loader
            this.toast.loader(false);
        });

        return defer.promise;
    }

    delete<T>(url: string) {
        // show loader
        this.toast.loader(true);
        var defer = this.q.defer<T>();

        this.http.delete<Operation<T>>(url).then(response => {
            this.toast.loader(false);
            var operation: Operation<T> = response.data;

            if (operation.Succeeded == true) {
                this.toast.pop(true, operation);
                defer.resolve(response.data.Result);
            }
            else {
                this.toast.pop(true, operation);
                defer.reject(response.data.Message);
            }

        }).catch(e => {
            this.toast.loader(false);
            this.toast.pop(true, e);
            defer.reject(e);

        }).finally(() => {
            // hide loader
            this.toast.loader(false);
        });

        return defer.promise;
    }

    deletebatch<T>(url: string, model: number[]) {
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
        var defer = this.q.defer<T>();

        this.http.delete<Operation<T>>(url, config).then(response => {
            this.toast.loader(false);
            var operation: Operation<T> = response.data;

            if (operation.Succeeded == true) {
                this.toast.pop(true, operation);
                defer.resolve(response.data.Result);
            }
            else {
                this.toast.pop(true, operation);
                defer.reject(response.data.Message);
            }

        }).catch(e => {
            this.toast.loader(false);
            this.toast.pop(true, e);
            defer.reject(e);

        }).finally(() => {
            // hide loader
            this.toast.loader(false);
        });

        return defer.promise;
    }

    initApi() {

        this.Api = <Api>{};
        var base = "/api/";
        this.Api.Playlists = base + "playlists";
        this.Api.PlaylistTracks = base + "playlisttracks";

    }
}

class Toast {
    delay: number;
    toast: any;

    constructor(toaster) {
        this.toast = toaster;
        this.delay = 10000;
    }

    loader(visible: boolean) {
        if (visible) {
            jQuery(".preloader").css("visibility", "visible");
        }
        else {
            jQuery(".preloader").css("visibility", "hidden");
        }
    }

    pop(visible: boolean, operation: Operation<any>) {

        if (operation != null && operation.Message != null) {
            if (operation.Succeeded) {
                this.toast.pop('success', '', operation.Message);
            } else {
                this.toast.pop('error', '', operation.Message);
            }

        }
    }

}

app.service("_data", DataService);