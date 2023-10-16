/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { F203CopyComponent } from './F203-copy.component';

describe('F203CopyComponent', () => {
  let component: F203CopyComponent;
  let fixture: ComponentFixture<F203CopyComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ F203CopyComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(F203CopyComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
