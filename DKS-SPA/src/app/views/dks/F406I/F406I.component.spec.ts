/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { F406IComponent } from './F406I.component';

describe('F406IComponent', () => {
  let component: F406IComponent;
  let fixture: ComponentFixture<F406IComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ F406IComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(F406IComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
