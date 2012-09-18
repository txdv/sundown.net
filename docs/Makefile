

.PHONY: monodocs html edit

monodocs:
	monodocer -assembly:../bin/Debug/SundownNet.dll -path:monodocs/en -pretty

edit:
	monodoc --edit monodocs/en

html:
	monodocs2html source monodocs/en -o html

clean:
	rm -rf html
