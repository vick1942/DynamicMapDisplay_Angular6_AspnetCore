import { ProviderService } from '../shared/_services/provider.service';
import { HttpUtilityService } from '../shared/_services/http-utility.service';
import {
    Component, Inject, Directive, forwardRef,
    Attribute, OnChanges, SimpleChanges, Input
} from '@angular/core';
import { Http, Headers, Response, Request } from '@angular/http';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/map';
import { Operator } from 'rxjs';
import { Group, GroupGrid } from '../_models/group'
import { Router, NavigationExtras } from '@angular/router';
import { GlobalEventsManager } from '../shared/_helpers/GlobalEventsManager';
import {
    NG_VALIDATORS, Validator,
    Validators, AbstractControl, ValidatorFn
} from '@angular/forms';

@Component({
    selector: 'home',
    templateUrl: './home.component.html',
    styleUrls: ['./home.component.css']

})
export class HomeComponent {

    public groupNameOrNumber: string;
    public planName: string;
    public emptyCheckGroup: boolean = false;
    public emptyCheckPlan: boolean = false;
    public groupData: GroupGrid[];
    public setNextNavigationUrl: string;
    public groupErrorText: string = "Group Name or Group Number is required";
    public PlanErrorText: string = "Plan Name is required";
    public spinner: boolean;
    result: Object;

    constructor(
        public _providerService: ProviderService,
        public _httpUtilityService: HttpUtilityService,
        public _http: Http,
        public _router: Router,
        public _globalEventsManager: GlobalEventsManager
    ) {

    }
    groupSubmit() {
        this.spinner = true;
        this.emptyCheckGroup = false;
        if (this.groupNameOrNumber == undefined || this.groupNameOrNumber == null || this.groupNameOrNumber.length == 0) {
            this.emptyCheckGroup = true;
            this.spinner = false;
            return false;
        }
        console.log('Search box group name or number: ' + this.groupNameOrNumber.toUpperCase());
        this._providerService.getGroupSubscribeDetails(this.groupNameOrNumber).subscribe(
            result => {
                let resultList = result.json() as GroupGrid[];
                if (resultList.length > 0) {
                    this.getDetails(result);
                } else {
                    this.emptyCheckGroup = true;
                    this.groupErrorText = "Group name or number does not exist";
                }
                this.spinner = false;
            }, error => {
                console.log("Error: ", error);
                this.emptyCheckGroup = true;
                this.spinner = false;
            }
        );
    }
    ClearErrorMsg(eve: any) {
        if (eve == 'group')
            this.emptyCheckGroup = false;
        else if (eve == 'plan')
            this.emptyCheckPlan = false;
    }
    planSubmit() {
        this.spinner = true;
        this.emptyCheckPlan = false;
        if (this.planName == undefined || this.planName == null || this.planName.length == 0) {
            this.emptyCheckPlan = true;
            this.spinner = false;
            return false;
        }
        console.log('Search box plan name: ' + this.planName.toUpperCase());
        this._providerService.getPlanSubscribeDetails(this.planName).subscribe(
            result => {
                let resultList = result.json() as GroupGrid[];
                if (resultList.length > 0) {
                    this.getDetails(result);
                } else {
                    this.emptyCheckPlan = true;
                    this.PlanErrorText = "Plan name does not exist";
                }
                this.spinner = false;
            }, error => {
                console.log("Error: ", error);
                this.emptyCheckPlan = true;
                this.spinner = false;
            }
        );
    }
    getDetails(result: any) {
        if (result.ok) {
            this.groupData = result.json() as GroupGrid[];
            let data = this.groupData;
            this._globalEventsManager._isUserLoggedIn.next(this.groupData);
            this._router.navigate(['confirmation-child']);
        }
    }
}
