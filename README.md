
# Hue Updater

Updates the color of a Philips Hue API compatible light based on the status of multiple projects.

![Stable, Stable & Building, Broken, Broken & Building](https://i.imgur.com/YuEo7Ak.jpg)
Stable | Stable & Building | Broken | Broken & Building






## Build status

AppVeyor status:  [![AppVeyor status](https://ci.appveyor.com/api/projects/status/9xebpi3ve7ujf2vb/branch/main?svg=true)](https://ci.appveyor.com/project/jorgeyanesdiez/HueUpdater)






## Motivation

I use a lamp at work to give our team instant feedback about the status of multiple projects tracked by our CI system.

This application manages that lamp.

The current version connects to a Jenkins instance and aggregates the status values to determine the light color.

Depending on a defined schedule and a calendar, it then turns the lamp on/off, and sets the light color accordingly.






## Usage prerequisites

* Operational Hue hub, Hue lights, CI systems and related networking equipment.

* An API key to control the Hue light.

* Basic JSON knowledge to edit the settings file.

* Write permission on a local folder to save the file that keeps track of the lamp's last status.

* User credentials for the Jenkins instance, when the Jenkins instance is password protected.






## Deployment

Unpack the release file wherever you want on the target system. I suggest *C:\HueUpdater*
Open the *appsettings.json* file with a plain text editor and carefully tweak the values to match your needs.
Here's an attempt to explain each one, although I hope most are self explanatory from the provided sample file.



* **Persistence** -> ***LastStatusFilePath***

  *HueUpdater* keeps track of the last status of the lamp by writing it to a file. This property determines the location of that file.
  
  It can be a full or relative path. At least a file name must be specified. The default value may be used, but it assumes the user that runs the application has write permissions on the folder that contains the application.
  
  If you plan on using [TrayIcon](https://github.com/jorgeyanesdiez/TrayIcon), set this value to a path and file served by a web server.
  
  If you use *HueUpdater* on the same server as your Jenkins instance, the easiest way is to write this to *userContent/last-status.json* under your Jenkins home.



* **Hue** -> ***Endpoint***

  API endpoint of the light to control.
  
  You will know the IP and API key once you've completed your Hue API setup.

  Example: `http://192.168.0.1/api/0123456789012345678901234567890123456789/lights/1/state`



* **Jenkins** -> ***BaseEndpoint***

  Base URL of your Jenkins instance.

  Example: `https://jenkins-server.mycompany.com`



* **Jenkins** -> ***JobNameRegexFilter***

  Sometimes it's desirable to ignore the failure of some jobs without creating a view in Jenkins.
  
  This optional regular expression acts as a job name filter. May be left empty.

  Example: `Project abc`



* **Jenkins** -> ***User***

  If your Jenkins instance is password protected, set this to your username.
  
  The user name to authenticate requests to Jenkins with.



* **Jenkins** -> ***Password***

  If your Jenkins instance is password protected, set this to your password.
  
  The password to authenticate requests to Jenkins with.





You will likely want to customize when the lamp turns on and off.

The calendar and schedules allow you to do so, and should be modified to match your needs.

* The section **Operation** -> **Schedules** defines time intervals of each day during which the light is on.

  Example:
  ```json
  "2-Regular" : { "Start": "08:45", "Finish": "19:15" }
  ```

  This defines a schedule called *2-Regular* that turns the light on at 8:45 in the morning and turns it off at 19:15 in the evening.



* The section **Operation** -> **Calendar** -> **Defaults** defines date intervals.

  To assign a schedule to a calendar, the keys in this section must match the keys in the **Operation** -> **Schedules** section.

  Example:

  ```json
  "2-Regular": [
    { "Start": "2023/01/05", "Finish": "2023/01/05" },
    { "Start": "2023/02/01", "Finish": "2023/02/20" }
  ]
  ```

  This defines a calendar that applies the *2-Regular* schedule during the fifth day of year 2023, and for the first 20 days in February of year 2023.



* The section **Operation** -> **Calendar** -> **DayOverrides** defines schedule overrides based on the day of the week.

  Example:
  ```json
  "1-Off": [ "Saturday", "Sunday" ]
  ```

  This defines an override that causes the schedule to be *1-Off* on Saturdays and Sundays. Following the previous examples, this would mean that on days February 4, 5, 11, 12, 18, 19, 25 and 26 of year 2023, the applicable schedule will be *1-Off* and not *2-Regular*.



* The values in the **Operation** -> **Calendar** -> **DayOverrides** section must be valid days in English.



* The section **Operation** -> **Calendar** -> **DayOverridesExclusions** defines schedules that should not be overridden.

  Example:

  ```json
  "DayOverridesExclusions": [ "1-Off" ]
  ```

  This declares that, if the default schedule is 1-Off, then it should not be overridden.

* The names of the keys in all sections must be chosen carefully, since matches will be resolved in alphabetical order.



* **REMEMBER:** The *appsettings.json* file must remain valid at all times. Use valid JSON only.





Finally, use your Windows Task Scheduler or alternative scheduling method to run this application frequently.

I usually run it every minute.

That's all there is to it.






## License

This project is licensed under the MIT License - see the [LICENSE.md](LICENSE.md) file for details
