# PortHack

It's the PortHack animation from the Game "Hacknet", one of my favorite games. Shoutout to Matt Trobbiani (@Orann) for handing the source code out to me. I've been working on this for way too long, so if something is not working, you're on your own. I'm not going to help, sorry.
There are debugging .cs for exploring Camera positions and the like.

This is literally just to look pretty on your Desktop, nothing else.
Oh yeah and this wasn't even made by me, the programming was made by Kiro (really cool AI), I just did a lot of research to just make it pop as good as it does...And that was tough, trust me.

## Features

- 3D wireframe cube animation with diagonal struts
- Smooth looping animation sequence
- Matrix-style scrolling text columns
- Terminal-inspired UI with status bars
- Customizable camera angles and perspectives

## Requirements

- .NET 9.0 SDK or later
- MonoGame Framework
- Linux (tested on Arch Linux)
- A monospace font (FreeMono, DejaVu Sans Mono, or similar)

## Installation

1. Clone the repository:
```bash
git clone https://github.com/skatty666/PortHack.git
cd PortHack/PortHackUI
```

2. Install .NET SDK if not already installed:
```bash
# Arch Linux
sudo pacman -S dotnet-sdk

# Ubuntu/Debian
sudo apt install dotnet-sdk-9.0
```

3. Install MonoGame Content Pipeline Tool:
```bash
dotnet tool install -g dotnet-mgcb
```

4. Build the project:
```bash
dotnet build
```

5. Run the application:
```bash
dotnet run
```

## Setup as Command

To run PortHack from anywhere in your terminal:

1. Copy the launch script:
```bash
mkdir -p ~/.local/bin
cp porthack.sh ~/.local/bin/PortHack
chmod +x ~/.local/bin/PortHack
```

2. Add to PATH (if not already):
```bash
echo 'export PATH="$HOME/.local/bin:$PATH"' >> ~/.bashrc
source ~/.bashrc
```

3. Run from anywhere:
```bash
PortHack
```

## Autostart on Boot

To start PortHack automatically when your system boots:

```bash
mkdir -p ~/.config/autostart
cat > ~/.config/autostart/porthack.desktop << 'EOF'
[Desktop Entry]
Type=Application
Name=PortHack
Exec=/home/$USER/.local/bin/PortHack
Hidden=false
NoDisplay=false
X-GNOME-Autostart-enabled=true
Comment=PortHack Cube Animation
EOF
```

## Customization

### Fonts
Edit `Content/Font.spritefont` to change the Matrix text font:
```xml
<FontName>FreeMono</FontName>
<Size>24</Size>
<Style>Bold</Style>
```

### Colors
Modify colors in the source files:
- Matrix text: `MatrixRain.cs` (default: red)
- UI bars: `TerminalUI.cs` (default: red bars, white text)

### Animation Speed
Adjust timing in `PortHackCubeSequence.cs`:
- `startupTotal`: Initial cube appearance
- `spinupTotal`: Cube spawn duration
- `runtimeTotal`: Main animation duration

### Camera Position
Modify camera settings in `Cube3D.cs`:
```csharp
UpdateCamera(0f, 0f, 27.2f, 135f, 35.4f, 0f);
// Parameters: right, up, distance, pan, tilt, roll
```

## Project Structure

```
PortHackUI/
├── Cube3D.cs              # 3D cube rendering with diagonals
├── PortHackCubeSequence.cs # Animation sequence logic
├── MatrixRain.cs          # Scrolling text effect
├── TerminalUI.cs          # Status bars and UI elements
├── Game1.cs               # Main game loop
├── Content/
│   ├── Font.spritefont    # Matrix text font (24pt)
│   └── UIFont.spritefont  # UI text font (12pt)
└── porthack.sh            # Launch script
```

## Dependencies

- MonoGame.Framework.DesktopGL (3.8.1 or later)
- .NET 9.0 Runtime

## License

MIT License - Feel free to use and modify!

## Credits

Created by Matt Trobbiani (@Orann), Kiro(AI), and me, Skatty666
Inspired by Hacknet and The Matrix

## Troubleshooting

### Font not loading
If you see rectangles instead of text, rebuild the fonts:
```bash
cd Content
dotnet mgcb -@:Content.mgcb
cp bin/DesktopGL/*.xnb ../bin/Debug/net9.0/Content/
```

### Command not found
Make sure `~/.local/bin` is in your PATH:
```bash
echo $PATH | grep ".local/bin"
```

### Window size issues
Adjust window size in `Game1.cs`:
```csharp
_graphics.PreferredBackBufferWidth = 780;
_graphics.PreferredBackBufferHeight = 450;
```
