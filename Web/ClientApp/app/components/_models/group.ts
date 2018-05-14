import { Plans } from './plan';
import { Organization } from './organization';
export interface Group {
    groupId: string;
    organization: Array<Organization>
    name: string;
    number: string;
    plans: Array<Plans>;
    option: boolean;
}
export interface GroupGrid {
    option: number;
    groupId: string;
    organizationId: string;
    groupName: string;
    groupNumber: string;
    planName: string;
    networkCode: string;
}

