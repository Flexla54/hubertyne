import { Component, OnInit } from '@angular/core';
import { OAuthService } from 'angular-oauth2-oidc';
import jwt_decode from 'jwt-decode';
import { Router } from '@angular/router';
import { ApiService } from '../../service/api.service';
import { Plug } from 'src/app/models/plug';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss'],
})
export class DashboardComponent implements OnInit {
  plugdata: any;
  plugs: Plug[] | null = null;
  chartOptions = {
    animation: false,
    elements: {
      point: {
        pointHitRadius: 25,
      },
    },
    scales: {
      y: {
        stacked: true,
        grid: {
          color: 'rgba(127,127,127,0.4)',
        },
      },
      x: {
        grid: {
          color: 'rgba(127,127,127,0.1)',
        },
      },
    },
    plugins: {
      filler: {
        propagate: true,
      },
    },
  };

  daterange: Date[] = [];
  dateprecision: 'minutes' | 'hours' | 'days' | 'weeks' | 'years' | null = null;
  dateprecisionoptions = ['minutes', 'hours', 'days', 'weeks', 'years'];

  constructor() {}

  ngOnInit() {
    let week = ['Mon', 'Tue', 'Wed', 'Thu', 'Fri', 'Sat', 'Sun'];
    for (let i = 0; i < new Date().getDay() + 6; i++) {
      week.push(week.shift()!);
    }
    this.daterange = [
      new Date(Date.now() - 1000 * 60 * 60 * 24 * 7),
      new Date(),
    ];

    //wait for 1 second to simulate loading
    new Promise((f) => setTimeout(f, 1000)).then(() => {
      this.plugs = [
        {
          id: '1',
          name: 'living room #1',
          addedDate: new Date('2023-03-22T07:56:38+0000'),
          isConnected: false,
          isTurnedOn: false,
          userId: 'admin',
          statistics: {
            labels: Array.from({ length: 10 }, (_, i) => i + 1),
            datasets: [
              {
                label: 'Plug Consumption last week',
                data: Array.from({ length: 10 }, (_) => Math.random() * 20),
                fill: false,
                borderColor: 'rgba(255,0,0,0.3)',
                tension: 1,
              },
            ],
          },
        },
        {
          id: '2',
          name: 'kitchen',
          addedDate: new Date('2023-04-22T07:56:38+0000'),
          isConnected: true,
          isTurnedOn: true,
          userId: 'admin',
          statistics: {
            labels: Array.from({ length: 10 }, (_, i) => i + 1),
            datasets: [
              {
                label: 'Plug Consumption last week',
                data: Array.from({ length: 10 }, (_) => Math.random() * 20),
                fill: false,
                borderColor: 'rgba(0,255,0,0.3)',
                tension: 0,
              },
            ],
          },
        },
        {
          id: '3',
          name: 'living room #2',
          addedDate: new Date('2023-04-22T07:56:38+0000'),
          isConnected: true,
          isTurnedOn: false,
          userId: 'admin',
          statistics: {
            labels: Array.from({ length: 10 }, (_, i) => i + 1),
            datasets: [
              {
                label: 'Plug Consumption last week',
                data: Array.from({ length: 10 }, (_) => Math.random() * 20),
                fill: false,
                borderColor: 'rgba(0,0,255,0.3)',
                tension: 0,
              },
            ],
          },
        },
        {
          id: '4',
          name: 'washing machine',
          addedDate: new Date('2023-04-22T07:56:38+0000'),
          isConnected: true,
          isTurnedOn: false,
          userId: 'admin',
          statistics: {
            labels: Array.from({ length: 10 }, (_, i) => i + 1),
            datasets: [
              {
                label: 'Plug Consumption last week',
                data: Array.from({ length: 10 }, (_) => Math.random() * 20),
                fill: false,
                borderColor: 'rgba(0,0,255,0.3)',
                tension: 0,
              },
            ],
          },
        },
      ];

      let l = this.plugs?.length;
      this.plugdata = {
        labels: week,
        datasets: this.plugs?.map((plug, idx) => ({
          label: plug.name,
          data: plug.statistics.datasets[0].data,
          fill: {
            target: idx === 0 ? 'origin' : '-1',
            above: `rgba(0,255,255,${(l - idx) / l})`,
          },
          borderColor: `rgba(0,255,255,${(l - idx) / l})`,
          borderJoinStyle: 'round',
          tension: 0,
        })),
      };
    });
  }
}
