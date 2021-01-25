/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { F420Component } from './F420.component';

describe('F420Component', () => {
  let component: F420Component;
  let fixture: ComponentFixture<F420Component>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ F420Component ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(F420Component);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
