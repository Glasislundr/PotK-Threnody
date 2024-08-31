SETLOCAL ENABLEDELAYEDEXPANSION
REM Create wav folder if not exist
cd extracted\StreamingAssets\android
if not exist "wav" mkdir "wav"

REM Rename files to allow for proper cue tracking
FOR %%a IN (*_awb.awb) DO (
SET a1=%%a
SET pth=!a1:~0,-8%!
ren "%%a" "!pth!.awb"
)
FOR %%a IN (*_acb.acb) DO (
SET a1=%%a
SET pth=!a1:~0,-8%!
ren "%%a" "!pth!.acb"
)

REM Loop though to unpack multiple files
FOR %%a IN (*.awb) DO (
SET a1=%%a
SET pth=!a1:~0,-4%!
if not exist "wav\!pth!" mkdir "wav\!pth!"
".\vgmstream-win64\vgmstream-cli" -S 0 -l 2 -f 10 -o "wav\!pth!\?04s_?n.wav" "%%a"
)