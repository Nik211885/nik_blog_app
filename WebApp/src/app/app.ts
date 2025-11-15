import { Component, inject, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { GlobalLoaderComponent } from "./core/shared/loader/global-loader/global-loader.component";
import { GlobalLoaderService } from './core/shared/loader/global-loader/global-loader.service';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, GlobalLoaderComponent],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App {
  globalLoadingService = inject(GlobalLoaderService);

  globalLoading(){
    console.log("AAA");
    this.globalLoadingService.show()

    setTimeout(()=>{
      this.globalLoadingService.hide();
    }, 3000)

  }
}
