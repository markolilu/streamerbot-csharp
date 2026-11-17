# streamerbot-csharp
Code to be used in my streamer.bot for my live streams. Also existing as an ongoing project to document my learning in C# and build a portfolio.

# Streamer.bot C# Projects

C# actions for [Streamer.bot](https://streamer.bot), written in VS Code and used live on my streams. Each feature (follows, subs, commands) is built as a small pipeline: one event triggers a set of single-purpose tasks.

## What is Streamer.bot?

Streamer.bot is a free tool for automating live streams (chat commands, alerts, OBS control and more). It lets you run your own C# code through its *Execute C# Code* sub-action. This repo holds that code, version-controlled and structured like a normal software project.

## Project structure

```
src/
├─ Follows/    (follow event: OBS alert, save data, chat message, timer)
├─ Subs/       (sub event)
└─ Shared/     (helpers used by more than one feature)
```

Files and classes are named after the Streamer.bot action they belong to, e.g. `Follow_ChatMessage.cs` is the action `Follow – Chat Message`.

## Setup

### Requirements

- [Streamer.bot](https://streamer.bot) installed (Windows)
- [Streamer.bot C# Documentation](https://docs.streamer.bot/api/csharp) for context of code and interactions within Streamer.bot Actions/Sub-Actions
- [VS Code](https://code.visualstudio.com/) with the C# extension and the Streamer.bot C# extension
- [.NET SDK](https://dotnet.microsoft.com/download)

### Point the project at your Streamer.bot install

The project needs Streamer.bot's DLLs for autocomplete and error checking. Your install location is different on every machine, so it lives in a local file that is **not** committed to Git.

1. In the repo root (next to `StreamerBot.csproj`), create a file called `StreamerBot.local.props`.
2. Paste this in, replacing the path with your own Streamer.bot folder (the one containing `Streamer.bot.exe`). Use forward slashes and no quotes:

   ```xml
   <Project>
     <PropertyGroup>
       <StreamerBotPath>C:/path/to/Streamer.bot</StreamerBotPath>
     </PropertyGroup>
   </Project>
   ```

3. Open the folder in VS Code. Typing `CPH.` in a `.cs` file should now show autocomplete suggestions, and the **Problems** panel should have no errors.

`StreamerBot.local.props` is in `.gitignore`, so it won't be uploaded.

### Using the code in Streamer.bot

1. In Streamer.bot, create or open an action and add an **Execute C# Code** sub-action.
2. Copy the contents of a `.cs` file from this repo into it.
3. Click **Save and Compile**. If you see "missing assembly reference" errors, click **Find Refs**.

The `#if EXTERNAL_EDITOR` blocks in each file only apply inside VS Code, so you don't need to remove them.

## Notes

This is a learning and portfolio project. I'm building it to practise C# and to keep my stream automation maintainable.