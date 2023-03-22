import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ApiService } from '../../service/api.service';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss'],
})
export class DashboardComponent implements OnInit {
  display: boolean = false;
  plugdata: any;
  addPlugWindow: Window | null = null;
  newDeviceId: string = ''; //TODO: Maybe handle with ngrx

  constructor(
    private readonly api: ApiService,
    private readonly router: Router
  ) {}

  ngOnInit() {
    let week = ['Mon', 'Tue', 'Wed', 'Thu', 'Fri', 'Sat', 'Sun'];
    for (let i = 0; i < new Date().getDay() + 6; i++) {
      week.push(week.shift()!);
    }

    let data = [];
    let avg_data = [];
    for (let i = 0; i < 7; i++) {
      data.push(Math.floor(Math.random() * 500));
      avg_data.push(
        Math.floor(Math.random() * 125) +
          Math.floor(Math.random() * 125) +
          Math.floor(Math.random() * 125) +
          Math.floor(Math.random() * 125)
      );
    }

    this.plugdata = {
      labels: week,
      datasets: [
        {
          label: 'Plug Consumption last week',
          data: data,
          fill: false,
          borderColor: '#42A5F5',
          tension: 0,
        },
        {
          label: 'avg. last 5 weeks',
          data: avg_data,
          fill: false,
          borderColor: '#A542F5',
          tension: 0.5,
        },
      ],
    };
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
      'localhost/ConnectPlug?id=' + this.newDeviceId
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
}
