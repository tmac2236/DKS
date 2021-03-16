/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { F340PpdComponent } from './F340-ppd.component';

describe('F340PpdComponent', () => {
  let component: F340PpdComponent;
  let fixture: ComponentFixture<F340PpdComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ F340PpdComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(F340PpdComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
