import { HttpClient } from "@angular/common/http";
import { Component, OnInit } from "@angular/core";
import { ActivatedRoute } from "@angular/router";
import { Plug } from "../models/plug";

@Component({
  selector: "app-plugdetail",
  templateUrl: "./plugdetail.component.html",
  styleUrls: ["./plugdetail.component.scss"],
})
export class PlugdetailComponent implements OnInit {
  pluginformation: Plug | null = null;
  loaded: boolean = false;

  constructor(private route: ActivatedRoute, private http: HttpClient) {}

  async ngOnInit(): Promise<void> {
    new Promise((f) => setTimeout(f, 100)).then(() => {
      this.pluginformation = {
        id: "1",
        name: "living room #1",
        addedDate: new Date("2023-03-22T07:56:38+0000"),
        isConnected: false,
        isTurnedOn: false,
        userId: "admin",
        statistics: {
          labels: Array.from({ length: 10 }, (_, i) => i + 1),
          datasets: [
            {
              label: "Plug Consumption last week",
              data: Array.from({ length: 10 }, (_) => Math.random() * 20),
              fill: false,
              borderColor: "#42A5F5",
              tension: 0,
            },
          ],
        },
      };
      this.loaded = true;
    });
  }

  togglePlugActive() {
    if (this.pluginformation == null) return;
    this.pluginformation.isTurnedOn = !this.pluginformation.isTurnedOn;

    console.log("toggle plug active");
  }
}
