import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ButtonLoaderModel, ButtonPrimaryType, ButtonSize, FactoryButtonByType } from './button-loader.model';

@Component({
  selector: 'app-button-loader',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './button-loader.component.html',
  styleUrl: './button-loader.component.css',
})
export class ButtonLoaderComponent implements OnInit {
  @Input() buttonType: ButtonPrimaryType = ButtonPrimaryType.Info;
  @Input() buttonText: string = "Button";
  @Input() size: ButtonSize = ButtonSize.Medium;
  @Input() disabled: boolean = false;
  @Input() fullWidth: boolean = false;
  @Input() icon?: string; 
  @Input() iconPosition: 'left' | 'right' = 'left';
  @Input() loadingText: string = "Loading...";
  @Input() showLoadingText: boolean = true;
  @Input() rounded: boolean = false;
  @Input() outlined: boolean = false;
  
  @Output() buttonClickEmit = new EventEmitter<void>();
  
  buttonLoader!: ButtonLoaderModel;
  isLoading: boolean = false;

  ngOnInit(): void {
    this.buttonLoader = FactoryButtonByType(this.buttonType);
  }

  handleButtonClick() {
    if (this.disabled || this.isLoading) {
      return;
    }
    
    this.isLoading = true;
    this.buttonClickEmit.emit();
  }

  stopLoading(): void {
    this.isLoading = false;
  }

  startLoading(): void {
    this.isLoading = true;
  }

  getSizeClass(): string {
    switch (this.size) {
      case ButtonSize.Small:
        return 'btn-small';
      case ButtonSize.Large:
        return 'btn-large';
      default:
        return 'btn-medium';
    }
  }
}