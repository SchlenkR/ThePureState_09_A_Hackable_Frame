import { ElectronAPI } from '@electron-toolkit/preload';

declare global {
  interface Window {
    electron: ElectronAPI;
    api: {
      onFrameReceived: (callback: (data: string[]) => void) => void;
      offFrameReceived: () => void;
    };
  }
}
