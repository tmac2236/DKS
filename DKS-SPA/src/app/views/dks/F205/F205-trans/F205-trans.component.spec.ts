/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { F205TransComponent } from './F205-trans.component';

describe('F205TransComponent', () => {
  let component: F205TransComponent;
  let fixture: ComponentFixture<F205TransComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ F205TransComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(F205TransComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
