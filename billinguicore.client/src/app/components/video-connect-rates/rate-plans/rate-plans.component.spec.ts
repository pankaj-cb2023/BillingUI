import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RatePlansComponent } from './rate-plans.component';

describe('RatePlansComponent', () => {
  let component: RatePlansComponent;
  let fixture: ComponentFixture<RatePlansComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [RatePlansComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(RatePlansComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
