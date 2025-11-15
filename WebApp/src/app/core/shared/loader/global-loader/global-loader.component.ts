import { Component, inject, OnDestroy, OnInit } from '@angular/core';
import { GlobalLoaderService } from './global-loader.service';
import { Subscription } from 'rxjs';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-global-loader',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './global-loader.component.html',
  styleUrl: './global-loader.component.css',
})
export class GlobalLoaderComponent implements OnInit, OnDestroy {
  loaderService = inject(GlobalLoaderService);
  
  isActive = false;
  message = 'Đang tải...';
  subMessage = 'Vui lòng đợi trong giây lát';

  private subscription?: Subscription;

  ngOnInit() {
    console.log('GlobalLoader initialized');
    this.subscription = this.loaderService.loaderState.subscribe(state => {
      this.isActive = state.show;
      if (state.message) {
        this.message = state.message;
      }
      if (state.subMessage) {
        this.subMessage = state.subMessage;
      }
    });
  }
   ngOnDestroy() {
    this.subscription?.unsubscribe();
  }
}
