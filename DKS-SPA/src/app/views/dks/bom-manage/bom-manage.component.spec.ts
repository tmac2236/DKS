/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { BomManageComponent } from './bom-manage.component';

describe('BomManageComponent', () => {
  let component: BomManageComponent;
  let fixture: ComponentFixture<BomManageComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ BomManageComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(BomManageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
