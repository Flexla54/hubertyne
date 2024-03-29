import { NgModule } from "@angular/core";
import { BrowserModule } from "@angular/platform-browser";
import { BrowserAnimationsModule } from "@angular/platform-browser/animations";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { HttpClientModule } from "@angular/common/http";

import { AppRoutingModule } from "./app-routing.module";
import { AppComponent } from "./app.component";
import { LoginComponent } from "./login/login.component";
import { SignupComponent } from "./signup/signup.component";

import { ButtonModule } from "primeng/button";
import { CardModule } from "primeng/card";
import { PasswordModule } from "primeng/password";
import { InputTextModule } from "primeng/inputtext";
import { DividerModule } from "primeng/divider";
import { InputNumberModule } from "primeng/inputnumber";

import { ApiService } from "./shared/services/api.service";

@NgModule({
  declarations: [AppComponent, LoginComponent, SignupComponent],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    AppRoutingModule,
    ButtonModule,
    CardModule,
    FormsModule,
    DividerModule,
    InputNumberModule,
    InputTextModule,
    PasswordModule,
    ReactiveFormsModule,
    HttpClientModule,
  ],
  providers: [ApiService],
  bootstrap: [AppComponent],
})
export class AppModule {}
