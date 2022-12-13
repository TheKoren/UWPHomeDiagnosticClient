# How to use DocFX documentation

## 1. Download DocFX

DocFX can be downloaded from its github page: [DocFX download](https://dotnet.github.io/docfx/)

It is adviced to download the **latest stable** version.

## 2. Add to Path

To use DocFX in a simple shell, you have to add it to your environmental variables.

## 3. Generate DocFX files

Open a command prompt, and navigate to your solutions folder.

To build the docFX documentation, execute the following command:
```
docfx docfx_project/docfx.json
```

This command simply tells docFx to "do what the json tells you to do".

If everything went well, there should be a _site folder under docfx_project.

## 4. Start DocFX webserver.

To start the webserver, execute the following command:

```
docfx serve docfx_project/_site
```

## 5. Profit

Open a browser, and go to [localhost](http://localhost:8080)
