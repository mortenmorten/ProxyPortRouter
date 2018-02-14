import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppComponent } from './app.component';
import { EntriesModule } from './entries/entries.module';

@NgModule({
  declarations: [
    AppComponent
  ],
  imports: [
    BrowserModule,
    EntriesModule
  ],
  providers: [
    {provide: Window, useValue: window}
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
