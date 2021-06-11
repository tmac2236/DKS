/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { EngF340Component } from './eng-F340.component';

describe('EngF340Component', () => {
  let component: EngF340Component;
  let fixture: ComponentFixture<EngF340Component>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EngF340Component ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EngF340Component);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
