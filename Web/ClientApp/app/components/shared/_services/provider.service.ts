import { Injectable } from "@angular/core";
import { Http, Headers, Response, URLSearchParams, RequestOptions } from "@angular/http";
import 'rxjs/add/operator/map';
import { Observable } from "rxjs/Observable";
import { HttpUtilityService } from '../_services/http-utility.service';


@Injectable()
export class ProviderService {

    constructor(
        public _httpUtility: HttpUtilityService
    ) { }

    getGroupDetails(nameOrNumber: string) {
        let params: URLSearchParams = new URLSearchParams();
        params.set('name', JSON.stringify(name));

        return this._httpUtility.get('Provider/GetGroupDetails?nameOrNumber=' + nameOrNumber);
    }
    getPlanDetails(name: string) {
        return this._httpUtility.get('Provider/GetPlanDetails?name=' + name);
    }
    getPlanSubscribeDetails(name: string) {
        return this._httpUtility.getSubscribe('Provider/GetPlanDetails?name=' + name);
    }
    getGroupSubscribeDetails(nameOrNumber: string) {
        return this._httpUtility.getSubscribe('Provider/GetGroupDetails?nameOrNumber=' + nameOrNumber);
    }
    getAllAddressesWithInRange(address: string, rangeInMiles: number, pageNumber: number) {
        return this._httpUtility.get('Provider/GetAllAddressesWithInRange?address=' + address + '&rangeInMiles=' + rangeInMiles + '&pageNumber=' + pageNumber);
    }
    getAllAddresses(pageNumber: number) {
        return this._httpUtility.get('Provider/GetAllAddresses?pageNumber=' + pageNumber);
    }
    getAllAddressesSubscribe(pageNumber: number, networkCode: string) {
        return this._httpUtility.getSubscribe('Provider/GetAllOrganizationDetails?pageNumber=' + pageNumber + '&networkCode=' + networkCode);
    }
    getAllAddressesWithFilterSubscribe(networkCode: string, zipCode: string, miles: any, pageNumber: number, specialization: any, facility: any, providerOrFacilityName: string, providerId: number) {
        return this._httpUtility.getSubscribe('Provider/GetFilteredOrganizationDetails?networkCode=' + networkCode + '&zipCode=' + zipCode + '&miles=' + miles + '&pageNumber=' + pageNumber + '&specialization=' + specialization + '&facility=' + facility + '&providerOrFacilityName=' + providerOrFacilityName + '&providerId=' + providerId);
    }
    getAllSpecialityList(pageNumber: number, collectionName: string) {
        return this._httpUtility.getSubscribe('Provider/GetAllSpecialityList?pageNumber=' + pageNumber + '&collectionName=' + collectionName);
    }
    getAllFacilityList(pageNumber: number, collectionName: string) {
        return this._httpUtility.getSubscribe('Provider/GetAllFacilityList?pageNumber=' + pageNumber + '&collectionName=' + collectionName);
    }
}