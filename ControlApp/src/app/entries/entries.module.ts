import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';

import { EntriesListComponent } from './entries-list/entries-list.component';
import { EntryComponent } from './entry/entry.component';
import { HttpEntriesService } from './http-entries.service';

@NgModule({
  imports: [
    CommonModule,
    HttpClientModule
  ],
  declarations: [
    EntriesListComponent,
    EntryComponent],
  exports: [
    EntriesListComponent
  ],
  providers: [
    HttpEntriesService
  ]
})
export class EntriesModule { }
