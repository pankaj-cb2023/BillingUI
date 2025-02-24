import { ComponentFixture, TestBed } from '@angular/core/testing';
import { AgencyResultComponent } from './agency-result.component';

describe('AgencyresultComponent', () => {
  let component: AgencyResultComponent;
  let fixture: ComponentFixture<AgencyResultComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [AgencyResultComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AgencyResultComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
