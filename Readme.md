# OKHOSTING.Core

Common basic functionality for any project.

![Build status](https://ci.appveyor.com/api/projects/status/32r7s2skrgm9ubva?svg=true)

[![Join the chat at https://gitter.im/okhosting/OKHOSTING.Core](https://badges.gitter.im/okhosting/OKHOSTING.Core.svg)](https://gitter.im/okhosting/OKHOSTING.Core?utm_source=badge&utm_medium=badge&utm_campaign=pr-badge&utm_content=badge)

## Download 

```
PM> Install-Package OKHOSTING.Core 
```

[Download on NuGet](https://www.nuget.org/packages/OKHOSTING.Core/)

## PCL, usable from any device

* Base classes for extending Dictionary<,> and List<>
* Extension methods for Type, String, Char, DateTime, XmlDocument and some others

## Net4

* Write log files
* Set any executable to Autostart on user login
* Shell proxy you can use to execute sequential command line commands
* Send email with templates
* Handle desktop/web user session's with the same code
* Send email templates
* Chek if there's an internet connection
* Geolocalize an IP address
* Create a Telnet connection 
* Reand / write your web.config settings

## Samples

On PCL you can use some simple but usefull extensions for String, DateTime, Type, Char, etc.

```csharp
DateTime date = new DateTime(2016, 2, 27);

Assert.IsTrue(date.IsWeekend());
Assert.Equals(date.GetLastDayOfMonth(), new DateTime(2016, 2, 29));
```

On Net4 you can read & write appsettings


```csharp
AppConfig config = new AppConfig();

string connectrionString = "my connection string";

config.SetAppSetting("connectionString", connectrionString);
config.Save();

Assert.Equals((string) config.GetValue("connectionString", typeof(string)), connectrionString);
```

Or set your app to start automatically with windows

```csharp
string location = System.Reflection.Assembly.GetExecutingAssembly().Location;

AutoStart.SetAutoStart(location);
Assert.IsTrue(AutoStart.IsAutoStartEnabled(location));

AutoStart.UnSetAutoStart(location);
Assert.IsFalse(AutoStart.IsAutoStartEnabled(location));
```

Or inherit from ConfigurationBase to create your custom configurations, and read / write them from disk really easy

```csharp
public class MyConfiguration: OKHOSTING.Core.Net4.ConfigurationBase
{
	public string Value1 { get; set; }
	public DateTime Value2 { get; set; }
	public int Value3 { get; set; }
	public decimal Value4 { get; set; }
}

...

var config = new MyConfiguration();
config.Value1 = "hello";
config.Value2 = DateTime.Today;
config.Value3 = 100;
config.Value4 = 12.5M;

//save to disk
config.Save();

//then load it from disk
config = (MyConfiguration) ConfigurationBase.Load(typeof(MyConfiguration));

Assert.Equals(config.Value1, "hello");
Assert.Equals(config.Value2, DateTime.Today);
Assert.Equals(config.Value3, 100);
Assert.Equals(config.Value4, 12.5M);
```

Or just write a log file

```csharp
Log.Write("source", "an error ocurred", Log.HandledException);
```