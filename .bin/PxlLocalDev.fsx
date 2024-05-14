#I ".bin"
#r "Pxl.Core.dll"
#r "Pxl.Core.UI.dll"
#r "Pxl.RemoteCanvas.dll"
#r "SkiaSharp.dll"
#r "FsHttp.dll"

FsHttp.Fsi.disableDebugLogs()

open Pxl

[<RequireQualifiedAccess>] type Target = Local | RPi
[<RequireQualifiedAccess>] type Connection = Http | SignalR

let createTestCanvas w h target connection =
    let url =
        match target, connection with
        | Target.Local, _ -> "http://localhost:3000"
        | Target.RPi, _ -> "http://zero2w:3000"
    let create =
        match connection with
        | Connection.Http -> RemoteCanvas.createForHttp
        | Connection.SignalR -> RemoteCanvas.createForSignalR
    create w h url
