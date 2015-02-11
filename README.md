MiNET
=====

[![Join the chat at https://gitter.im/NiclasOlofsson/MiNET](https://badges.gitter.im/Join%20Chat.svg)](https://gitter.im/NiclasOlofsson/MiNET?utm_source=badge&utm_medium=badge&utm_campaign=pr-badge&utm_content=badge) [![Build status](https://ci.appveyor.com/api/projects/status/gb8ukrnogknic26e/branch/master)](https://ci.appveyor.com/project/NiclasOlofsson/MiNET/branch/master)

## Can i test this before i download it?
Yes, you can. We have just setup a public demo server, this server is always up2date.<br>
IP: **94.23.50.5**    
Port: **19132**    
*This server will be automatically restarted when the CPU load is above 50% for security reasons.<br>

## What is this?

A basic Minecraft Pocket Edition (MCPE) server written in C#. Current goal is to create a server that has enterprise performance for large Minecraft multi-user gaming providers. Current servers handle 10-100 users per instance, the aim with MiNET is to deal with thousands. Priority is also to create interfaces, and to some extend, ready-made implementations of the most basic enterprise game-management features (users, worlds, etc.). With all likelyhood, MiNET will emerge into a Microsoft Cloud ready product.

As part of the project, I also deliver an up-to-date [automatically generated MCPE Protocol Specification](/src/MiNET/MiNET/Net/MCPE%20Protocol%20Documentation.md). This is a synery of that large parts of the communication code being generated using XML and T4 templates.

## Why do this?

This is actually a pet-project - a true father-son project that I do together with my son Oliver 6 years old. He is driving the requirements for this, doing much of prioritization of the order of implementation. He is a great fan of online-MCPE gaming, but still too young to play the "real deal". He also lacks the patience and understanding of laggy under-performant servers, and the consequences of that. He is also not a big fan of the kill-style game-modes around, so a sub-project of this is for him to create new kidz-friendly game-modes that we can implement in MiNET. Oliver was the one originally responsible for creating the scope of our project.

And as he really likes to tell his mother these days "Mom, don't disturb daddy. He is working for me now!". 6 years old, but I think you get the picture. I know he does.

Follow me on <a href="https://twitter.com/NiclasOlofsson" class="twitter-follow-button" data-show-count="false" data-size="large" data-dnt="true">Twitter @NiclasOlofsson</a> for news about the project, or simply track the checkins which i tend to comment heavily.
 
MiNET is running CI through the fantastic service of AppVeyor. Currently the build status of master is...    
[![Build status](https://ci.appveyor.com/api/projects/status/gb8ukrnogknic26e/branch/master)](https://ci.appveyor.com/project/NiclasOlofsson/MiNET/branch/master)

## Can i find any example plugins anywhere?
Yes you can! We try to post upload some example plugins <b><a href="https://github.com/MiNET-Plugins">HERE</a></b>.<Br>
Please note that the plugin system is work in progress and the example might be outdated.

## Can i contribute?

Ofcourse you can! We just need you to accept the following:

1. You will use the same coding style as the rest of the code.
2. You do not copy code from anyone or anywhere, unless you have their permissions.
3. We can always decide not to include your code, and we will make changes to it.

Also, make sure to join our Gitter chat for easy communication.    
[![Join the chat at https://gitter.im/NiclasOlofsson/MiNET](https://badges.gitter.im/Join%20Chat.svg)](https://gitter.im/NiclasOlofsson/MiNET?utm_source=badge&utm_medium=badge&utm_campaign=pr-badge&utm_content=badge)
