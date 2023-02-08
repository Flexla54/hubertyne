import { Component } from "@angular/core";
import { ApiService } from "../shared/services/api.service";
import { DelegationService } from "../shared/services/delegation.service";

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

  constructor(
    private readonly api: ApiService,
    private readonly delegation: DelegationService
  ) {}

  public onSubmit() {
    if (this.pw1 === this.pw2 && this.username == "" && this.email == "") {
      this.api
        .signup(this.username, this.email, this.pw1)
        .catch((e) => {
          // error handling code
        })
        .then(() => {
          this.delegation.redirect();
        });
    }
  }
}
