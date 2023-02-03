import { Component } from "@angular/core";

@Component({
  selector: "app-signup",
  templateUrl: "./signup.component.html",
  styleUrls: ["./signup.component.scss"],
})
export class SignupComponent {
  username: string = "";
  email: string = "";
  pw1: string = "";
  pw2: string = "";
  maxconsumption: number = 0;
}
