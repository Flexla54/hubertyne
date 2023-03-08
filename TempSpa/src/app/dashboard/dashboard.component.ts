import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss'],
})
export class DashboardComponent implements OnInit {
  display: boolean = false;
  plugdata: any;

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
    let response = await fetch('https://localhost/api/provisions', {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
      },
    });

    let deviceId = response.json();

    window.open('localhost/ConnectPlug?id=' + deviceId);

    // TODO: Some code that fires when getting the approval that the device connected
  }
}