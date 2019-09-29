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
