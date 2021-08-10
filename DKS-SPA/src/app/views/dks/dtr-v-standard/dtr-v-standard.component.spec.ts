/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { DtrVStandardComponent } from './dtr-v-standard.component';

describe('DtrVStandardComponent', () => {
  let component: DtrVStandardComponent;
  let fixture: ComponentFixture<DtrVStandardComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DtrVStandardComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DtrVStandardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
