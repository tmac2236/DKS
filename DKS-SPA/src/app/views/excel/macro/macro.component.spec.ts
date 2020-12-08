/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { MacroComponent } from './macro.component';

describe('MacroComponent', () => {
  let component: MacroComponent;
  let fixture: ComponentFixture<MacroComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ MacroComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(MacroComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
