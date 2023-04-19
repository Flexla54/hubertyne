import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { DelegationService } from '../shared/services/delegation.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {
  loginForm!: FormGroup;
  loginFailed: boolean = false;
  signUpPath: string = '';

  constructor(
    private formBuilder: FormBuilder,
    private readonly delegation: DelegationService
  ) {}

  ngOnInit() {
    this.loginForm = this.formBuilder.group({
        username: ['', Validators.required],
        password: ['', Validators.required]
    });

    this.signUpPath = `/signup${document.location.search}`;
  }

  get f() { return this.loginForm.controls; }

  onSubmit() {
    let uname = this.f['username'].value;
    let pw = this.f['password'].value

    this.delegation.tryLogin(uname, pw).catch(() => {
      this.loginFailed = true;

      this.f["username"].setValue("");
      this.f["password"].setValue("");
    });
  }
}
