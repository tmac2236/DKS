/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { EngComponent } from './eng.component';

describe('EngComponent', () => {
  let component: EngComponent;
  let fixture: ComponentFixture<EngComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EngComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EngComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
