# Fright: Unity C# Templates
A simple tool for creating C# files in Unity using extensible templates

## Purpose
The purpose of this tool is to make creation of new Unity scripts easier by providing templates. These templates fill out boilerplate code such as usings, namespaces, and common functions. Additionally the template builder normalizes files for line endings.

## Usage
1) Download the `.unitypackage` release file and import it into your project
2) Select a folder in the Unity Project view, right click, and select "Template Window"
3) Select a template from the template picker. For example "Class", "Struct", "MonoBehaviour", etc...
4) Fill in any options for that template such as it's name
5) Press "Create" to create that file in the folder you have selected

## Creating Your Own Templates
The template builder window will automatically pick up any `.xtemplate` file in the project. `.xtemplate` files are XML files that have a specific structure that the builder window can understand. Every template must contain exactly one `template` tag which defines the name of the template, what `.xtemplate` version it is, and an optional sorting priority. The sorting priority is used to sort the templates in the template picker, in ascending order.

```XML
<template id="NAME OF YOUR TEMPLATE AS SHOWN TO THE USER" version="1.0.0.0" priority="0">
  <!-- BODY OF THE TEMPLATE -->
</template>
```

While it's possible to simply put plain text within the body of the template, and that plain text would show up in the final `.cs` file, `.xtemplate` files are designed to have their contents built using predefined tags such as `<function>`, `<class>`, etc... This gives the builder knowledge about the contents of the template and allows it to force coding convention.

### Types - Interface and Enum
These simple tags will create interfaces and enums respectively in the template.

|Property|Is Optional|Default|Description|
|---|---|---|---|
|id|false|-|The name of the type|
|base|true|-|The parent type of the new type|
|comment|true|-|An XML comment for the type|
|access|true|private|The accessibility of the type. Such as "public", "private", or "protected"|

Interfaces can also have additional interfaces that they implement. These use the `<interface-contract>` tag to indicate that the type implements an interface.

```XML
<template id="Interface" version="1.0.0.0">
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
|comment|true|-|An XML comment for the type|
|isSealed|true|false|Is the type sealed|
|isPartial|true|false|Is the type a partial type|
|isStatic|true|false|Is the type static|
|isAbstract|true|false|Is the type abstract|


```XML
<template id="Class" version="1.0.0.0">
  <class id="MyClass" access="internal" base="ParentClass" comment="This is my class. Stay away!">
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
