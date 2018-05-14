import { Config } from '../_helpers/config';
import { Http, Headers, URLSearchParams, Response, RequestOptions} from '@angular/http';
import { Injectable, Inject } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { Router } from '@angular/router';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/catch';
import 'rxjs/add/operator/finally';
import 'rxjs/add/operator/concatAll';
import 'rxjs/add/operator/shareReplay';


@Injectable()
export class HttpUtilityService {
    public baseApiUrl: string;

    constructor(
        public config: Config,
        public http: Http,
        public router: Router,
        @Inject('BASE_URL') baseUrl: string
    ) {
        this.baseApiUrl = baseUrl + 'api/';
    }
    /**
     * Post
     * @param url
     * @param body
     * @param options
     */
    public post(url: string, body: any, options?: Http) {
        return this.http.post(this.getApiUrl(url), body)
            .catch((e) => this.handleError(e));
    }
    /**
     * delete
     * @param url
     * @param options
     */
    public delete(url: string, options?: Http) {
        return this.http.delete(this.getApiUrl(url))
            .catch((e) => this.handleError(e));
    }
    /**
     * get
     * @param url
     * @param options
     */
    get(url: string, options?: URLSearchParams) {
        //startLoader();
       
        //if (!options) {
        //    options = this.getDefaultHeaders();
        //}

        return this.http.get(this.getApiUrl(url))
            .map(response => response.json())
            .catch(this.handleError)
            .finally(() => {
                //stopLoader();
            });
    }

    getSubscribe(url: string, options?: URLSearchParams) {
        return this.http.get(this.getApiUrl(url)
        );

    }
    /**
     * getFile
     * @param url
     * @param options
     */
    public getFile(url: string, options?: Http) {
        window.open(this.getApiUrl(url));
    }

    /**
     * handleError
     * @param response
     */
    private handleError(errorResponse: Response) {
        // in a real world app, we may send the server to some remote logging infrastructure
        // instead of just logging it to the console
        let errorStatus = '404'
        console.log("error occured.", errorResponse);

        if (errorResponse instanceof Response) {
            errorStatus = errorResponse.status > 0 ? errorResponse.status.toLocaleString() : '404';
        }
        if (errorStatus !== '401' && errorStatus !== '400') {

            // Redirect the user after login
            //return this.router.navigate(['/error']);
        }
        return Observable.throw(errorResponse);

    }


    /**
     * getApiUrl
     * @param url
     */
    private getApiUrl(url : string) {
        return this.baseApiUrl + url;
    }
}