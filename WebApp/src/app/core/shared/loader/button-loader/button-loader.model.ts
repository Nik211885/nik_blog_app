export interface ButtonLoaderModel {
  bgColor: string;
  textColor: string;
  borderSolid: number;
  radius: number;
  hoverColor?: string;
  shadowColor?: string;
}

export enum ButtonPrimaryType {
  Primary,
  Success,
  Danger,
  Warning,
  Info,
  Dark,
  Light,
  Secondary
}

export enum ButtonSize {
  Small = 'small',
  Medium = 'medium',
  Large = 'large'
}

export function FactoryButtonByType(type: ButtonPrimaryType): ButtonLoaderModel {
  let bgColor: string = "";
  let hoverColor: string = "";
  let shadowColor: string = "";
  let borderRadius = 8;
  let borderSolid = 0;
  let textColor = "white";

  switch (type) {
    case ButtonPrimaryType.Primary:
      bgColor = "linear-gradient(135deg, #667eea 0%, #764ba2 100%)";
      hoverColor = "linear-gradient(135deg, #5568d3 0%, #653a8a 100%)";
      shadowColor = "rgba(102, 126, 234, 0.4)";
      break;
    case ButtonPrimaryType.Danger:
      bgColor = "linear-gradient(135deg, #ef4444 0%, #dc2626 100%)";
      hoverColor = "linear-gradient(135deg, #dc2626 0%, #b91c1c 100%)";
      shadowColor = "rgba(239, 68, 68, 0.4)";
      break;
    case ButtonPrimaryType.Dark:
      bgColor = "linear-gradient(135deg, #1e293b 0%, #0f172a 100%)";
      hoverColor = "linear-gradient(135deg, #0f172a 0%, #020617 100%)";
      shadowColor = "rgba(30, 41, 59, 0.4)";
      break;
    case ButtonPrimaryType.Info:
      bgColor = "linear-gradient(135deg, #06b6d4 0%, #0891b2 100%)";
      hoverColor = "linear-gradient(135deg, #0891b2 0%, #0e7490 100%)";
      shadowColor = "rgba(6, 182, 212, 0.4)";
      break;
    case ButtonPrimaryType.Success:
      bgColor = "linear-gradient(135deg, #10b981 0%, #059669 100%)";
      hoverColor = "linear-gradient(135deg, #059669 0%, #047857 100%)";
      shadowColor = "rgba(16, 185, 129, 0.4)";
      break;
    case ButtonPrimaryType.Warning:
      bgColor = "linear-gradient(135deg, #f59e0b 0%, #d97706 100%)";
      hoverColor = "linear-gradient(135deg, #d97706 0%, #b45309 100%)";
      shadowColor = "rgba(245, 158, 11, 0.4)";
      break;
    case ButtonPrimaryType.Light:
      bgColor = "linear-gradient(135deg, #f8fafc 0%, #e2e8f0 100%)";
      hoverColor = "linear-gradient(135deg, #e2e8f0 0%, #cbd5e1 100%)";
      shadowColor = "rgba(148, 163, 184, 0.3)";
      textColor = "#1e293b";
      break;
    case ButtonPrimaryType.Secondary:
      bgColor = "linear-gradient(135deg, #64748b 0%, #475569 100%)";
      hoverColor = "linear-gradient(135deg, #475569 0%, #334155 100%)";
      shadowColor = "rgba(100, 116, 139, 0.4)";
      break;
    default:
      throw new Error("Cannot support this button type");
  }

  const buttonLoaderModel: ButtonLoaderModel = {
    bgColor: bgColor,
    borderSolid: borderSolid,
    radius: borderRadius,
    textColor: textColor,
    hoverColor: hoverColor,
    shadowColor: shadowColor
  };
  
  return buttonLoaderModel;
}