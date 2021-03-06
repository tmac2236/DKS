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
import { F420Component } from "./views/dks/F420/F420.component";
import { F340Component } from "./views/dks/F340/F340.component";

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
import { F428Component } from "./views/dks/F428/F428.component";
import { F428EditComponent } from "./views/dks/F428/F428-edit/F428-edit.component";


//載入 "/assets/i18n/[lang].json" 語系檔
export function createTranslateLoader(http: HttpClient) {
  return new TranslateHttpLoader(http, "./assets/i18n/", ".json");
}
@NgModule({
  imports: [
    NgxSpinnerModule,
    BrowserModule,
    HttpClientModule,
    TranslateModule.forRoot({
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
    AppBreadcrumbModule.forRoot(),
    AppFooterModule,
    AppHeaderModule,
    AppSidebarModule,
    PerfectScrollbarModule,
    BsDropdownModule.forRoot(),//ngx-bootsrap
    TabsModule.forRoot(),      //ngx-bootsrap
    TooltipModule.forRoot(),   //ngx-bootsrap
    ChartsModule,
    NgImageSliderModule,
    TooltipModule.forRoot(),    //table tr td 用
    PaginationModule.forRoot(), //分頁用
  ],
  declarations: [
    AppComponent,
    APP_CONTAINERS,
    P404Component,
    P500Component,
    PictureComponent,
    F340Component,
    F420Component,
    F428Component,
    F428EditComponent,
    DictionaryComponent,
    HomePageComponent,
  ],
  providers: [
    AuthService,
    ErrorInterceptorProvider,
    AlertifyService,
    AuthGuard,
    DatePipe,
    {
      provide: LocationStrategy,
      useClass: HashLocationStrategy,
    },
  ],
  bootstrap: [AppComponent],
})
export class AppModule {}
