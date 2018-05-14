import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpModule } from '@angular/http';
import { RouterModule } from '@angular/router';
import { AppComponent } from './components/app/app.component';
import { HomeComponent } from './components/home/home.component';
import { ConfirmationComponent } from './components/confirmation/confirmation.component';
import { ServiceHub } from './service-hub.module';
import { LegalComponent } from './components/legal/legal.component'
import { ResultComponent } from './components/result/result.component'
import { AgmCoreModule } from '@agm/core';
import { InfiniteScrollModule } from 'ngx-infinite-scroll';
@NgModule({
    declarations: [
        AppComponent,
        HomeComponent,
        LegalComponent,
        ConfirmationComponent,
        ResultComponent
    ],
    imports: [
        CommonModule,
        HttpModule,
        FormsModule,
        InfiniteScrollModule,
        AgmCoreModule.forRoot({
            apiKey: 'AIzaSyCXs4LLwZZExw3JIoR89f7yKpLWq6KS1ro'
        }),
        ServiceHub,
        RouterModule.forRoot([
            { path: '', redirectTo: 'home', pathMatch: 'full' },
            { path: 'home', component: HomeComponent },
            { path: 'confirmation-child', component: ConfirmationComponent },
            { path: 'legal', component: LegalComponent },
            { path: 'result', component: ResultComponent },
            { path: '**', redirectTo: 'home' }
        ])
    ],
    exports: [AppComponent,
        HomeComponent,
        LegalComponent,
        ConfirmationComponent,
        ResultComponent
]
})
export class AppModuleShared {
}
