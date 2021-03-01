/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { F428Component } from './F428.component';

describe('F428Component', () => {
  let component: F428Component;
  let fixture: ComponentFixture<F428Component>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ F428Component ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(F428Component);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
