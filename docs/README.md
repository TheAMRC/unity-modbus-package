# Real Time Functional Manual (RTFM) Manual #

## How do I see the output of this page? ##

To view the documentation homepage, go to https://amrcgithub.shef.ac.uk/pages/IMG/digital-project-template/. When cloning this project, you will need to enable github pages in the repository settings where you will be given the new URL to access them.

## How do I create UML diagrams? ##

It is recommended to use the [PlantUML plugin for VSCode](https://marketplace.visualstudio.com/items?itemName=jebbs.plantuml) or [PlantUML plugin for JetBrains](https://plugins.jetbrains.com/plugin/7017-plantuml-integration/) for UML diagram editing (In addition you can do markdown editing in the same IDE for the RTFM). You will need to configure the plugin to use a local renderer in order for the style sheets to apply correctly which requires installing a couple of dependencies. As per the link above, the easiest way to get this is to run the following commands at the windows command line with administrative privileges:

```powershell
@"%SystemRoot%\System32\WindowsPowerShell\v1.0\powershell.exe" -NoProfile -ExecutionPolicy Bypass -Command "iex ((New-Object System.Net.WebClient).DownloadString('https://chocolatey.org/install.ps1'))" && SET "PATH=%PATH%;%ALLUSERSPROFILE%\chocolatey\bin"

choco install plantuml
```

It is also encouraged to set the output image format to SVG, which allows for searchable text in your diagrams. You can set this as well as the source and image output directories in your IDE by adding the following to settings.json for the plantUML plugin:

```json
{
    "plantuml.exportFormat": "svg",
    "plantuml.diagramsRoot": "docs/diagrams/src",
    "plantuml.exportOutDir": "docs/diagrams/out"
}
```

Ensure that you add the line `!include style.puml` to your plantUML markup to use the AMRC branded stylesheet.

The plantuml specification is available [here](https://plantuml.com/sitemap-language-specification).

## Tips, Tricks, and Workarounds ##

### I Don't Like the Standard Colour Variants ###

The default colours have been chosen as a compromise between standard AMRC report colour compatibility and readability. Often, you will need to override the defaults. If you want to switch to a light or dark variant of the standard colour, add the `<<light_variant>>` or  `<<dark_variant>>` stereotype to your element (e.g `node Foo <<light_variant>> as foo`). Alternatively, by adding `!$black_and_white = 1` to the top of your plantuml, you can force black and white only for the whole document. If you're still not happy please submit an issue, ask Rikki, or edit the style.puml yourself ([see the plantuml skinparam list](https://plantuml-documentation.readthedocs.io/en/latest/formatting/all-skin-params.html)) and submit a pull request.

Defaults | Dark Variants | Light Variants| Black and White
--|--|--|--
<img src="diagrams/out/defaults.png"> | <img src="diagrams/out/dark_variants.png"> | <img src="diagrams/out/light_variants.png"> |<img src="diagrams/out/black_and_white.png">

#### Line Colors ####
To change the default line colors specify the color between the line dashes in the `[#Color]` format. 

For example:

- `a -[#DarkRed]- b` will produce a narrow dark red line
- `a =[#DarkGray]= b` will produce a thick dark gray line 

### Force Arrow Routing ###

Arrow routing is performed by GraphViz. Graph layouting is a NP-complete problem, so algorithms usually take harsh shortcuts.

Typical workarounds include:

* Adding an additional hidden line `a -[hidden]- b`
* Extending the length of a line `a --- b` (more dashes, longer line)
* Specifying preferred direction of lines `a -left- b`, `a -right- b`, `a -up- b`, `a -down- b`. You can use the shorthand for the direction to produce a shorter line than default (e.g. `a -l- b`, `a -r- b`, `a -u- b`, `a -d- b`)
* Swapping association ends (replace `a -- b` with `b -- a`)
* Changing the order of definitions (the order does matter... sometimes)
* Adding empty nodes with background/border colors set to Transparent

### Make Lines Straight ###

By default, PlantUML tries it's best to keep lines straight but sometimes it has to make curved lines. This can get ugly pretty quickly.

You can force it to try and use straighter routes by uncommenting the following directives at the top of of the boilerplate.puml file:

(default) | `skinparam linetype ortho` | `skinparam linetype polyline`
--|--|--
<img src="diagrams/out/normal_lines.png"> | <img src="diagrams/out/ortho_lines.png"> | <img src="diagrams/out/poly_lines.png">

### Adding Images ###

To add an image to a diagram use the `<img: image_path>` command with an `object` or `class` element. 

For example:

```
object "My Image" as my_image
my_image : <img:C:/my_image.svg>
```

This will insert your chosen image inside the `object` element.

This feature works natively when developing a [*class diagram*](https://plantuml.com/class-diagram). If you are developing a [*deployment diagram*](https://plantuml.com/deployment-diagram) or similar then you also **must use `allow_mixing`** command at the top of the *uml* code.

### Participant labels outside of shape are not visible ###
When a participant shape is too small to fit its descriptive text inside, the label is pushed underneath it. Since the text is white, the label seems to disappear as the background is also white.

A workaround for this is to enclose your participant label in an HTML color tag and apply a black color to it: ```participant"<color:Black>Foo1</color>" as Foo1```.

PlantUML does not provide attributes which allow the stylesheet to work out if the label is inside the shape or not. It would be nice to conditionally format these labels, but at present this does not seem to be possible.

### My problem isn't listed here ###

Please submit an issue or ask Rikki :-) If you managed to fix your issue, please either add a workaround here or submit a pull request.
