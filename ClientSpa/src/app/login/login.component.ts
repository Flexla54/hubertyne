import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {
  loginForm!: FormGroup;
  loginFailed: boolean = false;

  constructor(
    private formBuilder: FormBuilder,
    private router: Router,
  ) {}

  ngOnInit() {
    this.loginForm = this.formBuilder.group({
        username: ['', Validators.required],
        password: ['', Validators.required]
    });
  }

  get f() { return this.loginForm.controls; }

  onSubmit() {
    let uname = this.f['username'].value;
    let pw = this.f['password'].value
    console.log(uname);
    console.log(pw);

    if (uname === "test" && pw === "UwU") {
      //redirect to actual url and handle backend stuff once backend works
      window.location.href = "https://google.com"
      this.loginFailed = false;
      return;
    }

    this.loginFailed = true;
    this.f['username'].setValue("");
    this.f['password'].setValue("");
  }
}
