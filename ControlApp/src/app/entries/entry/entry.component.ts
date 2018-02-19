import { Component, OnInit, Input, ChangeDetectionStrategy, HostBinding, HostListener } from '@angular/core';
import { Entry } from '../entry';
import { HttpEntriesService } from '../http-entries.service';

@Component({
  selector: 'app-entry',
  templateUrl: './entry.component.html',
  styleUrls: ['./entry.component.scss']
})
export class EntryComponent implements OnInit {
  private _entry: Entry;
  private _current: Entry;

  get entry(): Entry {
    return this._entry;
  }

  @Input()
  set entry(value: Entry) {
    this._entry = value;
    this.updateCurrent();
  }

  @HostBinding('class.entry__current')
  isCurrent: boolean;

  constructor(
    private entriesService: HttpEntriesService
  ) { }

  ngOnInit() {
    this.entriesService.current().subscribe((e: Entry) => {
      this._current = e;
      this.updateCurrent();
    });
  }

  @HostListener('click', ['$event'])
  public onClick(event) {
    this.entriesService.setCurrentEntry(this.entry);
  }

  private updateCurrent() {
    this.isCurrent = this._entry && this._current && this._entry.name === this._current.name;
  }
}
