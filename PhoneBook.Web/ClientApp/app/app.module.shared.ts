import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpModule } from '@angular/http';
import { RouterModule } from '@angular/router';
import { AccordionModule } from 'ngx-bootstrap/accordion';
import { PaginationModule } from 'ngx-bootstrap/pagination';

import { AppComponent } from './components/app/app.component';
import { NavMenuComponent } from './components/navmenu/navmenu.component';
import { HomeComponent } from './components/home/home.component';
import { PhoneBookComponent } from './components/phonebook/phonebook.component';
import { SearchFilterPipe } from "./components/phonebook/searchfilter.pipe";
import { NotificationsModule, NotificationsService } from 'angular4-notify';

@NgModule({
    declarations: [
        AppComponent,
        NavMenuComponent,
        PhoneBookComponent,
        HomeComponent,
        SearchFilterPipe
    ],
    providers: [
        NotificationsService
    ],
    imports: [
        AccordionModule.forRoot(),
        PaginationModule.forRoot(),
        CommonModule,
        HttpModule,
        ReactiveFormsModule,
        FormsModule,
        NotificationsModule,
        RouterModule.forRoot([
            { path: '', redirectTo: 'home', pathMatch: 'full' },
            { path: 'home', component: HomeComponent },
            { path: 'phone-book', component: PhoneBookComponent },
            { path: '**', redirectTo: 'home' }
        ])
    ]
})
export class AppModuleShared {
}
