# Automated Video Teleconferencing System

**This is a hardware quality testing tool on the platform level with the purpose of automating the receiving end of a Teams call to eliminate the need for a second tester on the opposite end of a call.**
![alt text](https://github.com/caroline-jin/vtc-automation/blob/master/images/example.PNG "Example setup")
The tester at the local machine 1 will control the tool remotely through a web interface. The tool installed on the remote machine 2 will respond to the web interface and automate Teams accordingly for test scenarios.

## Installation
* To utilize the tool, download the entire \app folder in the respository [found here](https://github.com/caroline-jin/vtc-automation/tree/master/app "App Folder")
. Do not change any of the folder names or inner locations since all coded paths are relative to the app folder. Place the folder in an easy to access location on your remote test machine. (This can be done either manually or through remote desktop, etc)
* Set the remote machine to Developer Mode by going to Settings > Updates and Security > For developers and choosing "Developer mode". 
* *(Optional)* Connect the HC-05 Bluetooth Component to the remote machine.
* To run the tool, go to app\ConsoleApp1\ConsoleApp1\bin\Debug and start ConsoleApp1.exe. Enter the prompted information. <br>
* The website for the local machine is accessible at http://automationtest2.azurewebsites.net/.

NOTE: The website code [found here](https://github.com/caroline-jin/vtc-automation/tree/master/website "Website Folder") is for reference. The actual code and the SQL database are hosted on Azure. 

## Current Available Functions
When not in a call...
1) Place an outgoing call
2) Pick up an incoming call
3) Screenshot

When in a call...
1) Play recording
2) Toggle video
3) Toggle mic
4) Screen share
5) Add person 
6) Screenshot
7) Send file
8) Hang up

## System Breakdown
![alt text](https://github.com/caroline-jin/vtc-automation/blob/master/images/chart.PNG "System organization")

### 1. Console Script
The automation portion of this tool relies on [WinAppDriver](https://github.com/microsoft/WinAppDriver) and keyboard shortcuts. <br>

The console script itself is not meant for a lot of user interaction besides the inital first time information input required. <br>
Since the script is remote, there are several error checking mechanisms built into the code.
1. *Try/catch* <br> All WinAppDriver code is wrapped in try/catch statements to prevent the program from exiting due to missing UI elements. The code will try 3 times to find the element (considering some UI elements take longer to load) before returning to the main menu if unable to complete the action. <br>NOTE: Functions built on keypresses do not have any error checking or feedback, so the script is unable to determine if a keyboard shortcut was successful. 
2. *Log file* <br> All of the console outputs are also outputted to a text file named the date of the test along with time stamps. The text file is saved in the root app folder. If an error occurs, reading the log file afterwards may help debug the issue.
3. *Batch file* <br> In case of an unaccounted for error or exception, the program may stop responding or exit. If this occurs, there is a Windows task scheduled set up by a Powershell script to run a batch file every 5 minutes to reopen ConsoleApp1.exe. (Diagram found [here](https://github.com/caroline-jin/vtc-automation/blob/master/images/reset.PNG)) The program will restart with a counter at 0, so the user will need to either clear history or refresh the page as well. 

The script is based on two switch stamements (one for in a call and one for out of a call). The only ways to move from one switch statement to the other is on the website to Send a call, Pick up a call OR Hang up, Return to previous options. <br><br>
If the website and the script are in different switch statements, the commands will not be accurate, and the user should probably restart the tool and website session accordingly if unable to realign the two. 

### 2. Website Interface
The website is connected to a cloud-based SQL database on the same Azure account. The SQL database can be accessed through http://automationtest2.azurewebsites.net/api/Items in JSON format or through SQL Management Studio/another SQL database editor by providing the Azure account details. <br>
The required connection string is: <br>

    "ConnectionStrings": {
        "ContextDB": "Server=tcp:websitedb-server1.database.windows.net,1433;Initial Catalog=website_db;Persist Security Info=False;User ID=t-cajin;Password=MicrosoftPassword123;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"
    }
    
The website interacts with the script through JSON packets containing the number of the option chosen (refer to "Current Available Functions" above), the counter, the pin, and the date. <br>

```[{"id":818,"name":"2","extra":"","count":1,"unique_id":"1234","history":"2020/8/17 22:3:17"}]```<br>

The counter refers to how many commands the user has inputted on this website session and serves keep the script in sync. The remote script also has a counter that increments each time it performs a command to prevent it from repeating commands. If the two counters are out of sync, there might be incongruence that would require both counters to be reset. To reset the counter on the website, either refresh the page or click "Clear History." To reset the script, the option "Exit Program" must be chosen from the not in call menu. This will cause the entire program to close, and it must be restarted. <br>

The website provides a dynamic history log based on your last inputted pin. "Clear History" will clear all commands based on that pin and reset the counter to 0. "Delete" will only delete one command at a time and does not affect the counter. "Clear History" is preferred when you are finished with the entire session to keep the database clean.<br>

Check the "IDs in use" section to make sure your pin is not already being used to prevent conflicting commands from other testing devices. <br>

Lastly, the options under the "call" menu cannot be used when not in a call and vise versa. If the script is unable to find a call window, exceptions will be thrown and the action will not be completed. 

### 3. Arduino Component
Schematics and breadboard diagrams are available in the root repository folder. <br>
The Arduino component is necessary for the "Play recording" option, but the rest of the tool is functional without it. <br><br>
*Materials required:*
* Arduino Uno
* HC-05 Bluetooth Module
* YX5300 Catalex Serial MP3 module
* Desk speaker (with audio jack) <br>

When connecting the HC-05 Bluetooth Module to the remote machine, the default password is 1234. To find the outgoing Bluetooth port of the HC-05, go to Control Panel and search Bluetooth. Under Devices and Printers, click "Change Bluetooth settings". Click on the tab "COM Ports" and find the outgoing port of the HC-05, ie, COM4 or COM5. 


