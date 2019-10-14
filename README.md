
# Hue Updater

Updates the color of a Hue light based on the status of multiple CI projects.

![Stable, Stable & Building, Broken, Broken & Building](https://i.imgur.com/YuEo7Ak.jpg)
Stable | Stable & Building | Broken | Broken & Building


## Build status

AppVeyor status:  [![AppVeyor status](https://ci.appveyor.com/api/projects/status/9xebpi3ve7ujf2vb?svg=true)](https://ci.appveyor.com/project/jorgeyanesdiez/HueUpdater)

Sonarcloud status: [![Sonarcloud status](https://sonarcloud.io/api/project_badges/measure?project=jorgeyanesdiez_HueUpdater&metric=alert_status)](https://sonarcloud.io/dashboard?id=jorgeyanesdiez_HueUpdater)


## Motivation

I use a lamp on my desk at work to give me feedback about the status of multiple CI projects.

This application manages that lamp.

The current version connects to a Jenkins instance and a TeamCity instance, and aggregates the status values to determine the light color.

Depending on a defined schedule and a calendar, it then turns the lamp on/off, and sets the light color accordingly.


## Usage prerequisites

* Hue hub, lights, CI systems and all networking must have been already set up and in working order.
* An API key to control the Hue light.
* User credentials for the Jenkins instance.
* User credentials for the TeamCity instance.
* The user that runs the application must have write permissions on the location where the last status file is written.


## Deployment

Unpack the release file where desired.
Open the *appsettings.json* file and tweak the values as needed:

* *LastStatusFilePath* - Full or relative path to the file to be used to keep track of the last status. At least a file name must be specified.
* **Hue** -> *Endpoint* - API endpoint of the light to control. Example: `http://192.168.0.1/api/0123456789012345678901234567890123456789/lights/1/state`
* **Jenkins** -> *BaseEndpoint* - Base of the Jenkins instance. Example: `http://jenkins-server.mycompany.com`
* **Jenkins** -> *JobNameRegexFilter* - Optional regex to filter jobs by name. May be left empty. Example: `Project abc`
* **Jenkins** -> *User* - The user name to authenticate requests with.
* **Jenkins** -> *Password* - The password to authenticate requests with.
* **TeamCity** -> *BaseEndpoint* - Base of the TeamCity instance. Example: `http://teamcity-server.mycompany.com`
* **TeamCity** -> *User* - The user name to authenticate requests with.
* **TeamCity** -> *Password* - The password to authenticate requests with.

You may also change the calendar and schedule, but keep in mind the following:

* The appsettings.json file must remain valid at all times (use valid json only)
* The **Operation** -> **Schedule** section can have as many keys as desired.
* The keys in the **Operation** -> **Calendar** -> **Defaults** section must match the keys in the **Operation** -> *Schedule* section
* The keys in the **Operation** -> **Calendar** -> **DayOverrides** section must match the keys in the **Operation** -> *Schedule* section
* The values in the **Operation** -> **Calendar** -> **DayOverrides** section must be valid days in English.
* The names of the keys must be chosen carefully, matches will be resolved in alphabetical order.

Once done, schedule a task to run this application frequently. That's all there is to it.


## License

This project is licensed under the MIT License - see the [LICENSE.md](LICENSE.md) file for details


## Future plans

* Make the providers pluggable.
* Add more providers.
