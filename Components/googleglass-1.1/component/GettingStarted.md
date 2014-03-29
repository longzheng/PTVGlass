You will need to be part of the Google Glass™ Explorer Program to run Google Glass applications, you can use [this form to sign up](http://www.google.com/glass/start/how-to-get-one/?source=xamarin).

Getting Glass ready for development
===================================
1. Make sure you install [XE12](https://developers.google.com/glass/release-notes) on your Google Glass (released 17th December 2013)
2. On Glass go to **Settings > Device Info > Turn on debug** to enable `adb`, which allows your development system to communicate with Glass.
3. Connect Glass to your development system.
4. Deploy your own Glassware!

Your Xamarin.Android Project Settings
=====================================

Inside your project settings go to **Build > General > Target framework** and make sure it is set to **Android 4.0.3 (Ice Cream Sandwich)**

![Project Settings](http://farm8.staticflickr.com/7289/11215596686_aef587e237_o.png)

Inside  **Build > Android Application > Minimum/Target Android Version** must be set to **Automatic - use target framework version**

![Android Manifest](http://farm4.staticflickr.com/3698/11215581504_f9b61ddc6d_o.png)

There is only one Glass version, so minimum and target SDK are the same.

Documentation
=============

You can find their full developer guide on their website [https://developers.google.com/glass/develop/gdk/index](https://developers.google.com/glass/develop/gdk/index)

Branding Guidelines: [https://developers.google.com/glass/brand-guidelines](https://developers.google.com/glass/brand-guidelines)


**Glass is a trademark of Google Inc.**