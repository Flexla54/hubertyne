import { Component, ElementRef, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ApiService } from '../shared/services/api.service';
import { DelegationService } from '../shared/services/delegation.service';
import { PasswordMatchValidator } from '../shared/password-match.directive';

@Component({
  selector: 'app-signup',
  templateUrl: './signup.component.html',
  styleUrls: ['./signup.component.scss'],
})
export class SignupComponent implements OnInit {
  signupForm!: FormGroup;
  signupStatus: {
    isEmpty: boolean;
    invEmail: boolean;
    difPw: boolean;
  } = {
    isEmpty: false,
    invEmail: false,
    difPw: false,
  };

  constructor(
    private readonly api: ApiService,
    private readonly delegation: DelegationService,
    private formBuilder: FormBuilder
  ) {}

  ngOnInit() {
    this.signupForm = this.formBuilder.group(
      {
        username: ['', Validators.required],
        email: ['', [Validators.required, Validators.email]],
        pw1: ['', Validators.required],
        pw2: ['', Validators.required],
      },
      { validators: PasswordMatchValidator }
    );

    this.signupForm.valueChanges.subscribe((value) => {
      this.signupStatus.isEmpty =
        (this.f['username'].errors?.['required'] ||
          this.f['email'].errors?.['required'] ||
          this.f['pw1'].errors?.['required'] ||
          this.f['pw2'].errors?.['required']) &&
        !this.f['username'].pristine &&
        !this.f['email'].pristine &&
        !this.f['pw1'].pristine &&
        !this.f['pw2'].pristine;

      this.signupStatus.invEmail = this.f['email'].errors?.['email'];
      this.signupStatus.difPw = this.signupForm.errors?.['passwordDiffers'];
    });
  }

  get f() {
    return this.signupForm.controls;
  }

  public onSubmit() {
    this.api
      .signup(
        this.f['username'].value,
        this.f['email'].value,
        this.f['pw1'].value
      )
      .catch((e) => {
        // error handling code

        this.f['pw1'].setValue('');
        this.f['pw2'].setValue('');
      })
      .then(() => {
        this.delegation.redirect();
      });
  }
}
