import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpModule } from '@angular/http';
import { BrowserModule } from '@angular/platform-browser';

import { ProviderService } from './components/shared/_services/provider.service';
import { Config } from './components/shared/_helpers/config';
import { HttpUtilityService } from './components/shared/_services/http-utility.service';
import { GlobalEventsManager } from './components/shared/_helpers/GlobalEventsManager';

@NgModule({    
    providers: [
        ProviderService,
        Config,
        HttpUtilityService,
        GlobalEventsManager
    ],
    imports: [        
        HttpModule,
        CommonModule,
        BrowserModule  
    ]  
})
export class ServiceHub {
}