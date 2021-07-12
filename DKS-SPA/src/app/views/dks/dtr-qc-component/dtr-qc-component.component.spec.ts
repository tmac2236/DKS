import { async, ComponentFixture, TestBed } from '@angular/core/testing';
/* tslint:disable:no-unused-variable */
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { DtrQcComponentComponent } from './dtr-qc-component.component';

describe('DtrQcComponentComponent', () => {
  let component: DtrQcComponentComponent;
  let fixture: ComponentFixture<DtrQcComponentComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DtrQcComponentComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DtrQcComponentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
