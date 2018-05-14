import { ProviderService } from '../shared/_services/provider.service';
import { HttpUtilityService } from '../shared/_services/http-utility.service';
import { Component, Inject } from '@angular/core';
import { Http, Headers, Response, Request } from '@angular/http';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/map';
import { Operator } from 'rxjs';
import { Router } from '@angular/router';
import { Group, GroupGrid } from '../_models/group'
import { GlobalEventsManager } from '../shared/_helpers/GlobalEventsManager';

@Component({
    selector: 'legal',
    templateUrl: './legal.component.html',
    styleUrls: ['./legal.component.css']
})
export class LegalComponent {
    public groupNameOrNumber: string;
    public planName: string;
    public groupData: Group[];
    public groupCollection: GroupGrid;
    result: Object;
    constructor(
        public _providerService: ProviderService,
        public _httpUtilityService: HttpUtilityService,
        public _globalEventsManager: GlobalEventsManager,
        public _http: Http,
        public _router: Router
    ) {
    }
    ngOnInit() {
        try {
            var test = "hello";
            console.log(test);

            this._globalEventsManager.ConfirmationEntityInEmitter.subscribe((groupGrid: GroupGrid) => {
                if (groupGrid != null) {
                    if (groupGrid.networkCode != undefined) {
                        this.groupCollection = groupGrid;
                        if (this.groupCollection) {
                        }
                    } else {
                        this._router.navigate(['home']);
                    }
                }
            });
        }
        catch (e) {

        }
    }
    legalSubmit() {
        this._router.navigate(['result']);
    }
}