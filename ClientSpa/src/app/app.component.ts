import { Component } from '@angular/core';
import { OAuthService } from 'angular-oauth2-oidc';
import { authCodeFlowConfig } from './config/auth.config';
import jwt_decode from 'jwt-decode';
import { ApiService } from './service/api.service';
import { Router } from '@angular/router';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss'],
})
export class AppComponent {
  title = 'ClientSpa';
  token_payload: object = {};
  display_navigation: boolean = false;
  addPlugWindow: Window | null = null;
  newDeviceId: string = ''; //TODO: Maybe handle with ngrx

  constructor(
    private oauthService: OAuthService,
    private readonly api: ApiService,
    private readonly router: Router
  ) {
    this.oauthService.configure(authCodeFlowConfig);
    this.oauthService.loadDiscoveryDocumentAndLogin();
  }

  async addDevice() {
    // Request ProvisionId
    this.api
      .getProvisionId('')
      .then((provision) => {
        if (typeof provision == null) {
          console.log('The provision handed back was null.');
          return;
        }
        this.newDeviceId = provision!.id;
      })
      .catch((e) => {
        // Error handling
        console.log('The Request getProvision failed!');
      });

    this.addPlugWindow = window.open(
      `http://${environment.domain}/ConnectPlug?id=` + this.newDeviceId
    );

    // TODO: Exchange with some code that fires when getting the approval that the device connected
    let connected: boolean = false;
    for (let i = 0; i < 10; i++) {
      await new Promise((resolve) => setTimeout(resolve, 2000));

      this.api
        .getProvisionStatus(this.newDeviceId)
        .then((prov) => (connected = prov.hasConnected));

      if (connected) {
        await this.completeDeviceRegistration();
        return;
      }
    }

    // reset newDeviceId
    if (!connected) this.newDeviceId = '';
  }

  async completeDeviceRegistration() {
    this.addPlugWindow?.close;
    this.router.navigate([`/devices/${this.newDeviceId}`]);
  }

  showUser() {
    if (this.oauthService.hasValidIdToken()) {
      let payload: { sub: string } = jwt_decode(this.oauthService.getIdToken());
      alert(`username: ${payload.sub}`);
    }
  }
}
