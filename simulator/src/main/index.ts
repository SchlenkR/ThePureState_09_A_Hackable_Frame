import { app, shell, BrowserWindow } from 'electron';
import { join } from 'path';
import { electronApp, optimizer, is } from '@electron-toolkit/utils';
import icon from '../../resources/icon.png?asset';
import { createServer } from 'http';
import express from 'express';

/*
 * The API of this method has to be identical to
 * the one exposed in Pxl.RemoteDisplay.
 */
function createPxlServer(mainWindow: BrowserWindow) {
  const app = express();
  const server = createServer(app);

  app.use(express.json());

  app.post('/pushFrame', (req, res) => {
    mainWindow.webContents.send('frameReceived', req.body);

    // TODO: error handling
    // ipcMain.on('frameReceivedResponse', (event, arg) => {
    //   if (arg.error) {
    //     console.error('Error in renderer process:', arg.error);
    //   } else {
    //     console.log('Message received by renderer process');
    //   }
    // });
    res.sendStatus(204);
  });

  server.listen(3000, () => {
    console.log('listening on *:3000');
  });
}

function createWindow() {
  const mainWindow = new BrowserWindow({
    width: 750,
    height: 1100,
    show: true,
    autoHideMenuBar: true,
    ...(process.platform === 'linux' ? { icon } : {}),
    webPreferences: {
      preload: join(__dirname, '../preload/index.js'),
      sandbox: false,
      contextIsolation: true,
    },
  });
  mainWindow.setAlwaysOnTop(true, 'floating');

  mainWindow.on('ready-to-show', () => {
    mainWindow.show();
    mainWindow.webContents.setZoomFactor(0.9);
  });
  mainWindow.webContents.setWindowOpenHandler((details) => {
    shell.openExternal(details.url);
    return { action: 'deny' };
  });

  // HMR for renderer base on electron-vite cli.
  // Load the remote URL for development or the local html file for production.
  if (is.dev && process.env['ELECTRON_RENDERER_URL']) {
    mainWindow.loadURL(process.env['ELECTRON_RENDERER_URL']);
  } else {
    mainWindow.loadFile(join(__dirname, '../renderer/index.html'));
  }

  return mainWindow;
}

// This method will be called when Electron has finished
// initialization and is ready to create browser windows.
// Some APIs can only be used after this event occurs.
app.whenReady().then(() => {
  // Set app user model id for windows
  electronApp.setAppUserModelId('com.electron');

  // Default open or close DevTools by F12 in development
  // and ignore CommandOrControl + R in production.
  // see https://github.com/alex8088/electron-toolkit/tree/master/packages/utils
  app.on('browser-window-created', (_, window) => {
    optimizer.watchWindowShortcuts(window);
  });

  const mainWindow = createWindow();
  createPxlServer(mainWindow);

  app.on('activate', function () {
    // On macOS it's common to re-create a window in the app when the
    // dock icon is clicked and there are no other windows open.
    if (BrowserWindow.getAllWindows().length === 0) createWindow();
  });
});

// Quit when all windows are closed, except on macOS. There, it's common
// for applications and their menu bar to stay active until the user quits
// explicitly with Cmd + Q.
app.on('window-all-closed', () => {
  if (process.platform !== 'darwin') {
    app.quit();
  }
});

// In this file you can include the rest of your app"s specific main process
// code. You can also put them in separate files and require them here.
