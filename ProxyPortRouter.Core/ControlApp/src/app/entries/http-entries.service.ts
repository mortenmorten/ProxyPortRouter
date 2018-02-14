import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';

import { Observable } from 'rxjs/Observable';
import { Subject } from 'rxjs/Subject';
import 'rxjs/add/operator/finally';

import { Entry } from './entry';

@Injectable()
export class HttpEntriesService {
  private _currentEntry = new Subject<Entry>();
  private _entries = new Subject<Entry[]>();
  private _baseUrl;

  constructor(
    private window: Window,
    private http: HttpClient
  ) {
    this._baseUrl = `http://${this.window.location.hostname}:8080`;
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
    this.http.get<Entry>(`${this._baseUrl}/api/entry`).subscribe((e: Entry) => {
      this._currentEntry.next(e);
    });
  }

  public getEntries(): void {
    this.http.get<Entry[]>(`${this._baseUrl}/api/entry/list`).subscribe((e: Entry[]) => {
      this._entries.next(e);
    });
  }

  public setCurrentEntry(entry: Entry) {
    return this.http.put(`${this._baseUrl}/api/entry`, entry, this.configureOptions())
      .finally(() => this.getCurrentEntry());
  }
}
