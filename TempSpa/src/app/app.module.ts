import { NgModule } from "@angular/core";
import { HttpClientModule } from "@angular/common/http";
import { BrowserAnimationsModule } from "@angular/platform-browser/animations";
import { BrowserModule } from "@angular/platform-browser";

import { OAuthModule } from "angular-oauth2-oidc";

import { AppRoutingModule } from "./app-routing.module";
import { AppComponent } from "./app.component";
import { DashboardComponent } from "./dashboard/dashboard.component";

import { AvatarModule } from "primeng/avatar";
import { ButtonModule } from "primeng/button";
import { ChartModule } from "primeng/chart";
import { ProgressSpinnerModule } from "primeng/progressspinner";
import { SidebarModule } from "primeng/sidebar";
import { SkeletonModule } from "primeng/skeleton";
import { ToggleButtonModule } from "primeng/togglebutton";
import { ToolbarModule } from "primeng/toolbar";
import { PlugdetailComponent } from "./plugdetail/plugdetail.component";

@NgModule({
  declarations: [AppComponent, DashboardComponent, PlugdetailComponent],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    AppRoutingModule,
    OAuthModule.forRoot(),
    AvatarModule,
    ButtonModule,
    ChartModule,
    HttpClientModule,
    ProgressSpinnerModule,
    SidebarModule,
    SkeletonModule,
    ToggleButtonModule,
    ToolbarModule,
  ],
  providers: [],
  bootstrap: [AppComponent],
})
export class AppModule {}
