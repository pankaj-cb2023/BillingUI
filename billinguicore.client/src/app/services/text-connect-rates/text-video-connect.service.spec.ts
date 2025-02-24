import { TestBed } from '@angular/core/testing';

import { TextVideoConnectService } from './text-video-connect.service';

describe('TextVideoConnectService', () => {
  let service: TextVideoConnectService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(TextVideoConnectService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
