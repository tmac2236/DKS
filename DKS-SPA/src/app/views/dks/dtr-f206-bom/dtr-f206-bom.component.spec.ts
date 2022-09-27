/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { DtrF206BomComponent } from './dtr-f206-bom.component';

describe('DtrF206BomComponent', () => {
  let component: DtrF206BomComponent;
  let fixture: ComponentFixture<DtrF206BomComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DtrF206BomComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DtrF206BomComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
