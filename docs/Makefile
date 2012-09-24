PAGES=/home/bentkus/Projects/gh-pages/sundown.net

.PHONY: monodocs html edit copy

monodocs:
	monodocer -assembly:../bin/Debug/SundownNet.dll -path:monodocs/en -pretty

edit:
	monodoc --edit monodocs/en


html:
	monodocs2html --debug --template=template.xsl monodocs/en -o html

copy: html
	cp html/index.html $(PAGES)/documentation.html
	cp -rvf html/Sundown/ $(PAGES)

clean:
	rm -rf html
