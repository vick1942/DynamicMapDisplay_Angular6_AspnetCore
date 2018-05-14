import { ProviderService } from '../shared/_services/provider.service';
import { HttpUtilityService } from '../shared/_services/http-utility.service';
import { Component, Inject } from '@angular/core';
import { Http, Headers, Response, Request } from '@angular/http';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/map';
import { Operator } from 'rxjs';
import { Router } from '@angular/router';
import { Map } from '../_models/_map'
import { forEach } from '@angular/router/src/utils/collection';
import { Group, GroupGrid } from '../_models/group'
import { Mile, Speciality, Facility, DropdownList } from '../_models/dropdown'
import { GlobalEventsManager } from '../shared/_helpers/GlobalEventsManager';

@Component({
    selector: 'result',
    templateUrl: './result.component.html',
    styleUrls: ['./result.component.css']
})
export class ResultComponent {
    public groupNameOrNumber: string;
    public planName: string;
    public maps = new Array<Map>();
    public mapmodels = new Map();
    public groupData: Group[];
    public zoom: number = 15;
    public groupCollection: any;
    public maping = new Array<Map>();
    public model: number = this.maping.length;
    result: Object;
    public miles: Mile[];
    public specialities: DropdownList[];
    public facilities: DropdownList[];
    public mile: Mile;
    public selectedMile: any;
    public selectedSpeciality: any
    public selectedFacility: any
    public networkCode: string
    public zipCode: string = "";
    public pageNumber: number = 1;
    public isFilterApplied: boolean = false;
    public providerOrFacilityName: string = "";
    public isRecordExist: boolean = false;
    public userProfile: any;
    public providerId: number = 0;
    public spinner: boolean;
    constructor(
        public _providerService: ProviderService,
        public _globalEventsManager: GlobalEventsManager,
        public _httpUtilityService: HttpUtilityService,
        public _http: Http,
        public _router: Router
    ) {
        this.pageNumber = 1;
        try {
            this.getInitialResults();
            this.showDefaultDetails();
        }
        catch (e) {

        }
    }
    getInitialResults() {
        this.spinner = true;
        this._globalEventsManager.ConfirmationEntityInEmitter.subscribe((groupGrid: GroupGrid) => {
            if (groupGrid != null) {
                if (groupGrid.networkCode != undefined) {
                    this.groupCollection = groupGrid;
                    this.getDefaultDropdown(groupGrid.networkCode);
                } else {
                    this._router.navigate(['home']);
                }
            }
        });

        this.miles = [
            {
                id: 10,
                value: 'Within 10 miles'
            },
            {
                id: 20,
                value: 'Within 20 miles'
            },
            {
                id: 30,
                value: 'Within 30 miles'
            },
            {
                id: 40,
                value: 'Within 40 miles'
            },
        ];
        this.selectedMile = this.miles[0].id;
        console.log(this.miles);
    }

    getDefaultDropdown(collectionName: string) {
        this._providerService.getAllSpecialityList(this.pageNumber, collectionName).subscribe(
            result => {
                let dropdownSpecialityList = result.json() as DropdownList[];
                console.log(dropdownSpecialityList);
                this.specialities = dropdownSpecialityList;
            }, error => {
                console.log("Error: ", error);
            }
        );
        this._providerService.getAllFacilityList(this.pageNumber, collectionName).subscribe(
            result => {
                let dropdownFacilityList = result.json() as DropdownList[];
                console.log(dropdownFacilityList);
                this.facilities = dropdownFacilityList;
            }, error => {
                console.log("Error: ", error);
            }
        );
    }

    showDefaultDetails() {
        if (this.groupCollection != undefined) {
            this.networkCode = this.groupCollection.networkCode;
            this._providerService.getAllAddressesSubscribe(this.pageNumber, this.networkCode).subscribe(
                result => {
                    let maps = result.json() as Map[];
                    if (maps != null) {
                        if (maps.length > 0) {
                            this.addItemsToMap(maps);
                            console.log(maps);
                        }
                    } else {
                        this.isRecordExist = true;
                    }
                    this.spinner = false;
                }, error => {
                    console.log("Error: ", error);
                    this.spinner = false;
                }
            );
        } else {
            this._router.navigate(['home']);
        }
    }

    searchDetails() {
        this.spinner = true;
        this.isFilterApplied = true;
        this.pageNumber = 1;
        this.providerId = 0;
        this.maping = new Array<Map>();
        this.showSearchDetails();

    }

    showSearchDetails() {
        this.isRecordExist = false;
        let speciality: string = "";
        let facility: string = "";

        var selectedSpeciality = this.selectedSpeciality;
        if (typeof selectedSpeciality == 'undefined') {
            selectedSpeciality = "";
        }
        var selectedFacility = this.selectedFacility;
        if (typeof selectedFacility == 'undefined') {
            selectedFacility = "";
        }
        console.log("ZipCode:", this.zipCode, "  Miles:", this.selectedMile, "  pageNumber:", this.pageNumber, "  speciality:", selectedSpeciality, "  facility:", selectedFacility, "  provideOrFacilityName:", this.providerOrFacilityName, "  providerId:", this.providerId);
        this._providerService.getAllAddressesWithFilterSubscribe(this.networkCode, this.zipCode, this.selectedMile, this.pageNumber, encodeURIComponent(selectedSpeciality), encodeURIComponent(selectedFacility), encodeURIComponent(this.providerOrFacilityName), this.providerId).subscribe(
            result => {
                let maps = result.json() as Map[];
                if (maps != null) {
                    if (maps.length > 0) {
                        this.addItemsToMap(maps);
                        console.log(maps);
                        let lastMap = this.maping[this.maping.length - 1];
                        this.providerId = lastMap.id;
                    } else {
                        this.isRecordExist = (this.pageNumber > 1) ? false : true;
                    }
                } else {
                    this.isRecordExist = (this.pageNumber > 1) ? false : true;
                }
                this.spinner = false;
            }, error => {
                console.log("Error: ", error);
                this.spinner = false;
            }
        );
    }

    addItemsToMap(maps: Map[]) {
        this.spinner = true;
        if (maps.length > 0) {
            for (let index = 0; index < maps.length; index++) {
                this.maping.push(maps[index]);
            }
            this.model = this.maping.length;
        }
        this.spinner = false;
    }

    onScrollDown(ev: any) {
        this.spinner = true;
        console.log('scrolled down!!', ev);
        this.isRecordExist = false;
        this.pageNumber++;
        if (this.isFilterApplied) {
            this.showSearchDetails();
        }
        else {
            this.showDefaultDetails();
        }
    }
}