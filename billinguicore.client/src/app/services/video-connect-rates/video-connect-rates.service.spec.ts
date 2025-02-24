import { TestBed } from '@angular/core/testing';

import { VideoConnectRatesService } from './video-connect-rates.service';

describe('VideoConnectRatesService', () => {
  let service: VideoConnectRatesService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(VideoConnectRatesService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
