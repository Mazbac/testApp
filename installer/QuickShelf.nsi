Unicode true
!include "MUI2.nsh"

!ifndef VERSION
  !define VERSION "0.1.0"
!endif
!ifndef PUBLISH_DIR
  !error "PUBLISH_DIR must be supplied by the build script"
!endif
!ifndef OUTPUT_DIR
  !error "OUTPUT_DIR must be supplied by the build script"
!endif
!ifndef ICON_PATH
  !error "ICON_PATH must be supplied by the build script"
!endif

Name "QuickShelf"
OutFile "${OUTPUT_DIR}\QuickShelf-Setup-${VERSION}.exe"
InstallDir "$LOCALAPPDATA\Programs\QuickShelf"
InstallDirRegKey HKCU "Software\QuickShelf" "InstallLocation"
RequestExecutionLevel user
SetCompressor /SOLID lzma
BrandingText "QuickShelf"
ShowInstDetails nevershow
ShowUninstDetails nevershow
Icon "${ICON_PATH}"
UninstallIcon "${ICON_PATH}"
VIProductVersion "${VERSION}.0"
VIAddVersionKey /LANG=1033 "ProductName" "QuickShelf"
VIAddVersionKey /LANG=1033 "FileDescription" "QuickShelf installer"
VIAddVersionKey /LANG=1033 "CompanyName" "QuickShelf"
VIAddVersionKey /LANG=1033 "LegalCopyright" "Copyright 2026 QuickShelf contributors"
VIAddVersionKey /LANG=1033 "FileVersion" "${VERSION}"
VIAddVersionKey /LANG=1033 "ProductVersion" "${VERSION}"

!define MUI_ABORTWARNING
!define MUI_FINISHPAGE_RUN "$INSTDIR\QuickShelf.exe"
!define MUI_FINISHPAGE_RUN_TEXT "Open QuickShelf"
!insertmacro MUI_PAGE_WELCOME
!insertmacro MUI_PAGE_DIRECTORY
!insertmacro MUI_PAGE_INSTFILES
!insertmacro MUI_PAGE_FINISH
!insertmacro MUI_UNPAGE_CONFIRM
!insertmacro MUI_UNPAGE_INSTFILES
!insertmacro MUI_LANGUAGE "English"

Section "QuickShelf" SEC_MAIN
  SetShellVarContext current
  SetOutPath "$INSTDIR"
  File /r "${PUBLISH_DIR}\*.*"
  WriteUninstaller "$INSTDIR\Uninstall.exe"

  CreateDirectory "$SMPROGRAMS\QuickShelf"
  CreateShortcut "$SMPROGRAMS\QuickShelf\QuickShelf.lnk" "$INSTDIR\QuickShelf.exe"
  CreateShortcut "$SMPROGRAMS\QuickShelf\Uninstall QuickShelf.lnk" "$INSTDIR\Uninstall.exe"

  WriteRegStr HKCU "Software\QuickShelf" "InstallLocation" "$INSTDIR"
  WriteRegStr HKCU "Software\Microsoft\Windows\CurrentVersion\Uninstall\QuickShelf" "DisplayName" "QuickShelf"
  WriteRegStr HKCU "Software\Microsoft\Windows\CurrentVersion\Uninstall\QuickShelf" "DisplayVersion" "${VERSION}"
  WriteRegStr HKCU "Software\Microsoft\Windows\CurrentVersion\Uninstall\QuickShelf" "Publisher" "QuickShelf"
  WriteRegStr HKCU "Software\Microsoft\Windows\CurrentVersion\Uninstall\QuickShelf" "InstallLocation" "$INSTDIR"
  WriteRegStr HKCU "Software\Microsoft\Windows\CurrentVersion\Uninstall\QuickShelf" "DisplayIcon" "$INSTDIR\QuickShelf.exe"
  WriteRegStr HKCU "Software\Microsoft\Windows\CurrentVersion\Uninstall\QuickShelf" "UninstallString" '"$INSTDIR\Uninstall.exe"'
  WriteRegStr HKCU "Software\Microsoft\Windows\CurrentVersion\Uninstall\QuickShelf" "QuietUninstallString" '"$INSTDIR\Uninstall.exe" /S'
  WriteRegDWORD HKCU "Software\Microsoft\Windows\CurrentVersion\Uninstall\QuickShelf" "NoModify" 1
  WriteRegDWORD HKCU "Software\Microsoft\Windows\CurrentVersion\Uninstall\QuickShelf" "NoRepair" 1
SectionEnd

Section "Uninstall"
  SetShellVarContext current
  Delete "$SMPROGRAMS\QuickShelf\QuickShelf.lnk"
  Delete "$SMPROGRAMS\QuickShelf\Uninstall QuickShelf.lnk"
  RMDir "$SMPROGRAMS\QuickShelf"
  DeleteRegKey HKCU "Software\Microsoft\Windows\CurrentVersion\Uninstall\QuickShelf"
  DeleteRegKey HKCU "Software\QuickShelf"
  ; User-authored snippets intentionally live in $LOCALAPPDATA\QuickShelf and are preserved.
  RMDir /r "$INSTDIR"
SectionEnd
