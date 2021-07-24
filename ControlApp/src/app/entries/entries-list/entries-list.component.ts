import { Component, OnInit } from '@angular/core';
import { HttpEntriesService } from '../http-entries.service';
import { Entry } from '../entry';

@Component({
  selector: 'app-entries-list',
  templateUrl: './entries-list.component.html',
  styleUrls: ['./entries-list.component.scss']
})
export class EntriesListComponent implements OnInit {
  public entries: Entry[];

  constructor(
    private entriesService: HttpEntriesService
  ) {
    this.entriesService.entries().subscribe((entries: Entry[]) => {
      this.entries = entries;
    });
  }

  ngOnInit() {
    this.entries = [];
  }
}
