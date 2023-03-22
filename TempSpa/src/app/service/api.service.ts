import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ActivatedRoute, Route, Router } from '@angular/router';
import { firstValueFrom } from 'rxjs';
import { Provision } from '../models/provision';

@Injectable({
  providedIn: 'root',
})
export class ApiService {
  constructor(private readonly http: HttpClient) {}

  getProvisionId(description: string) {
    return firstValueFrom(
      this.http.post<Provision>('api/provisions', { description })
    );
  }

  getProvisionStatus(id: string) {
    return firstValueFrom(this.http.get<Provision>(`api/provisions/${id}`));
  }
}
