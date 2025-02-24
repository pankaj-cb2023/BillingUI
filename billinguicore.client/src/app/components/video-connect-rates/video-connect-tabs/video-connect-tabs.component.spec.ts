import { ComponentFixture, TestBed } from '@angular/core/testing';

import { VideoConnectTabsComponent } from './video-connect-tabs.component';

describe('VideoConnectTabsComponent', () => {
  let component: VideoConnectTabsComponent;
  let fixture: ComponentFixture<VideoConnectTabsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [VideoConnectTabsComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(VideoConnectTabsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
