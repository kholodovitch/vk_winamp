// This is the main DLL file.

#include "stdafx.h"

#include "plug-in.h"


// these are callback functions/events which will be called by Winamp
int  init(void);
void config(void);
void quit(void);


// this structure contains plugin information, version, name...
// GPPHDR_VER is the version of the winampGeneralPurposePlugin (GPP) structure
winampGeneralPurposePlugin plugin = {
	GPPHDR_VER,  // version of the plugin, defined in "gen_myplugin.h"
	PLUGIN_NAME, // name/title of the plugin, defined in "gen_myplugin.h"
	init,        // function name which will be executed on init event
	config,      // function name which will be executed on config event
	quit,        // function name which will be executed on quit event
	0,           // handle to Winamp main window, loaded by winamp when this dll is loaded
	0            // hinstance to this dll, loaded by winamp when this dll is loaded
};


// event functions follow

int init() {
	System::Windows::Forms::MessageBox::Show("Init event triggered for gen_myplugin. Plugin installed successfully!");
	return 0;
}

void config() {
	System::Windows::Forms::MessageBox::Show("Config event triggered for gen_myplugin.");
}

void quit() {
	System::Windows::Forms::MessageBox::Show("Quit event triggered for gen_myplugin.");
}


// This is an export function called by winamp which returns this plugin info.
// We wrap the code in 'extern "C"' to ensure the export isn't mangled if used in a CPP file.
extern "C" __declspec(dllexport) winampGeneralPurposePlugin * winampGetGeneralPurposePlugin() {
	return &plugin;
}