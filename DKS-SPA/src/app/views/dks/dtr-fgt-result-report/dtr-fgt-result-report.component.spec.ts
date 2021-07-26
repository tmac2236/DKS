/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { DtrFgtResultReportComponent } from './dtr-fgt-result-report.component';

describe('DtrFgtResultReportComponent', () => {
  let component: DtrFgtResultReportComponent;
  let fixture: ComponentFixture<DtrFgtResultReportComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DtrFgtResultReportComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DtrFgtResultReportComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
