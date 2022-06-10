# ChatCommands
### Server Only Mod
This Mod will probably crash your Client if you try to install it.\
I am by far not the best programmer so the code might be a little spaghetti here and there. I'm trying my best to get everything as polished as I currently can though. :)
### Config
- `Prefix` [default `?`]: The prefix use for chat commands.
- `DisabledCommands` [default `Empty`]: Enter command names to disable them. Seperated by commas. Ex.: health,speed
## Chat Commands
`help [<Command>]`: Shows a list of all commands.\
`kit <Name>`: Gives you a previously specified set of items.
<details>
<summary>How does kit work?</summary>

&ensp;&ensp;You will get a new config file located in `BepInEx/config/ChatCommands/kits.json`
```json
[
  {
    "Name": "Example1",
    "PrefabGUIDs": {
      "820932258": 50, <-- 50 Gem Dust
      "2106123809": 20 <-- 20 Ghost Yarn
    }
  },
  {
    "Name": "Example2",
    "PrefabGUIDs": {
      "x1": y1,
      "x2": y2
    }
  }
]
```
&ensp;&ensp;A list of all PrefabGUIDs can be found [here](https://github.com/NopeyBoi/ChatCommands/blob/main/PrefabGUIDs%20-%209th%20June%202022.txt)!

</details>

`blood <BloodType> [<Quality>] [<Value>]`: Sets your Blood type to the specified Type, Quality and Value.\
&ensp;&ensp;**Example:** `blood Scholar 100 100`

`bloodpotion <BloodType> [<Quality>]`: Creates a Potion with specified Blood Type, Quality and Value.\
&ensp;&ensp;**Example:** `bloodpotion Scholar 100`

`waypoint <Name|Set|Remove> [<Name>] [global]`: Teleports you to previously created waypoints.\
&ensp;&ensp;**Example:** `waypoint set home` <-- Creates a local waypoint just for you.\
&ensp;&ensp;**Example:** `waypoint set arena global` <-- Creates a global waypoint for everyone (Admin-Only).\
&ensp;&ensp;**Example:** `waypoint home` <-- Teleports you to your local waypoint.\
&ensp;&ensp;**Example:** `waypoint remove home` <-- Removes your local waypoint.

`give <Item Name> [<Amount>]`: Adds the specified Item to your Inventory.\
&ensp;&ensp;**Example:** `give Stone Brick 17 `

`health <Amount>`: Sets your health to the specified amount.\
`speed <Amount|Reset>`: Sets your movement speed to the specified amount.\
`sunimmunity`: Toggles sun immunity. (*mind blown*)\
*There used to be a command that toggles the blood hunger view without the ugly effects but that needs some bug fixing. SoonTM*
## More Information
<details>
<summary>Changelog</summary>

`1.x`
- Added a reset function to speed

`1.2.0`
- Added new command: waypoint
- Added new command: kit
- Added new config: DisabledCommands
- Fixed console spam when using give

`1.1.1`
- Tiny fix

`1.1.0`
- Added new command: bloodpotion
- Reworked give command (Send items directly into the inventory now)

`1.0.0`
- Initial release

</details>
<details>
<summary>Planned Features</summary>

- Chat Permission Roles
- Kits Option: Limited Uses
- Waypoint Option: Waypoint Limit per User
- Teleport Command: Teleport to other Users
- Bring back the Blood HUD Command
- Anything fun that'll distract me from my initial goals
- Few smaller things changes and improvements

</details>