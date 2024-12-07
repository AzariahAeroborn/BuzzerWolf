# Introduction 
BuzzerWolf is a team management assistant for the online basketball simulation [BuzzerBeater](https://www.buzzerbeater.com)

# Getting Started
In order to develop for BuzzerWolf, you will need access to the following:
1. Visual Studio 2022 Community or better, configured with the ASP.NET and web development workload, .NET 8.0 Runtime, and NuGet package manager.
2. SQL Server Express 

## Visual Studio installation
A minimum of Visual Studio 2022 is required for BuzzerWolf development.  The Community edition has all required features and BuzzerWolf qualifies as an open source software project under the VS Community licensing terms.  
You will need to install the "ASP.NET and web development" workloado, which should install most required individual components.  After selecting the workload, go to the individual components tab and ensure that the 
".NET 8.0 Runtime (Long Term Support)" and "NuGet package manager" components are also installed.

## SQL Server Express installation
SQL Server Express is used for a local cache database, to allow development of the caching data layer we use online (the online caching data layer is an Azure SQL Database, which runs MS SQL Server).  
Download SQL Server Express from Microsoft's website, and then install it on the same machine you're going to be doing development from.  The default / Basic settings will create a SQL Server instance that can only be accessed from the 
same machine it is installed on, with no authentication required.  After SQL Server Express installation completes, you can either press the button on the installer to open a SQL command prompt, or install a graphical tool such as
SQL Server Management Studio (SSMS) to connect to the database.  When connected to the database, you need to run the command `CREATE DATABASE [buzzerwolf]` to create the initial database.  
No other setup steps are required for initalization -- the BuzzerWolf server will automatically initalize the database with the required tables and data on first run.

# Setup & First run 
After checking out the code, you will need to set up a local version of the appsettings.Development.config file to populate required information for connecting to the BBAPI for certain synchronization operations.
1. Navigate to the folder where you checked out the BuzzerWolf code
2. Go to the BuzzerWolf.Server folder, and copy appsettings.Development.json.sample and rename the copy to appsettings.Development.json
3. Open the appsettings.Development.json file and fill in the UserName and AccessKey values for the BBAPIPublicUser configuration value with your BuzzerBeater user name and BBAPI access key.  
This credential is used by certain synchronization tasks to retrieve data that isn't team specific (league standings, team schedules, etc) but the online service needs to cache periodically to reduce load times and the number of 
requests we send to the BBAPI.  Setting that in appsettings.Development.json will only use your API account when you're developing on your local machine and won't effect the account we use online or provide data from your API access
to any other developer or user. **Be sure not to add any credentials in the appsettings.Development.json.sample file!**
**The solution's .gitignore file is set up to ensure you don't accidentally commit changes to the running appsettings.Development.json file, which needs those credentials, but changes to the sample file will be added to source control and visible to other people.**

# Contribute
All contributions must be submitted under the MIT License