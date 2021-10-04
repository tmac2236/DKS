import { BrowserModule } from "@angular/platform-browser";
import { NgModule } from "@angular/core";
import {
  LocationStrategy,
  HashLocationStrategy,
  DatePipe,
} from "@angular/common";
import { BrowserAnimationsModule } from "@angular/platform-browser/animations";

import { PerfectScrollbarModule } from "ngx-perfect-scrollbar";
import { PerfectScrollbarConfigInterface } from "ngx-perfect-scrollbar";

const DEFAULT_PERFECT_SCROLLBAR_CONFIG: PerfectScrollbarConfigInterface = {
  suppressScrollX: true,
};

import { AppComponent } from "./app.component";

// Import containers
import { DefaultLayoutComponent } from "./containers";

import { P404Component } from "./views/error/404.component";
import { P500Component } from "./views/error/500.component";
import { PictureComponent } from "./views/picture/picture.component";
import { F205TransComponent } from "./views/dks/F205/F205-trans/F205-trans.component";
import { F340Component } from "./views/dks/F340/F340.component";
import { F340PpdComponent } from "./views/dks/F340/F340-ppd/F340-ppd.component";
import { F428Component } from "./views/dks/F428/F428.component";
import { F428EditComponent } from "./views/dks/F428/F428-edit/F428-edit.component";


import { AuthService } from "../../src/app/core/_services/auth.service";
import { AlertifyService } from "../../src/app/core/_services/alertify.service";
import { AuthGuard } from "../../src/app/core/_guards/auth.guard";
import { ErrorInterceptorProvider } from "../../src/app/core/_services/error.interceptor";
import { NgxSpinnerModule } from "ngx-spinner";

const APP_CONTAINERS = [DefaultLayoutComponent];

import {
  AppAsideModule,
  AppBreadcrumbModule,
  AppHeaderModule,
  AppFooterModule,
  AppSidebarModule,
} from "@coreui/angular";

// Import routing module
import { AppRoutingModule } from "./app.routing";

// Import 3rd party components
import { BsDropdownModule } from "ngx-bootstrap/dropdown";
import { TabsModule } from "ngx-bootstrap/tabs";
import { TooltipModule } from 'ngx-bootstrap/tooltip';

import { ChartsModule } from "ng2-charts";
import { HttpClient, HttpClientModule } from "@angular/common/http";
import { HomePageComponent } from "./views/home-page/home-page.component";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { NgImageSliderModule } from "ng-image-slider";
import { DictionaryComponent } from "./views/dictionary/dictionary.component";
import { TranslateLoader, TranslateModule } from "@ngx-translate/core";
import { TranslateHttpLoader } from "@ngx-translate/http-loader";
import { DataTablesModule } from "angular-datatables";
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { ModalModule } from "ngx-bootstrap/modal";
import { AuthGuardRole } from "./core/_guards/auth.guard-role";
import { NgSelectModule } from "@ng-select/ng-select";
import { DtrFgtResultComponentComponent } from "./views/dks/dtr-fgt-result-component/dtr-fgt-result-component.component";
import { DtrFgtResultReportComponent } from "./views/dks/dtr-fgt-result-report/dtr-fgt-result-report.component";
import { DtrVStandardComponent } from "./views/dks/dtr-v-standard/dtr-v-standard.component";
import { DtrVStandardListComponent } from "./views/dks/dtr-v-standard-list/dtr-v-standard-list.component";
import { CovidComponent } from "./views/covid/covid.component";
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';

//載入 "/assets/i18n/[lang].json" 語系檔
export function createTranslateLoader(http: HttpClient) {
  return new TranslateHttpLoader(http, "./assets/i18n/", ".json");
}
@NgModule({
  imports: [
    NgxSpinnerModule,
    BrowserModule,
    HttpClientModule,
    BrowserAnimationsModule,   //ngx-dropdown
    BsDropdownModule.forRoot(),//ngx-dropdown
    TranslateModule.forRoot({  // I18N
      loader: {
        provide: TranslateLoader,
        useFactory: createTranslateLoader,
        deps: [HttpClient],
      },
    }),
    DataTablesModule,
    FormsModule,
    ReactiveFormsModule,
    BrowserAnimationsModule,
    AppRoutingModule,
    AppAsideModule,
    AppBreadcrumbModule.forRoot(),//Add forRoot() if service is singleton.
    AppFooterModule,
    AppHeaderModule,
    AppSidebarModule,
    PerfectScrollbarModule,
    BsDropdownModule.forRoot(),//ngx-bootsrap
    TabsModule.forRoot(),      //ngx-bootsrap
    TooltipModule.forRoot(),   //ngx-bootsrap
    ModalModule,               //ngx-bootsrap
    ChartsModule,
    NgImageSliderModule,
    PaginationModule.forRoot(), //分頁用
    NgSelectModule, //selection
    BsDatepickerModule.forRoot(), //datepicker
  ],
  declarations: [
    AppComponent,
    APP_CONTAINERS,
    P404Component,
    P500Component,
    PictureComponent,
    F205TransComponent,
    F340Component,
    F340PpdComponent,
    F428Component,
    F428EditComponent,
    DtrFgtResultComponentComponent,
    DtrFgtResultReportComponent,
    DtrVStandardComponent,
    DtrVStandardListComponent,
    DictionaryComponent,
    HomePageComponent,
    CovidComponent,
  ],
  providers: [
    AuthService,
    ErrorInterceptorProvider,
    AlertifyService,
    AuthGuard,
    AuthGuardRole,
    DatePipe,
    {
      provide: LocationStrategy,
      useClass: HashLocationStrategy,
    },
  ],
  bootstrap: [AppComponent],
})
export class AppModule {}
