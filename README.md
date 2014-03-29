# PTVGlass 

##About
By [Long Zheng](http://www.istartedsomething.com) / [@longzheng](https://twitter.com/longzheng). Read the [blog post](http://www.istartedsomething.com/20140329/making-a-google-glass-app-with-c-on-xamarin-ptvglass/).

Melbourne public transport timetable on Google Glass. (C# Xamarin for Android/Google Glass project)

- List next departures from nearby train/tram/bus stops (via "trams nearby" voice/menu command)
- List next departures for trains at specified City stops (via "trains departing, Southern Cross" voice/menu command)

##Download binary
Download an APK to sideload to Google Glass via ADB.

##Screenshots
![](http://longzheng.github.io/PTVGlass/ptvglass.png)

![](http://longzheng.github.io/PTVGlass/20140326_112346_933_x.png)

##Source compile
You must supply your own PTV Timetable API developer ID and security key (free) from PTV. See [API documentation for instructions to get a key](http://stevage.github.io/PTV-API-doc/#header4).

Put your key in `\PTVGlass\Services\PtvApi.cs`

    public class PtvApi
    	{
    		string ptvDevId = "";
    		string ptvDevKey = "";

##Credits
- [Public Transport Victoria API](https://www.data.vic.gov.au/raw_data/ptv-timetable-api/6056)
- [Steve Bennett unofficial PTV API documentation](http://stevage.github.io/PTV-API-doc/#header18)
- [Leon Gouletsas](http://leon.gouletsas.com/) provided additional transport data
- [Chris Hardy @ Xamarin](https://twitter.com/chrisntr)
- C# tech support by [David Golden](https://twitter.com/GoldenTao) and [Rafael Rivera](https://twitter.com/withinrafael)
