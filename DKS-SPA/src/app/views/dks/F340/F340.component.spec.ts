/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { F340Component } from './F340.component';

describe('F340Component', () => {
  let component: F340Component;
  let fixture: ComponentFixture<F340Component>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ F340Component ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(F340Component);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
