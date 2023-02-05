import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { firstValueFrom } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ApiService {

  constructor(private readonly http: HttpClient) { }

  login(username: string, password: string) {
    return firstValueFrom(this.http.post("/api/account/session", {
      username,
      password
    }));
  }

  signup(
    username: string,
    email: string | null | undefined,
    password: string
  ) {
    return firstValueFrom(this.http.post("/api/account", {
      username,
      email,
      password
    }));
  }
}
