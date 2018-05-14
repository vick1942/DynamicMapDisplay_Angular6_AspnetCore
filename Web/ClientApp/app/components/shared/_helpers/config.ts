import { Http } from '@angular/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { environment } from './environment';
declare var prodModeOn: string;

@Injectable()
export class Config {
    public _config: any;
    public _env = environment.production ? "production" : "development";

    constructor(public http: Http) {
    }
    public load() {
        // json files will be loaded here
        return new Promise((resolve, reject) => {
            this.http.get(`json/settings.${this._env}.json`)
                .subscribe((data) => {
                    this._config = data;
                    resolve(true);
                },
                (error) => {
                    console.error(error);
                    return Observable.throw(error.json().error || 'Server error');
                });
        });
    }
    public getEnv(key: any) {
        return this._env[key];
    }
    public getByKey(key: any) {
        return this._config[key];
    }
}
