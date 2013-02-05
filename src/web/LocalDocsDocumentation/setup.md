Setup
=====

Webserver
---------
You need some kind of webserver that can run .NET/Mono.

LocalDocs is developed on a Linux machine running Nginx and Mono.

Personally I like to create an actual hostname using */etc/hosts* - usually *localdocs.local*.

The website
-----------
Not much setup is needed here, but you do need to tell LocalDocs where to find your documentation files.

This is done in the **web.config** file.

```xml
<TargetSites>
	<add id="localdocsdocs" name="LocalDocs Documentation" root="LocalDocsDocumentation/" isdefault="true" />
	<add id="some-identifyer-for-your-docs" name="My docs" root="/somewhere/on/your/harddrive/gitclones/project/docs/" />
</TargetSites>
```

Let's go through the attributes 1 by 1...

* **_id_** contains an ID for the site that must be unique within your set of sites. This must be usable in the addressbar of a browser.
* **_name_** contains an overall name for that particular site. It is also used when generating the list of available sites.
* **_root_** contains the relative or absolute path to your documentation files.
* **_isdefault_** is optional and is a boolean that tells LocalDocs which documentation site to show on "bootup". If not supplied on any site
the first site in the list is used.

#### That's it ... let me see that pretty little smile of yours :)