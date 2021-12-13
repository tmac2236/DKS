/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { PlmPartComponent } from './plm-part.component';

describe('PlmPartComponent', () => {
  let component: PlmPartComponent;
  let fixture: ComponentFixture<PlmPartComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PlmPartComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PlmPartComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
