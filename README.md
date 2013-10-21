Sundown.net
===========

Sundown.net provides .NET bindings for the nice and popular
[sundown markdown library](https://github.com/tanoku/sundown).

The aim of this library is to provide all the functionality
that sundown provides alongside the possibility to do stuff
quickly (simple API).

It is ridicolously fast compared to pure .NET/Regex implementations.

More information on the [official website](http://txdv.github.com/sundown.net).

Building
========

The harder part is to compile the native markdown library on windows. I used the 'Developer Command Prompt for VS2012'. 

Open the visual studio  command line tool and type `nmake -f Makefile.win` 

License
=======
The wrapper is available under the [MIT license](http://en.wikipedia.org/wiki/MIT_License).
Note that the actual library is not licensed under the MIT license.
