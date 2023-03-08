# This installs two files, LoLMasteryTool.exe and LoLMasteryToolIcon.ico, creates a start menu shortcut, builds an uninstaller, and
# adds uninstall information to the registry for Add/Remove Programs
 
# To get started, put this script into a folder with the two files (LoLMasteryTool.exe, LoLMasteryToolIcon.ico, and license.rtf -
# You'll have to create these yourself) and run makensis on it
 
# If you change the names "LoLMasteryTool.exe", "LoLMasteryToolIcon.ico", or "license.rtf" you should do a search and replace - they
# show up in a few places.
# All the other settings can be tweaked by editing the !defines at the top of this script
!define APPNAME "LoLMasteryTool"
!define COMPANYNAME "Fluruxion"
!define DESCRIPTION "LoL tool for tracking mastery information"
# These three must be integers
!define VERSIONMAJOR 1
!define VERSIONMINOR 2
!define VERSIONBUILD 0
 
RequestExecutionLevel admin ;Require admin rights on NT6+ (When UAC is turned on)
 
InstallDir "$PROGRAMFILES\${APPNAME}"


# This will be in the installer/uninstaller's title bar
Name "${COMPANYNAME} - ${APPNAME}"
Icon "LoLMasteryToolIcon.ico"
outFile "LoLMasteryToolInstaller.exe"

BrandingText "Floopstickle Inc B)"

!include LogicLib.nsh

page directory
Page instfiles

section "install"
	# Files for the install directory - to build the installer, these should be in the same directory as the install script (this file)
	setOutPath $INSTDIR
	# Files added here should be removed by the uninstaller (see section "uninstall")
	file "bin\Release\ChangeLog.txt" 
	file "bin\Release\Json.Net.dll" 
	file "bin\Release\LoLMasteryTool.exe" 
	file "bin\Release\LoLMasteryTool.exe.config" 
	file "bin\Release\LoLMasteryTool.pdb" 
	file "bin\Release\MessagePack.Annotations.dll" 
	file "bin\Release\MessagePack.Annotations.xml" 
	file "bin\Release\MessagePack.dll" 
	file "bin\Release\MessagePack.xml" 
	file "bin\Release\Microsoft.Bcl.AsyncInterfaces.dll" 
	file "bin\Release\Microsoft.Bcl.AsyncInterfaces.xml" 
	file "bin\Release\Microsoft.ServiceHub.Client.dll" 
	file "bin\Release\Microsoft.ServiceHub.Client.xml" 
	file "bin\Release\Microsoft.ServiceHub.Framework.dll" 
	file "bin\Release\Microsoft.ServiceHub.Framework.xml" 
	file "bin\Release\Microsoft.VisualStudio.RemoteControl.dll" 
	file "bin\Release\Microsoft.VisualStudio.RemoteControl.xml" 
	file "bin\Release\Microsoft.VisualStudio.RpcContracts.dll" 
	file "bin\Release\Microsoft.VisualStudio.RpcContracts.xml" 
	file "bin\Release\Microsoft.VisualStudio.Telemetry.dll" 
	file "bin\Release\Microsoft.VisualStudio.Telemetry.xml" 
	file "bin\Release\Microsoft.VisualStudio.Threading.dll" 
	file "bin\Release\Microsoft.VisualStudio.Threading.xml" 
	file "bin\Release\Microsoft.VisualStudio.Utilities.dll" 
	file "bin\Release\Microsoft.VisualStudio.Utilities.Internal.dll" 
	file "bin\Release\Microsoft.VisualStudio.Utilities.Internal.xml" 
	file "bin\Release\Microsoft.VisualStudio.Utilities.xml" 
	file "bin\Release\Microsoft.VisualStudio.Validation.dll" 
	file "bin\Release\Microsoft.VisualStudio.Validation.xml" 
	file "bin\Release\Microsoft.Win32.Registry.dll" 
	file "bin\Release\Microsoft.Win32.Registry.xml" 
	file "bin\Release\Nerdbank.Streams.dll" 
	file "bin\Release\Nerdbank.Streams.pdb" 
	file "bin\Release\Nerdbank.Streams.xml" 
	file "bin\Release\Newtonsoft.Json.dll" 
	file "bin\Release\Newtonsoft.Json.xml" 
	file "bin\Release\StreamJsonRpc.dll" 
	file "bin\Release\StreamJsonRpc.xml" 
	file "bin\Release\System.Buffers.dll" 
	file "bin\Release\System.Buffers.xml" 
	file "bin\Release\System.Collections.Immutable.dll" 
	file "bin\Release\System.Collections.Immutable.xml" 
	file "bin\Release\System.Diagnostics.DiagnosticSource.dll" 
	file "bin\Release\System.Diagnostics.DiagnosticSource.xml" 
	file "bin\Release\System.IO.Pipelines.dll" 
	file "bin\Release\System.IO.Pipelines.xml" 
	file "bin\Release\System.Memory.dll" 
	file "bin\Release\System.Memory.xml" 
	file "bin\Release\System.Numerics.Vectors.dll" 
	file "bin\Release\System.Numerics.Vectors.xml" 
	file "bin\Release\System.Runtime.CompilerServices.Unsafe.dll" 
	file "bin\Release\System.Runtime.CompilerServices.Unsafe.xml" 
	file "bin\Release\System.Security.AccessControl.dll" 
	file "bin\Release\System.Security.AccessControl.xml" 
	file "bin\Release\System.Security.Principal.Windows.dll" 
	file "bin\Release\System.Security.Principal.Windows.xml" 
	file "bin\Release\System.Threading.AccessControl.dll" 
	file "bin\Release\System.Threading.AccessControl.xml" 
	file "bin\Release\System.Threading.Tasks.Dataflow.dll" 
	file "bin\Release\System.Threading.Tasks.Dataflow.xml" 
	file "bin\Release\System.Threading.Tasks.Extensions.dll" 
	file "bin\Release\System.Threading.Tasks.Extensions.xml" 
	file "bin\Release\TestOutput.txt" 
	file "bin\Release\YamlDotNet.dll" 
	file "bin\Release\YamlDotNet.xml" 


	createDirectory "$INSTDIR\ChampionIcons"
	setOutPath $INSTDIR\ChampionIcons
	file "bin\Release\ChampionIcons\mast1.png"
	file "bin\Release\ChampionIcons\mast2.png"
	file "bin\Release\ChampionIcons\mast3.png"
	file "bin\Release\ChampionIcons\mast4.png"
	file "bin\Release\ChampionIcons\mast5.png"
	file "bin\Release\ChampionIcons\mast6.png"
	file "bin\Release\ChampionIcons\mast7.png"
	
	createDirectory "$INSTDIR\en"
	setOutPath $INSTDIR\en
	file "bin\Release\en\Microsoft.VisualStudio.Utilities.resources.dll"
	
	setOutPath $INSTDIR
	
	file "LoLMasteryToolIcon.ico"
	# Add any other files for the install directory (license files, app data, etc) here
 
	# Uninstaller - See function un.onInit and section "uninstall" for configuration
	writeUninstaller "$INSTDIR\uninstall.exe"
 
	# Start Menu
	createShortCut "$SMPROGRAMS\${APPNAME}.lnk" "$INSTDIR\LoLMasteryTool.exe" "" "$INSTDIR\LoLMasteryToolIcon.ico"
 
	# Registry information for add/remove programs
	WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${COMPANYNAME} ${APPNAME}" "DisplayName" "${APPNAME} by ${COMPANYNAME}"
	WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${COMPANYNAME} ${APPNAME}" "UninstallString" "$\"$INSTDIR\uninstall.exe$\""
	WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${COMPANYNAME} ${APPNAME}" "QuietUninstallString" "$\"$INSTDIR\uninstall.exe$\" /S"
	WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${COMPANYNAME} ${APPNAME}" "InstallLocation" "$\"$INSTDIR$\""
	WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${COMPANYNAME} ${APPNAME}" "DisplayIcon" "$\"$INSTDIR\LoLMasteryToolIcon.ico$\""
	WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${COMPANYNAME} ${APPNAME}" "Publisher" "$\"${COMPANYNAME}$\""
	# There is no option for modifying or repairing the install
	WriteRegDWORD HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${COMPANYNAME} ${APPNAME}" "NoModify" 1
	WriteRegDWORD HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${COMPANYNAME} ${APPNAME}" "NoRepair" 1
sectionEnd
 
# Uninstaller
 
 
section "uninstall"
 
	# Remove Start Menu launcher
	delete "$SMPROGRAMS\${APPNAME}.lnk"
 
	# Remove files
	delete $INSTDIR\ChangeLog.txt
	delete $INSTDIR\Json.Net.dll
	delete $INSTDIR\LoLMasteryTool.exe
	delete $INSTDIR\LoLMasteryTool.exe.config
	delete $INSTDIR\LoLMasteryTool.pdb
	delete $INSTDIR\MessagePack.Annotations.dll
	delete $INSTDIR\MessagePack.Annotations.xml
	delete $INSTDIR\MessagePack.dll
	delete $INSTDIR\MessagePack.xml
	delete $INSTDIR\Microsoft.Bcl.AsyncInterfaces.dll
	delete $INSTDIR\Microsoft.Bcl.AsyncInterfaces.xml
	delete $INSTDIR\Microsoft.ServiceHub.Client.dll
	delete $INSTDIR\Microsoft.ServiceHub.Client.xml
	delete $INSTDIR\Microsoft.ServiceHub.Framework.dll
	delete $INSTDIR\Microsoft.ServiceHub.Framework.xml
	delete $INSTDIR\Microsoft.VisualStudio.RemoteControl.dll
	delete $INSTDIR\Microsoft.VisualStudio.RemoteControl.xml
	delete $INSTDIR\Microsoft.VisualStudio.RpcContracts.dll
	delete $INSTDIR\Microsoft.VisualStudio.RpcContracts.xml
	delete $INSTDIR\Microsoft.VisualStudio.Telemetry.dll
	delete $INSTDIR\Microsoft.VisualStudio.Telemetry.xml
	delete $INSTDIR\Microsoft.VisualStudio.Threading.dll
	delete $INSTDIR\Microsoft.VisualStudio.Threading.xml
	delete $INSTDIR\Microsoft.VisualStudio.Utilities.dll
	delete $INSTDIR\Microsoft.VisualStudio.Utilities.Internal.dll
	delete $INSTDIR\Microsoft.VisualStudio.Utilities.Internal.xml
	delete $INSTDIR\Microsoft.VisualStudio.Utilities.xml
	delete $INSTDIR\Microsoft.VisualStudio.Validation.dll
	delete $INSTDIR\Microsoft.VisualStudio.Validation.xml
	delete $INSTDIR\Microsoft.Win32.Registry.dll
	delete $INSTDIR\Microsoft.Win32.Registry.xml
	delete $INSTDIR\Nerdbank.Streams.dll
	delete $INSTDIR\Nerdbank.Streams.pdb
	delete $INSTDIR\Nerdbank.Streams.xml
	delete $INSTDIR\Newtonsoft.Json.dll
	delete $INSTDIR\Newtonsoft.Json.xml
	delete $INSTDIR\StreamJsonRpc.dll
	delete $INSTDIR\StreamJsonRpc.xml
	delete $INSTDIR\System.Buffers.dll
	delete $INSTDIR\System.Buffers.xml
	delete $INSTDIR\System.Collections.Immutable.dll
	delete $INSTDIR\System.Collections.Immutable.xml
	delete $INSTDIR\System.Diagnostics.DiagnosticSource.dll
	delete $INSTDIR\System.Diagnostics.DiagnosticSource.xml
	delete $INSTDIR\System.IO.Pipelines.dll
	delete $INSTDIR\System.IO.Pipelines.xml
	delete $INSTDIR\System.Memory.dll
	delete $INSTDIR\System.Memory.xml
	delete $INSTDIR\System.Numerics.Vectors.dll
	delete $INSTDIR\System.Numerics.Vectors.xml
	delete $INSTDIR\System.Runtime.CompilerServices.Unsafe.dll
	delete $INSTDIR\System.Runtime.CompilerServices.Unsafe.xml
	delete $INSTDIR\System.Security.AccessControl.dll
	delete $INSTDIR\System.Security.AccessControl.xml
	delete $INSTDIR\System.Security.Principal.Windows.dll
	delete $INSTDIR\System.Security.Principal.Windows.xml
	delete $INSTDIR\System.Threading.AccessControl.dll
	delete $INSTDIR\System.Threading.AccessControl.xml
	delete $INSTDIR\System.Threading.Tasks.Dataflow.dll
	delete $INSTDIR\System.Threading.Tasks.Dataflow.xml
	delete $INSTDIR\System.Threading.Tasks.Extensions.dll
	delete $INSTDIR\System.Threading.Tasks.Extensions.xml
	delete $INSTDIR\TestOutput.txt
	delete $INSTDIR\YamlDotNet.dll
	delete $INSTDIR\YamlDotNet.xml
	delete $INSTDIR\ChampionIcons\mast1.png
	delete $INSTDIR\ChampionIcons\mast2.png
	delete $INSTDIR\ChampionIcons\mast3.png
	delete $INSTDIR\ChampionIcons\mast4.png
	delete $INSTDIR\ChampionIcons\mast5.png
	delete $INSTDIR\ChampionIcons\mast6.png
	delete $INSTDIR\ChampionIcons\mast7.png
	delete $INSTDIR\en\Microsoft.VisualStudio.Utilities.resources.dll
	delete $INSTDIR\LoLMasteryToolIcon.ico
	rmDir $INSTDIR\en
	rmDir $INSTDIR\ChampionIcons

	# Always delete uninstaller as the last action
	delete $INSTDIR\uninstall.exe
 
	# Try to remove the install directory - this will only happen if it is empty
	rmDir $INSTDIR
 
	# Remove uninstaller information from the registry
	DeleteRegKey HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${COMPANYNAME} ${APPNAME}"
sectionEnd