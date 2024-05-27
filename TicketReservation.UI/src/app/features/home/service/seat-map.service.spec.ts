import { TestBed } from '@angular/core/testing';

import { SeatMapService } from './seat-map.service';

describe('SeatMapService', () => {
  let service: SeatMapService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(SeatMapService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
