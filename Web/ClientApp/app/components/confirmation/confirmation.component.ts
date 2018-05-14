import { ProviderService } from '../shared/_services/provider.service';
import { HttpUtilityService } from '../shared/_services/http-utility.service';
import { Component, Inject, Input } from '@angular/core';
import { Http, Headers, Response, Request } from '@angular/http';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/map';
import { Operator } from 'rxjs';
import { Group, GroupGrid } from '../_models/group'
import { Router, ActivatedRoute } from '@angular/router';
import { GlobalEventsManager } from '../shared/_helpers/GlobalEventsManager';
import * as _ from "lodash";
@Component({
    selector: 'confirmation-child',
    templateUrl: './confirmation.component.html',
    styleUrls: ['./confirmation.component.css']
})
export class ConfirmationComponent {
    public groupCollection1: GroupGrid[] = [];
    public groupNameOrNumber: string;
    public planName: string;
    public buttonModel: boolean = false;
    public groupData: Group[];
    public groupCollection: GroupGrid;
    result: Object;
    constructor(
        private _activatedRoute: ActivatedRoute,
        private _providerService: ProviderService,
        private _httpUtilityService: HttpUtilityService,
        private _http: Http,
        private _router: Router,
        public _globalEventsManager: GlobalEventsManager,
        @Inject('BASE_URL') baseUrl: string
    ) {
    }

    ngOnInit() {
        try {
            this._globalEventsManager.UserLoggedInEmitter.subscribe((groupGrid: Array<GroupGrid>) => {
                if (groupGrid.length > 0) {     
                    if (groupGrid.length > 0) {
                        this.groupCollection1 = groupGrid;
                        if (this.groupCollection1) {
                        }
                    } else {
                        this._router.navigate(['home']);
                    }
                } else {
                    this._router.navigate(['home']);
                }
            });
            console.log(this.groupCollection1);
        }
        catch (e) {

        }
    }

    onSelectionChange(entry: any) {
        this.groupCollection = entry;
        this.buttonModel = true;
    }
    confirmationSubmit() {
        this._globalEventsManager._isConfirmationEntityIn.next(this.groupCollection);
        this._router.navigate(['legal']);
    }
    isRadioSelected() {
    }
    isRadioCheck(value: any) {
        var test = event;
    }
}
