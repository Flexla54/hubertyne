import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ActivatedRoute, Route, Router } from '@angular/router';
import { firstValueFrom } from 'rxjs';

export interface Provision {
  Id: string;
  UserId: string;
  HasConnected: boolean;
  Description: string | undefined;
}

@Injectable({
  providedIn: 'root',
})
export class ProvisionService {
  constructor(
    private readonly http: HttpClient,
    private readonly router: Router
  ) {}

  getProvisionId(description: string) {
    return firstValueFrom(
      this.http.post<Provision>('api/provisions', { description })
    );
  }

  getProvisionStatus(id: string) {
    return firstValueFrom(this.http.get<Provision>(`api/provisions/${id}`));
  }

  routeToDevicePage(id: string) {
    this.router.navigate([`/devices/${id}`]);
  }
}
