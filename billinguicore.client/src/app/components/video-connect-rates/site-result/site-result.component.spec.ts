import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SiteResultComponent } from './site-result.component';

describe('SiteResultComponent', () => {
  let component: SiteResultComponent;
  let fixture: ComponentFixture<SiteResultComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [SiteResultComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(SiteResultComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
