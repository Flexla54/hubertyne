import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PlugdetailComponent } from './plugdetail.component';

describe('PlugdetailComponent', () => {
  let component: PlugdetailComponent;
  let fixture: ComponentFixture<PlugdetailComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ PlugdetailComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(PlugdetailComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
