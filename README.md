
# Hue Updater

Updates the color of a Hue light based on the status of multiple CI projects.

![Stable, Stable & Building, Broken, Broken & Building](https://i.imgur.com/YuEo7Ak.jpg)
Stable | Stable & Building | Broken | Broken & Building






## Build status

AppVeyor status:  [![AppVeyor status](https://ci.appveyor.com/api/projects/status/9xebpi3ve7ujf2vb?svg=true)](https://ci.appveyor.com/project/jorgeyanesdiez/HueUpdater)

Sonarcloud status:  [![Sonarcloud status](https://sonarcloud.io/api/project_badges/measure?project=jorgeyanesdiez_HueUpdater&metric=alert_status)](https://sonarcloud.io/dashboard?id=jorgeyanesdiez_HueUpdater)






## Motivation

I use a lamp on my desk at work to give me feedback about the status of multiple CI projects.

This application manages that lamp.

The current version connects to a Jenkins instance and aggregates the status values to determine the light color.

Depending on a defined schedule and a calendar, it then turns the lamp on/off, and sets the light color accordingly.






## Usage prerequisites

* Hue hub, lights, CI systems and all networking must have been already set up and in working order.

* An API key to control the Hue light.

* User credentials for the Jenkins instance.

* The user that runs the application must have write permissions on the location where the last status file is written.








## Deployment

Unpack the release file where desired.
Open the *appsettings.json* file with a plain text editor and tweak the values as needed:



* **Persistence** -> ***LastStatusFilePath***

  Full or relative path to a file to be used to keep track of the last build status. At least a file name must be specified. The default value may be used, but it assumes the user that runs the application has write permissions on the folder that contains the application.



* **Hue** -> ***Endpoint***

  API endpoint of the light to control.

  Example: `http://192.168.0.1/api/0123456789012345678901234567890123456789/lights/1/state`



* **Jenkins** -> ***BaseEndpoint***

  Base of the Jenkins instance.

  Example: `http://jenkins-server.mycompany.com`



* **Jenkins** -> ***JobNameRegexFilter***

  Optional regex to filter jobs by name. May be left empty.

  Example: `Project abc`



* **Jenkins** -> ***User***

  The user name to authenticate requests to Jenkins with.



* **Jenkins** -> ***Password***

  The password to authenticate requests to Jenkins with.





The calendar and schedules should be modified to your needs too.

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
    { "Start": "2022/01/05", "Finish": "2022/01/05" },
    { "Start": "2022/02/01", "Finish": "2022/02/20" }
  ]
  ```

  This defines a calendar that applies the *2-Regular* schedule during the fifth day of year 2022, and for the first 20 days in February of year 2022.



* The section **Operation** -> **Calendar** -> **DayOverrides** defines schedule overrides based on the day of the week.

  Example:
  ```json
  "1-Off": [ "Saturday", "Sunday" ]
  ```

  This defines an override that causes the schedule to be *1-Off* on Saturdays and Sundays. Following the previous examples, this would mean that on days February 5, 6, 12, 13, 19, and 20 of year 2022, the applicable schedule will be *1-Off* and not *2-Regular*.



* The values in the **Operation** -> **Calendar** -> **DayOverrides** section must be valid days in English.



* The section **Operation** -> **Calendar** -> **DayOverridesExclusions** defines schedules that should not be overridden.

  Example:

  ```json
  "DayOverridesExclusions": [ "1-Off" ]
  ```

  This declares that, if the default schedule is 1-Off, then it should not be overridden.

* The names of the keys in all sections must be chosen carefully, since matches will be resolved in alphabetical order.



* The *appsettings.json* file must remain valid at all times. Use valid JSON only.





Once done, schedule a task to run this application frequently.

I use Windows' Task Scheduler to run this every minute or so.

That's all there is to it.






## License

This project is licensed under the MIT License - see the [LICENSE.md](LICENSE.md) file for details






## Future plans

* Make the providers pluggable.
* Allow more than one time interval per schedule.
