/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { P202Component } from './P202.component';

describe('P202Component', () => {
  let component: P202Component;
  let fixture: ComponentFixture<P202Component>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ P202Component ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(P202Component);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
