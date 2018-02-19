import { Component, OnInit, Input, ChangeDetectionStrategy, HostBinding } from '@angular/core';
import { Entry } from '../entry';
import { HttpEntriesService } from '../http-entries.service';

@Component({
  selector: 'app-entry',
  templateUrl: './entry.component.html',
  styleUrls: ['./entry.component.scss']
})
export class EntryComponent implements OnInit {

  @Input()
  entry: Entry;

  @HostBinding('class.entry__current')
  isCurrent: boolean;

  constructor(
    private entriesService: HttpEntriesService
  ) { }

  ngOnInit() {
    this.entriesService.current().subscribe((e: Entry) => {
      this.isCurrent = e && this.entry.name === e.name;
    });
  }

  public onClick(event) {
    this.entriesService.setCurrentEntry(this.entry);
  }
}
