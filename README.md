# Lyralei
ServerQuery bot for TS3

Lyralei is intended to be a bot that connects to multiple teamspeak 3 servers, somewhat resemblant of IRC bots connecting to multiple channels. The name Lyralei is not final.

Lyralei is meant to be handling many things, including but not limited to:
* Automatic channel generation / cleaning
* Clan channel management (hiding and re-appearing when members go online for example)
* User class promotions

# What you need
* MSSQL Express (or other) to run local databases.
* Visual Studio 2015 Community Edition or other recent version.
* .NET Framework 4.6.1
* TeamSpeak 3 Server to mess with locally (here's one with password prepared: https://dl.dropboxusercontent.com/u/3782895/TeamSpeak%203%20Server.rar)

# Knowledge areas of this project
* TeamSpeak ServerQuery logic
* TS3QueryLib knowledge
* Entity Framework 7
* Addon handling in C#
* Command parsing in C#
* Permission handlling in C#
* HttpWebRequest crawling

# Getting started
1. Clone this repo
2. Clone my other (forked) repo: https://github.com/qvazzler/TS3QueryLib.Net
3. If needed, you'll have to build TS3QueryLib.NET and reference it properly in this repo (Lyralei).
4. Contact me if needed

# Goals of this project
* Have a functioning bot with the bare minimums, running on windrunner.cc teamspeak 3 server
* Be able to run the bot on multiple teamspeak 3 servers
* Host a real website for visual representation of the project
* Have a working addon system
