import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Plug } from '../../models/plug';
import { PlugsDataPoints } from 'src/app/models/statistics';

@Component({
  selector: 'app-plugdetail',
  templateUrl: './plugdetail.component.html',
  styleUrls: ['./plugdetail.component.scss'],
})
export class PlugdetailComponent implements OnInit {
  pluginformation: Plug | null = null;
  statistic: any = null;
  loaded: boolean = false;

  constructor(private route: ActivatedRoute, private http: HttpClient) {}

  async ngOnInit(): Promise<void> {
    new Promise((f) => setTimeout(f, 1000)).then(() => {
      this.pluginformation = {
        id: '1',
        name: 'living room #1',
        addedDate: new Date('2023-03-22T07:56:38+0000'),
        isConnected: false,
        isTurnedOn: false,
        userId: 'admin',
        statistics: Array.from({ length: 10 }, (_) => Math.random() * 20),
      };
      this.statistic = {
        labels: Array.from({ length: 10 }, (_, i) => i + 1),
        datasets: [
          {
            label: 'Plug Consumption last week',
            data: this.pluginformation?.statistics,
            fill: false,
            borderColor: '#42A5F5',
            tension: 0,
          },
        ],
      };
      this.loaded = true;
    });
  }

  togglePlugActive() {
    if (this.pluginformation == null) return;

    this.pluginformation.isTurnedOn = !this.pluginformation.isTurnedOn;

    console.log('toggle plug active');
  }

  removePlug() {
    if (this.pluginformation == null) return;

    if (confirm('Are you sure you want to remove this plug?')) {
      console.log('remove plug');
    }
  }
}
