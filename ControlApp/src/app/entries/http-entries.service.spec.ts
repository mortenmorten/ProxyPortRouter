import { TestBed, inject } from '@angular/core/testing';

import { HttpEntriesService } from './http-entries.service';

describe('HttpEntriesService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [HttpEntriesService]
    });
  });

  it('should be created', inject([HttpEntriesService], (service: HttpEntriesService) => {
    expect(service).toBeTruthy();
  }));
});
