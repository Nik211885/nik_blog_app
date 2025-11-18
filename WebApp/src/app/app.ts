import { Component} from '@angular/core';
import {ButtonLoaderComponent} from '../app/core/shared/loader/button-loader/button-loader.component'
import { ButtonPrimaryType } from './core/shared/loader/button-loader/button-loader.model';

@Component({
  selector: 'app-root',
  imports: [ButtonLoaderComponent],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App {
  ButtonPrimaryType = ButtonPrimaryType;
  
  handleClick(buttonComponent: ButtonLoaderComponent) {
    setInterval(()=>{
      buttonComponent.stopLoading()
    },3000)
  }
}
