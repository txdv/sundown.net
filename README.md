Sundown.net
===========

Sundown.net provides .NET bindings for the nice and popular
[sundown markdown library](https://github.com/tanoku/sundown).

The aim of this library is to provide all the functionality
that sundown provides alongside the possibility to do stuff
quickly (simple API).

It is ridicolously fast compared to pure .NET/Regex implementations.

##How to Compile

1.  Make sure you've installed Visual C++ component in your current version of Visual Studio. (Because we'll use its compiler to compile native code of [sundown][])
2.  Make suer you've [sundown] code ready. See if the folder sundown/ is empty. If yes, execute following git commands:    
    
    git submodule init  
    git submodule update  

3.  Compile in VisualStudio!  

License
=======
The wrapper is available under the [MIT license](http://en.wikipedia.org/wiki/MIT_License).
Note that the actual library is not licensed under the MIT license.

[sundown]:https://github.com/tanoku/sundown