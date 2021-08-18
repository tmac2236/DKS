/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { DtrVStandardListComponent } from './dtr-v-standard-list.component';

describe('DtrVStandardListComponent', () => {
  let component: DtrVStandardListComponent;
  let fixture: ComponentFixture<DtrVStandardListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DtrVStandardListComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DtrVStandardListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
