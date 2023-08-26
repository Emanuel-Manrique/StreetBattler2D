# StreetBattler2D
Experience the thrill of a 2D fighting game, paying homage to the iconic Street Fighter series. Engage in intense combat between a user-controlled player and a challenging enemy AI. Unleash your combat skills and master the game controls!

## Architecture 🏛

The architecture of the game leverages the **object-oriented programming** paradigm offered by C#. It's neatly divided into modular classes to encapsulate functionalities:

1. **`GameLogic`**: The backbone of the game, orchestrating core mechanics such as movements, collision detections, and handling user actions like punches and fireballs.
2. **`Player`**: Dedicated to the player's character, this class manages animations, actions, health, and movements.
3. **`Enemy`**: Controls the enemy AI, its animations, actions, and health. The AI has a simple behavior: it detects proximity to the player and reacts by attacking.

The primary interface, **`Form1`**, built on .NET's **Windows Forms**, is responsible for rendering the game's graphics, capturing user inputs, and coordinating game events.

## Directory Structure 📂

- **Images**: Inside Resources.
- **Sounds**: Inside Resources.
- **Classes**:
  - `GameLogic.cs`: Houses the game's engine.
  - `Player.cs`: Manages player dynamics.
  - `Enemy.cs`: Controls enemy behaviors.
  - `Form1.cs`: The game's GUI and main event loop.

## Mechanics & Features ⚙️

### Game Loop

Utilizing **Windows Forms Timer**, the game refreshes via a consistent game loop (`gameTimer`), which ticks every 20 milliseconds, invoking the `GameTimerEvent`. This mechanism ensures smooth graphics rendering, character position updates, collision checks, and more.

### Rendering

The game's visual elements are drawn using the `Graphics` class in .NET's **Windows Forms** during the `Paint` event. The `FormPaintEvent` method is tasked with layering the game visuals such as backgrounds, characters, and action animations.

### Controls

Harness the power of keyboard inputs to command the player:

- **Arrow Left**: Navigate left.
- **Arrow Right**: Navigate right.
- **Z**: Unleash a punch.
- **X**: Deliver a stronger punch.
- **C**: Shoot a fireball.
- **M**: Change the background.

### Enemy AI

Crafted with logic, the enemy character uses a rudimentary AI. It consistently evaluates its distance from the player, ensuring it remains within attack range. Furthermore, the AI smartly ensures the enemy doesn't wander off the screen.

## Credits 📜
Credit to MOO ICT for his marvelous tutorial that helped me a lot.
Credit to Chips 'N Cellos for the Music (Balrog's Theme (Street Fighter II: Champion Edition) - Mega Man Style 8-Bit Remix).
