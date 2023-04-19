import { Injectable } from "@angular/core";
import { ActivatedRoute, Router } from "@angular/router";
import { ApiService } from "./api.service";

@Injectable({
  providedIn: "root",
})
export class DelegationService {
  constructor(
    private readonly api: ApiService,
    private readonly route: ActivatedRoute,
    private readonly router: Router
  ) {}

  /**
   * Try to login and redirect if the attempt was successful 
   * @returns A http response where .then() is already handled
   */
  tryLogin(username: string, password: string) {
    return this.api.login(username, password).then(() => this.redirect())
  }

  redirect() {
    const returnUrl = this.route.snapshot.queryParamMap.get("ReturnUrl");

    if (returnUrl) {
      location.href = returnUrl;
    } else {
      this.router.navigate(["/"]);
    }
  }
}
