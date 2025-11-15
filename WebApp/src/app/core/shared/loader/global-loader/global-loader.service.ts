import { Injectable } from '@angular/core';
import { Subject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class GlobalLoaderService {
  private loaderSubject = new Subject<{show: boolean, message?: string, subMessage?: string}>();
  loaderState = this.loaderSubject.asObservable();

  show(message = 'Đang tải...', subMessage = 'Vui lòng đợi trong giây lát') {
    this.loaderSubject.next({show: true, message, subMessage});
  }

  hide() {
    this.loaderSubject.next({show: false});
  }
}