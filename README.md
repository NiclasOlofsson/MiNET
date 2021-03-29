In case you don't find the information you are looking for in the README. Do try the [wiki](https://github.com/NiclasOlofsson/MiNET/wiki)

MiNET
=====

[![Chat](https://img.shields.io/badge/chat-on%20discord-7289da.svg)](https://discord.gg/xCNrhDd) 
[![Build status](https://github.com/NiclasOlofsson/MiNET/actions/workflows/dotnetcore.yml/badge.svg)](https://github.com/NiclasOlofsson/MiNET/actions/workflows/dotnetcore.yml)
[![NuGet Version and Downloads count](https://buildstats.info/nuget/MiNET)](https://www.nuget.org/packages/MiNET) 

## What is this?

A basic Minecraft Pocket Edition (MCPE) server written in C#. Current goal is to create a server that has enterprise performance for large Minecraft multi-user gaming providers. Current servers handle 10-100 users per instance, the aim with MiNET is to deal with thousands. Priority is also to create interfaces, and to some extend, ready-made implementations of the most basic enterprise game-management features (users, worlds, etc.). With all likelyhood, MiNET will emerge into a Microsoft Cloud ready product.

As part of the project, I also deliver an up-to-date [automatically generated MCPE Protocol Specification](/src/MiNET/MiNET/Net/MCPE%20Protocol%20Documentation.md). This is a synery of that large parts of the communication code being generated using XML and T4 templates.

## Why do this?

This is actually a pet-project - a true father-son project that I do together with my son Oliver 6 years old. He is driving the requirements for this, doing much of prioritization of the order of implementation. He is a great fan of online-MCPE gaming, but still too young to play the "real deal". He also lacks the patience and understanding of laggy under-performant servers, and the consequences of that. He is also not a big fan of the kill-style game-modes around, so a sub-project of this is for him to create new kidz-friendly game-modes that we can implement in MiNET. Oliver was the one originally responsible for creating the scope of our project.

And as he really likes to tell his mother these days "Mom, don't disturb daddy. He is working for me now!". 6 years old, but I think you get the picture. I know he does.

Follow me on <a href="https://twitter.com/NiclasOlofsson" class="twitter-follow-button" data-show-count="true" data-size="large" data-dnt="true">Twitter @NiclasOlofsson</a> for news about the project, or simply track the checkins which i tend to comment heavily.
 
MiNET is running CI through the fantastic service of AppVeyor. Currently the build status of master is...    
[![Build status](https://ci.appveyor.com/api/projects/status/gb8ukrnogknic26e/branch/master)](https://ci.appveyor.com/project/NiclasOlofsson/MiNET/branch/master)

## Can I do my own plugins?

Yes you can! See the [Plugin documention in the wiki](https://github.com/NiclasOlofsson/MiNET/wiki/Plugin-API-Documentation)

**Please note that the plugin system is always going to be work in progress and the example might be a bit outdated at times.**

## Can I contribute?

Of course you can! We just need you to accept the following:

1. You will use the same coding style as the rest of the code.
2. You do not copy code from anyone or anywhere, unless you have their permissions.
3. We can always decide not to include your code, and we might make changes to it. So better ask before you do a pull request to the project.

Also, make sure to join our discord chat for easy communication.    
[![Chat](https://img.shields.io/badge/chat-on%20discord-7289da.svg)](https://discord.gg/xCNrhDd) 

## Are there forums for MiNET?

No, but you can ask questions on [stack**overflow**](http://stackoverflow.com/questions/ask?tags=minet) using the tag *MiNET*. 

## Getting started

See the [Getting Started](https://github.com/NiclasOlofsson/MiNET/wiki/Getting-Started) section on the wiki.
