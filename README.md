MiNET
=====

## Can i test this before i download it?
Yes, you can. We have just setup a public demo server, this server is always up2date.<br>
IP: 94.23.50.5<br>
Port: 19132<Br>

## What is this?

A basic Minecraft Pocket Edition (MCPE) server written in C#. Current goal is to create a server that has enterprise performance for large Minecraft multi-user gaming providers. Current servers handle 10-100 users per instance, the aim with MiNET is to deal with thousands. Priority is also to create interfaces, and to some extend, ready-made implementations of the most basic enterprise game-management features (users, worlds, etc.). With all likelyhood, MiNET will emerge into a Microsoft Cloud ready product.

As part of the project, I also deliver an up-to-date [automatically generated MCPE Protocol Specification](/src/MiNET/MiNET/Net/MCPE%20Protocol%20Documentation.md). This is a synery of that large parts of the communication code being generated using XML and T4 templates.

## Why do this?

This is actually a pet-project - a true father-son project that I do together with my son Oliver 6 years old. He is driving the requirements for this, doing much of prioritization of the order of implementation. He is a great fan of online-MCPE gaming, but still too young to play the "real deal". He also lacks the patience and understanding of laggy under-performant servers, and the consequences of that. He is also not a big fan of the kill-style game-modes around, so a sub-project of this is for him to create new kidz-friendly game-modes that we can implement in MiNET. Oliver was the one originally responsible for creating the scope of our project.

And as he really likes to tell his mother these days "Mom, don't disturb daddy. He is working for me now!". 6 years old, but I think you get the picture. I know he does.

Follow me on <a href="https://twitter.com/NiclasOlofsson" class="twitter-follow-button" data-show-count="false" data-size="large" data-dnt="true">Twitter @NiclasOlofsson</a> for news about the project, or simply track the checkins which i tend to comment heavily.
 
MiNET is running CI through the fantastic service of AppVeyor. Currently the build status of master is...

[![Build status](https://ci.appveyor.com/api/projects/status/gb8ukrnogknic26e/branch/master)](https://ci.appveyor.com/project/NiclasOlofsson/MiNET/branch/master)
