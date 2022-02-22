/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { DtrFgtShoesComponent } from './dtr-fgt-shoes.component';

describe('DtrFgtShoesComponent', () => {
  let component: DtrFgtShoesComponent;
  let fixture: ComponentFixture<DtrFgtShoesComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DtrFgtShoesComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DtrFgtShoesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
