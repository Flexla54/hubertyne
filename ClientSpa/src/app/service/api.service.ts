import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ActivatedRoute, Route, Router } from '@angular/router';
import { firstValueFrom } from 'rxjs';
import { Provision } from '../models/provision';
import {
  Consumption,
  ConsumptionQuery,
  Usage,
  UsageQuery,
} from '../models/statistics';

@Injectable({
  providedIn: 'root',
})
export class ApiService {
  constructor(private readonly http: HttpClient) {}

  // ############################################
  // Provision queries

  createProvisionId(description: string) {
    return firstValueFrom(
      this.http.post<Provision>('api/provisions', { description })
    );
  }

  getProvisionStatus(id: string) {
    return firstValueFrom(this.http.get<Provision>(`api/provisions/${id}`));
  }

  // ############################################
  // Statistics queries

  getPowerConsumption(query: ConsumptionQuery) {
    if (query.resourceId == null) {
      return firstValueFrom(
        this.http.get<Consumption>(
          `api/plugs/${query.resourceId}/consumption?from=${query.from}&to=${query.to}&tact=${query.tact}`
        )
      );
    } else {
      return firstValueFrom(
        this.http.get<Consumption[]>(
          `api/plugs/all/consumption?from=${query.from}&to=${query.to}&tact=${query.tact}`
        )
      );
    }
  }

  getPowerUsage(query: UsageQuery) {
    if (query.resourceId == null) {
      return firstValueFrom(
        this.http.get<Usage[]>(
          `api/plugs/all/usage?from=${query.from}&to=${query.to}`
        )
      );
    } else {
      return firstValueFrom(
        this.http.get<Usage>(
          `api/plugs/${query.resourceId}/usage?from=${query.from}&to=${query.to}`
        )
      );
    }
  }
}
