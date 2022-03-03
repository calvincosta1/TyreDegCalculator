------------------------------------------------------------------------------------------------------------------------
Tyre Deg Calculator - Readme
------------------------------------------------------------------------------------------------------------------------

CONTENTS OF THIS FILE
---------------------
 * Introduction
 * Requirements
 * Recommended System
 * Installation
 * Troubleshooting
 * Planned Feature Enhancements
 * Developer Details

INTRODUCTION
------------
Tyre Deg Calculator is a WPF C# program developed by Calvin Costa. The goal of the program is to showcase
some of my software design capabilities, software development skills and knowledge of best practices.

The program intends to calculate the average tyre degredation based on circuit data aquired from previous F1 
seasons and display this information to the user in an easy to read format. 

The software is required to perform the following tasks:
 - Read Tyre related information from TyresXML.xml
 - Select the various types of tyres for each corner of the F1 car
 - Read Track related information from TrackDegradationCoefficients.txt
 - Select the F1 track
 - Pull in real time weather information based on the selected track
 - Calculate PointTyreDegredation
 - Calculate and display the average PointTyreDegredation
 - Calculate and display the range of PointTyreDegredation

REQUIREMENTS
------------
1. A computer with Microsoft Visual Studio 2022 or later installed is required to compile and run the 
   Tyre Deg Calculator program.
2. An active internet connection to get updated weather information from OpenWeatherMap.
3. Not critical, however, the application looks it's best when viewed on a screen size larger than 17".

RECOMMENDED SYSTEM
------------------
Tyre Deg Calculator was developed using the following system described below:

CPU - Intel(R) Core(TM) i5-4210U @1.70GHz
GPU - Intel Integreated Graphics
RAM - 8GB
OS  - Windows 10 Pro 64-bit
Screen Size - 24"
IDE - Microsoft Visual Studio Community 2022 (64-bit) Version 17.1.0
.NET Framework - 4.8

INSTALLATION
------------
Follow these step to compile and run the Tyre Deg Calculator program:

1. Ensure you have the correct software required.
2. Download all neccessary project files.
3. Double click on the TyreDegCalculator.sln solution file which will launch Visual Studio.
4. Wait a few seconds for the project to load. (Wait time may vary depending on your system).
5. Click on the Start button denoted with the green play button in the menu bar of Visual Studio
   to compile and run the Tyre Deg Calculator program. (Press F5 for a quick keyboard shortcut)
6. Wait a few seconds for the project to compile and run. (Wait time may vary depending on your system)
7. Once the program has loaded a short splash screen will be displayed for a few seconds then loading
   you to the main window of the program.

TROUBLESHOOTING
---------------
Thorough testing has been done on the system making sure there are no errors or bugs. 
At this point in time I am not aware of any bugs that may have creeped in, if any are found 
please do not hesitate to contact me as I'd love to squash them and improve your experience.

For basic troubleshooting if anything were to go wrong I'd reccomend making sure your system meets the
requirements and that you have the updated and correct project files and software. It also never hurts to do 
a system reboot - my go-to step when issues arrise.

PLANNED FEATURE ENHANCEMENTS
----------------------------
There are a few enhancements I see myself doing to my application in order to deliver a more refined experience.
Tyre Deg Calculator is currently in version 1.9 and these are the areas I would like to enhance and develop:

 - Add validations to the Tyre selections
 - Performance improvements such as multithreading so that the UI is more responsive when performing calculations
 - Design a more adaptive UI layout with XAML, such that the application scales according to the users preference
 - UI changes based on user feedback
 - Develop Unit tests with MSTest

DEVELOPER DETAILS
-----------------
Calvin Costa

Contact me via e-mail for any queries or concerns.
calvincosta1@gmail.com.

------------------------------------------------------------------------------------------------------------------------