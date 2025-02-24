import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RatePlansDetailsComponent } from './rate-plans-details.component';

describe('RatePlansDetailsComponent', () => {
  let component: RatePlansDetailsComponent;
  let fixture: ComponentFixture<RatePlansDetailsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [RatePlansDetailsComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(RatePlansDetailsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

});
