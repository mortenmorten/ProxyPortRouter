import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { HttpEntriesService } from '../http-entries.service';
import { EntriesListComponent } from './entries-list.component';

describe('EntriesListComponent', () => {
  let component: EntriesListComponent;
  let fixture: ComponentFixture<EntriesListComponent>;

  beforeEach(waitForAsync(() => {
    TestBed.configureTestingModule({
      declarations: [ EntriesListComponent ],
      imports: [HttpClientTestingModule],
      providers: [HttpEntriesService]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EntriesListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
