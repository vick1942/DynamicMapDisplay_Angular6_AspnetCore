import { Injectable } from '@angular/core'
import { BehaviorSubject } from "rxjs/BehaviorSubject";
import { Observable } from "rxjs/Observable";

@Injectable()
export class GlobalEventsManager {

    constructor() { }

    public _isUserLoggedIn: BehaviorSubject<Array<any>> = new BehaviorSubject<Array<any>>([]);
    public UserLoggedInEmitter: Observable<Array<any>> = this._isUserLoggedIn.asObservable();

    public _isConfirmationEntityIn: BehaviorSubject<any> = new BehaviorSubject<any>([]);
    public ConfirmationEntityInEmitter: Observable<any> = this._isConfirmationEntityIn.asObservable();

    isUserLoggedIn(ifLoggedIn: Array<any>) {
        this._isUserLoggedIn.next(ifLoggedIn);
    }

    isConfirmationEntityIn(ifLoggedIn: any) {
        this._isConfirmationEntityIn.next(ifLoggedIn);
    }
}