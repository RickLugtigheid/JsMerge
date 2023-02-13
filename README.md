# JsMerge | File Merging Tool
[![Version](https://img.shields.io/github/v/release/RickLugtigheid/JsMerge)](https://github.com/RickLugtigheid/JsMerge/releases/latest)
[![Downloads](https://img.shields.io/github/downloads/RickLugtigheid/JsMerge/total)](https://github.com/RickLugtigheid/JsMerge/releases)

## What is JsMerge?
JsMerge is a tool that allows you to merge multiple JavaScript files into one file.

The goal is to make a tool that allows the user to split their JavaScript into multiple files without the drawback of having to load multiple js files, since this will slow down your website.

## Installation
### Windows (with installer):
- Go to the [latest version](https://github.com/RickLugtigheid/JsMerge/releases/latest) and download the attached jsMerge-setup file.
- Run the jsMerge setup
- And [add the following path to the PATH environment variable](https://www.architectryan.com/2018/03/17/add-to-the-path-on-windows-10/) to use JsMerge as a command-line tool: **C:\Program Files (x86)\JsMerge**

### Windows (manually)
- Go to the [latest version](https://github.com/RickLugtigheid/JsMerge/releases/latest) and download the attached release-build.zip file.
- Place the contents in a folder for easy access, in this example we place the release in **C:\Program Files (x86)\JsMerge**.
- And [add a path to this folder to the PATH environment variable](https://www.architectryan.com/2018/03/17/add-to-the-path-on-windows-10/) to use JsMerge as a command-line tool. For this example we add **C:\Program Files (x86)\JsMerge**.

## Usage 
- Add a .jsmerge file to your project, then add your configurations to this file ([check the wiki](https://github.com/RickLugtigheid/JsMerge/wiki/.jsmerge-config) for more information)
- Run the JsMerge command to create the requested merge files (use 'JsMerge -h' or 'JsMerge --help' for more information)
- The merge files are now created in the configured output directories
