import { Component } from "@angular/core";
import { OAuthService } from "angular-oauth2-oidc";
import { authCodeFlowConfig } from "./config/auth.config";
import jwt_decode from "jwt-decode";

@Component({
  selector: "app-root",
  templateUrl: "./app.component.html",
  styleUrls: ["./app.component.scss"],
})
export class AppComponent {
  title = "TempSpa";
  token_payload: object = {};
  display_navigation: boolean = false;

  constructor(private oauthService: OAuthService) {
    this.oauthService.configure(authCodeFlowConfig);
    this.oauthService.loadDiscoveryDocumentAndLogin().then((hasToken) => {
      if (hasToken) {
        console.log(this.oauthService.getIdToken()); //TODO: remove debug output
      }
    });
  }

  showUser() {
    if (this.oauthService.hasValidIdToken()) {
      let payload: { sub: string } = jwt_decode(this.oauthService.getIdToken());
      alert(`username: ${payload.sub}`);
    }
  }
}
