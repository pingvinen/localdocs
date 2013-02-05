Introduction
============

Hello and welcome to the wonderfull world of LocalDocs.

LocalDocs is a small specialized webplatform built to serve as a platform for in-house documentation created by software-teams. The basic
concept is simple; let the documentation live within the code repositories as [Markdown](http://daringfireball.net/projects/markdown/) files and use your browser to read it.



Why?
----
LocalDocs is supposed to solve 2 problems:

1. Documentation changes during development and might be different between each major branch
2. It needs to be fast and available

The first problem is solved by having the documentation files live in the repository. That way you can safely update the documentation
in your branch, without confusing the rest of the team with warnings for updated sections.

The second problem is solved by the small webplatform that *is* LocalDocs. It allows you to navigate the documentation as any other
website and it generates the HTML on-the-fly - you no longer have to run the documentation files through some tool.

The webplatform is designed to be run locally directly on each developer's machine. This ensures speed and availability even on those
horrible days when the internet decides to leave us for a while.

I chose Markdown as the syntax as this seems to be what many developers already use on sites like StackOverflow and Github. Actually the
plan is to use [Github Flavored Markdown](http://github.github.com/github-flavored-markdown/) (without the Github-specific links etc.).



Some of the features
--------------------
All developers work on multiple projects at any given time. LocalDocs supports this. You only need 1 "installation" of LocalDocs, as the
platform supports any number of documentations (called "target sites" in LocalDocs terms).

If you do not like the way LocalDocs looks, you can create your own layout.



This is opensource
------------------
LocalDocs is opensource. It was born on [Github](https://github.com/pingvinen/localdocs/) and it still lives on Github.

This is also where you can go when something misbehaves or you have a brilliant idea. Either create a new issue or 
fork the code and submit a pull-request.