# Fright: Unity C# Templates
A simple tool for creating C# files in Unity using extensible templates

## Purpose
The purpose of this tool is to make creation of new Unity scripts easier by providing templates. These templates fill out boilerplate code such as usings, namespaces, and common functions. Additionally the template builder normalizes files for line endings.

![Template Builder Window Preview](https://github.com/StaticPtr/Fright-Unity-CSharp-Templates/blob/master/Readme%20Assets/TemplateBuilderWindow-0001.png?raw=true)

## Usage
1) Download the `.unitypackage` release file and import it into your project
2) Select a folder in the Unity Project view, right click, and select "Template Window"
3) Select a template from the template picker. For example "Class", "Struct", "MonoBehaviour", etc...
4) Fill in any options for that template such as it's name
5) Press "Create" to create that file in the folder you have selected

## Creating Your Own Templates
The template builder window will automatically pick up any `.xtemplate` files in the project. `.xtemplate` files are XML files that have a specific structure that the builder window can understand. Every template must contain exactly one `template` tag which defines the name of the template, what `.xtemplate` format it is, and an optional sorting priority. The sorting priority is used to sort the templates in the template picker, in ascending order.

```XML
<template id="NAME OF YOUR TEMPLATE AS SHOWN TO THE USER" format="1.0.0.0" priority="0">
  <!-- BODY OF THE TEMPLATE -->
</template>
```

While it's possible to simply put plain text within the body of the template, and that plain text would show up in the final `.cs` file, `.xtemplate` files are designed to have their contents built using predefined tags such as `<function>`, `<class>`, etc... This gives the builder knowledge about the contents of the template, providing syntax highlighting in the code preview, and allowing it to enforce coding conventions.

### Types - Interface and Enum
These simple tags will create interfaces and enums respectively in the template.

|Property|Is Optional|Default|Description|
|---|---|---|---|
|id|false|-|The name of the type|
|base|true|-|The parent type of the new type|
|access|true|private|The accessibility of the type. Such as "public", "private", or "protected"|

Interfaces can also have additional interfaces that they implement. These use the `<interface-contract>` tag to indicate that the type implements an interface.

```XML
<template id="Interface" format="1.0.0.0">
  <interface id="ISomething" access="public">
    <interface-contract id="System.IDisposable" />
    <interface-contract id="System.IConvertable" />
</template>
```

```C#
public interface ISomething : System.IDisposable, System.IConvertable
{
}
```

### Types - Class and Struct
Classes and Structures act in the same was as Interfaces and Enums, but have more options available.

|Property|Is Optional|Default|Description|
|---|---|---|---|
|id|false|-|The name of the type|
|base|true|-|The parent type of the new type|
|access|true|private|The accessibility of the type. Such as "public", "private", or "protected"|
|sealed|true|false|Is the type sealed|
|partial|true|false|Is the type a partial type|
|static|true|false|Is the type static|
|abstract|true|false|Is the type abstract|


```XML
<template id="Class" format="1.0.0.0">
  <xml-comment>This is my class. Stay away!</xml-comment>
  <class id="MyClass" access="internal" base="ParentClass">
    <!-- Interfaces -->
    <interface-contract id="System.IDisposable" />
    <!-- Class Body -->
  </class>
</template>
```

```C#
/// This is my class. Stay away!
internal class MyClass : ParentClass, System.IDisposable
{
}
```

### Type Bodies
Just like how types have dedicated tags within the template, type fields and functions also have dedicated tags.

#### Members
|Property|Is Optional|Default|Description|
|---|---|---|---|
|id|false|-|The name of the member|
|type|false|-|The type of the member|
|default|true|-|An optional default value for the member|
|access|true|private|The accessibility of the type. Such as "public", "private", or "protected"|
|static|true|false|Is the member static|

```XML
<template id="Member" format="1.0.0.0">
  <class id="MyClass">
    <member id="x" type="int" default="5" />
    <member id="y" type="float" access="public" />
    <member id="z" type="string" access="public" default="five" />
  </class>
</template>
```

```C#
private class MyClass
{
  private int x = 5;
  public float y;
  public string z = "five";
}
```

#### Properties
Properties are similar to members, except they have additional attributes such as virtuality. Additionally properties can have get and set functions.

> &lt;property&gt;

|Property|Is Optional|Default|Description|
|---|---|---|---|
|id|false|-|The name of the member|
|type|false|-|The type of the member|
|default|true|-|An optional default value for the Property. Cannot be combined with `lambda`, getters, or setters|
|lambda|true|-|An optional lambda value for the property. Cannot be combined with `default`, getters, or setters|
|access|true|private|The accessibility of the type. Such as "public", "private", or "protected"|
|static|true|false|Is the member static|
|virtuality|true|none|Options: none, virtual, abstract, override|

> &lt;getter&gt; or &lt;setter&gt;

|Property|Is Optional|Default|Description|
|---|---|---|---|
|access|true|private|The accessibility of the type. Such as "public", "private", or "protected"|

```XML
<template id="Property" format="1.0.0.0">
  <class id="MyClass">
    <property id="first" type="int" access="public" />
    <property id="second" type="int" access="public">
      <getter>return 5;</getter>
    </property>
    <property id="third" type="int" virtuality="virtual">
      <getter>return first</getter>
      <setter>
first = value;
second = value * 2;
      </setter>
    </property>
    <property id="forth" type="int" default="4" />
    <property id="fifth" type="int" lambda="5" />
  </class>
</template>
```

```C#
private class MyClass
{
  public int first;
  public int second
  {
    get { return 5; }
  }
  public int third
  {
    get { return first; }
    set
    {
      first = value;
      second = value * 2;
    }
  }
  public int forth = 4;
  public int fifth => 5;
}
```

#### Functions
Functions use the `<function>` tag. You must declare the name and return type of the function using the `id` and `returnType` attributes. Then you can added one or more `<argument>` tags within the function to add arguments to the function signature. Finally you can add a body to the function by adding plaintext within the `<function>` tag. The body will automatically be indented.

|Property|Is Optional|Default|Description|
|---|---|---|---|
|id|false|-|The name of the function|
|returnType|true|void|The return type of the function|
|access|true|private|The accessibility of the type. Such as "public", "private", or "protected"|
|virtuality|true|none|Options: none, virtual, abstract, override|
|static|true|false|Is the function static|
|sealed|true|false|Is the function sealed|

|Property|Is Optional|Default|Description|
|---|---|---|---|
|id|false|-|The name of the argument|
|type|false|-|The type of the argument|

```XML
<template id="Member" format="1.0.0.0">
  <class id="MyClass">
    <function id="Add" returnType="int" access="public">
      <argument id="lhs" type="int" />
      <argument id="rhs" type="int" />
return lhs + rhs;
    </function>
  </class>
</template>
```

```C#
private class MyClass
{
  public int Add(int lhs, int rhs)
  {
    return lhs + rhs;
  }
}
```
#### Constructors
Constructors are functions that use the `<constructor>` tag. Conceptually a `constructor` is the same as a `function`, except it lacks support for `returnType`, `static`, and `sealed`.

|Property|Is Optional|Default|Description|
|---|---|---|---|
|id|true|-|The name of the function. If null, the constructor will get its name from the type it is a part of|
|access|true|private|The accessibility of the type. Such as "public", "private", or "protected"|
|virtuality|true|none|Options: none, virtual, abstract, override|
|base|true|none|Use to define a reference to base constructor|
|this|true|none|Use to define a reference to a convenience constructor|

```XML
<template id="Constructors" format="1.0.0.0">
  <class id="SubClass" base="ParentClass">
    <constructor this="5">
    </constructor>
    <br />
    
    <constructor base="x">
      <argument id="x" type="int" />
    </constructor>
  </class>
</template>
```

```C#
private class SubClass : ParentClass
{
  private SubClass()
    : this(5)
  {
  
  }
  
  private SubClass(int x)
    : base(x)
  {

  }
}
```

#### Attributes
Add attributes to your types, fields, or functions to extend their functionality using the `<attribute>` tag.

|Property|Is Optional|Default|Description|
|---|---|---|---|
|id|false|-|The name of the attribute|

```XML
<template id="Attributes" format="1.0.0.0">
  <attribute id="Obsolete">"This type is no longer supported"</attribute>
  <class id="MyClass">
    <function id="Add" returnType="int" access="public">
      <argument id="lhs" type="int" />
      <argument id="rhs" type="int" />
return lhs + rhs;
    </function>
  </class>
</template>
```
```C#
[Obsolete("This type is no longer supported")]
private class MyClass
{
  public int Add(int lhs, int rhs)
  {
    return lhs + rhs;
  }
}
```

## Namespaces
### Namespace Tag
The `<namespace>` tag can be used to encapsulate one or more other tags within a namespace. The `<namespace>` tag only has one attribute, "id", which is the name of the namespace. All children inside of the namespace will be appropriately indented.

```XML
<template id="Namespaces" format="1.0.0.0">
  <namespace id="Fright.Example">
    <class id="MyClass" />
  </namespace>
</template>
```
```C#
namespace Fright.Example
{
  private class MyClass
  {
  }
}
```

### Using Namespaces
Additionally to putting code within a namespace, you can also define which namespaces your file will use in the template. This is done with the `<using>` tag. Using namespaces can be marked as optional and the user will be able to enable/disable it in the template builder. Additionally the user will be able to add missing usings that they think they'll need.

|Property|Is Optional|Default|Description|
|---|---|---|---|
|id|false|-|The namespace to add a _using_ for|
|optional|true|false|Can this namespace be omitted|
|default|true|true|Is the default state of this _using_ set to be used|

```XML
<template id="Usings" format="1.0.0.0">
  <using id="System.Collections" />
  <using id="System.Collections.Generic" />
  <br />
  <namespace id="Fright.Example">
    <class id="MyClass" />
  </namespace>
</template>
```
```C#
using System.Collections;
using System.Collections.Generic;

namespace Fright.Example
{
  private class MyClass
  {
  }
}
```

## Build Options and Replacements
You can define options that the user can tweak in the template builder that affect how the template is built. For example this can be used to change the name of a class, variable, namespace, etc. This is done using the `<build-option>` tag. The tag must have a name, type, and replacement.

|Property|Is Optional|Default|Description|
|---|---|---|---|
|name|false|-|The name is shown to the user in the builder window|
|replacement|false|-|Anywhere {REPLACEMENT} shows up in the template will be replaced with the value of the build option|
|type|true|string|What type of build option is this. Options: int, float, string, bool|
|default|true|-|The default value for the build option|
|required|true|true|If set to true, the template cannot be built if the option's value is null or empty|

The build option replacement "filename" is special. This build option will change the name of the generated `.cs` file to match the option. It can also be used within the replacement though as seen in the following example.

```XML
<template id="BuildOptions" format="1.0.0.0">
  <build-option name="Class Name" replacement="filename" default="ClassName" />
  
  <class id="{filename}" />
</template>
```

> Where "Class Name" equals "MyClass"

```C#
private class MyClass
{
}
```

### Conditions
Boolean build options can also be used to add or omit code from the template. The `<if-build-option>` tag is used to include code only if it's build option requirements are met.

|Property|Is Optional|Default|Description|
|---|---|---|---|
|options|false|-|The replacement parts of one or more boolean build options that will be considered by this tag. Use a "!" infront of an option to indicate that it's value should be inversed.|
|operation|true|and|Determines whether the options are all required, or if any of them are required. Valid values are "and" or "or"|

```XML
<template id="Conditional" format="1.0.0.0">
  <build-option name="Has Function" replacement="hasFunction" type="bool" />
  <class id="MyClass">
    <if-build-option options="hasFunction">
      <function id="MyFunction" />
    </if-build-option>
    <if-build-option options="!hasFunction">
      //...
    </if-build-option>
  </class>
</template>
```

> Where "Has Function" equals "true"

```C#
private class MyClass
{
  private void MyFunction()
  {
  }
}
```

> Where "Has Function" equals "false"

```C#
private class MyClass
{
  //...
}
```

### Automatic Replacements
Below is a list of replacements that are automatically applied by the template builder.

|Replacement|Description|
|---|---|
|`{Random:System.Guid}`|Every instance of this replacement will be replaced with a random C# GUID|
|`{Random:System.Int}`|Every instance of this replacement will be replaced with a random C# 32 bit integer|
|`{currentYear}`|The numberical value of the current year|
|`{currentMonth}`|The numberical value of the current month|
|`{currentDay}`|The numberical value of the current day of the month|
|`{currentHour}`|The numberical value of the current hour of the day|
|`{currentMinute}`|The numberical value of the current minute of the hour|
|`{currentSecond}`|The numberical value of the current second of the minute|

## Additional Tags
### Breaks
The fully enclosed `<br />` tag can be used to add a new line anywhere in the template.

### Meta Data
The `<meta>` tag can be used to change how the builder builds your templates. Below are the supported meta data values:

|Key|Description|
|---|---|
|filenamePrefix|An optional prefix that is appended to the beginning of the file name|
|filenameSuffix|An optional suffix that is appended to the end of the file name, but before the extension|

```XML
<template id="Meta" format="1.0.0.0">
  <meta key="filenamePrefix" value="Mega" />
  <build-option name="Filename" replacement="filename" default="ClassName" />
</template>
```

If you build the above template with the filename "FalconDestroyer", then the resulting file will be "MegaFalconDestroyer.cs".

### Comments
The `<comment>` and `<xml-comment>` tags can be used to create double `//` or triple `///` comment blocks.


```XML
<template id="Member" format="1.0.0.0">
  <xml-comment>This class doesn't do anything</xml-comment>
  <class id="MyClass">
    <member id="x" type="int" default="5" />
    <member id="y" type="float" access="public" />
    <br />
    <comment>What is even the point?</comment>
    <member id="z" type="string" access="public" default="five" />
  </class>
</template>
```

```C#
/// This class doesn't do anything
private class MyClass
{
  private int x = 5;
  public float y;
  
  // What is even the point?
  public string z = "five";
}
```
