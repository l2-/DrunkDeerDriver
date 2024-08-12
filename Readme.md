#### Note: !! Tested on Windows 10 with a Drunk Deer G65 Keyboard with firmware v0.48. Use at your own risk, I'm not responsible for what happens to your keyboard.
#### Note: !! You can find the portable exe in Releases. Windows Defender might trigger for this App with a Wacatac Trojan warning. This is a false positive as reinforced by [the Virus total result](https://www.virustotal.com/gui/file/f8b5f5262351e7760a244d64f4bb181f05e62a9f06a921b7a1a9c2d7c945fd79), not a single virus scanner is triggered by the app.
### Unofficial Drunk Deer Driver
This program allows for easy switching between profiles for drunk deer keyboards. Currently it supports importing profiles as made with the webdriver.
Minimizing the program puts it in system tray. Exiting the program closes it. The startup shortcut points to the location of the exe so if the exe is moved you will need to unselect and select the start on windows startup again.

For profiles in this program you can quickly switch between them in the following ways:
- *Right click the deer icon in the system tray and select one of the profiles there. The profile with the deer icon is the currently active one. Hover the deer icon in the tray to show the currently active one.
- *Use Ctrl + Alt + Enter to cycle through the profiles that are marked as quick switch.
- Select processes that will trigger a profile change, per profile. When 1 of these processes takes the foreground in windows the app will switch to the first associated profile.

Note: The above marked with * do work.
However, since the app will always push the (marked as) default profile when a process that is not associated with any profile takes the foreground, it will seem like it does not work.
If you wish to specifically use those 2 methods I suggest not selecting any profile as default.dddd

### Info
Tested in the following environment:
- Windows 10 version 22H2
- Drunk Deer G65
- Keyboard firmware v0.48
- Web driver v0.09 (exported profiles from the webdriver to import with this app)
- Windows user account has admin rights

As I understood it the keyboard does not have internal storage to store more profiles other than the active one.
Therefore this App writes the default profile whenever it is started and starts tracking the active profile from there on.
If you change the active profile through the web driver this App will not know about it.

**If you know why windows defender gives a false positive for this App feel free to open an Issue for it.**

### Screenshots
**Main window**\
![Main window](https://i.imgur.com/cFSQTs8.png)

**Process selection**\
![Process selection](https://i.imgur.com/8Rbb4gX.png)

**Tray menu**\
![Tray menu](https://i.imgur.com/oyuNZyR.png)