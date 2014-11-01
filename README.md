TeamCity Build Monitor
===

Looking for a easy way to display your current TeamCity build status on a monitor? Then look below for a demo!

![Sample monitor](/Images/Demo.png)

As you can see, the project is a simple website that shows the build status as green (all good), red (some builds have failed), or orange (all failed builds are under investigation) 
for TeamCity. If there are any builds failed, the monitor will also show the list of failed project/build configuration, along with the investigator assigned if any.
The polling is done at a two minute interval. And you can set your TeamCity REST api root in the Root string constant of BuildMonitor.cs.

Enjoy~

Support: TeamCity server 8.1+