# Define the name of the installer
OutFile "CSGrokInstaller.exe"

# Define the default installation directory
InstallDir "$LOCALAPPDATA\CSGrok"

# Define the name of the application
Name "CSGrok"

# Define the files to install
!define APP_EXE "CSGrok.exe"
!define APP_CONFIG "CSGrok.exe.config"
!define APP_PDB "CSGrok.pdb"
!define JSON_DLL "Newtonsoft.Json.dll"
!define JSON_XML "Newtonsoft.Json.xml"

# Include modern user interface
!include MUI2.nsh

# Define pages
Page directory
Page instfiles

# Default section
Section ""

  # Set output path to the installation directory
  SetOutPath "$INSTDIR"

  # Install the files
  File "${APP_EXE}"
  File "${APP_CONFIG}"
  File "${APP_PDB}"
  File "${JSON_DLL}"
  File "${JSON_XML}"

  # Read the existing user PATH
  ReadRegStr $0 HKCU "Environment" "Path"
  
  # Check if the installation directory is already in the PATH
  StrCpy $1 "$0"
  ${If} $1 != "" 
    ${If} $1 != "$INSTDIR" ; Checks if the path is not already in the PATH
      ; Append the installation directory to the user PATH
      StrCpy $1 "$0;$INSTDIR"
    ${EndIf}
  ${Else}
    ; If the PATH is empty, just set it to the installation directory
    StrCpy $1 "$INSTDIR"
  ${EndIf}

  # Write the new PATH value to the user environment variable
  WriteRegStr HKCU "Environment" "Path" "$1"
  
  # Notify the system about the PATH change
  System::Call 'Kernel32::SendMessageA(i 0xffff, i ${WM_SETTINGCHANGE}, i 0, i "Environment")'

SectionEnd