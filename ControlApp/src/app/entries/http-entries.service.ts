import { Injectable, OnInit } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';

import { Observable } from 'rxjs/Observable';
import { Subject } from 'rxjs/Subject';

import { Entry } from './entry';

@Injectable()
export class HttpEntriesService {
  private _currentEntry = new Subject<Entry>();
  private _entries = new Subject<Entry[]>();

  constructor(
    private http: HttpClient
  ) {
  }

  private configureOptions() {
    return {
      headers: new HttpHeaders()
        .set('Content-Type', 'application/json')
    };
  }

  public current(): Observable<Entry> {
    return this._currentEntry.asObservable();
  }

  public entries(): Observable<Entry[]> {
    return this._entries.asObservable();
  }

  public getCurrentEntry(): void {
    console.log(`GET: /api/entry`);
    this.http.get<Entry>('/api/entry').subscribe((e: Entry) => {
      this._currentEntry.next(e);
    });
  }

  public getEntries(): void {
    console.log(`GET: /api/entry/list`);
    this.http.get<Entry[]>('/api/entry/list').subscribe((e: Entry[]) => {
      this._entries.next(e);
    });
  }

  public setCurrentEntry(entry: Entry): void {
    console.log(`PUT: /api/entry`);
    const body = JSON.stringify(entry);
    this.http.put<Entry>('/api/entry', body, this.configureOptions())
      .subscribe((e: Entry) => this._currentEntry.next(e));
  }
}
