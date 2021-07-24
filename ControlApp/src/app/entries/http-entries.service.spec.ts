import { TestBed, inject } from '@angular/core/testing';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { HttpEntriesService } from './http-entries.service';

describe('HttpEntriesService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [HttpEntriesService]
    });
  });

  it('should be created', inject([HttpEntriesService], (service: HttpEntriesService) => {
    expect(service).toBeTruthy();
  }));
});
