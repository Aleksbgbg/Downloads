import { BrowserModule } from "@angular/platform-browser";
import { NgModule } from "@angular/core";

import { AppRoutingModule } from "./app-routing.module";
import { AppComponent } from "./app.component";
import { HomeComponent } from "./components/home/home.component";
import { AppDownloadComponent } from "./components/partials/app-download/app-download.component";
import { HttpClientModule } from "@angular/common/http";
import { MarkdownModule } from "ngx-markdown";
import { HeaderComponent } from "./components/common/header/header.component";
import { SpecificAppComponent } from "./components/specific-app/specific-app.component";

@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
    AppDownloadComponent,
    HeaderComponent,
    SpecificAppComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    MarkdownModule.forRoot()
  ],
  providers: [{
    provide: "protocol",
    useValue: window.location.protocol
  }, {
    provide: "host",
    useValue: window.location.host
  }],
  bootstrap: [AppComponent]
})
export class AppModule { }
