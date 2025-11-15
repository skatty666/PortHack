using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PortHackUI
{
    public class TerminalUI
    {
        private SpriteFont font;
        private Texture2D pixel;
        private Random random = new Random();
        private float commandTimer = 0f;
        private string currentCommand = "";
        
        private string[] commands = new string[]
        {
            "Hacking Port 22...",
            "Scanning Network...",
            "Bypassing Firewall...",
            "Decrypting Data...",
            "Injecting Payload...",
            "Establishing Connection...",
            "Brute Force Attack...",
            "SQL Injection Active...",
            "Exploiting Vulnerability...",
            "Accessing Root Directory...",
            "Cracking Password Hash...",
            "Spoofing MAC Address...",
            "Tunneling Through VPN...",
            "Intercepting Packets...",
            "Executing Remote Code...",
            "Privilege Escalation...",
            "Backdoor Installed...",
            "Keylogger Active...",
            "Dumping Memory...",
            "Reverse Shell Connected..."
        };

        public void LoadContent(SpriteFont spriteFont, GraphicsDevice device)
        {
            font = spriteFont;
            
            pixel = new Texture2D(device, 1, 1);
            pixel.SetData(new[] { Color.White });
            
            currentCommand = commands[random.Next(commands.Length)];
        }

        public void Update(float deltaTime)
        {
            commandTimer += deltaTime;
            
            // Wechsle Command alle 2 Sekunden
            if (commandTimer > 2.0f)
            {
                commandTimer = 0f;
                currentCommand = commands[random.Next(commands.Length)];
            }
        }

        public void Draw(SpriteBatch spriteBatch, int screenWidth, int screenHeight)
        {
            if (font == null || pixel == null) return;

            Color redColor = new Color(200, 0, 0);
            Color whiteColor = Color.White;
            int barHeight = 25;

            // Top Bar
            spriteBatch.Draw(pixel, new Rectangle(0, 0, screenWidth, barHeight), redColor);
            spriteBatch.DrawString(font, "app: PortHack", new Vector2(10, 5), whiteColor);
            
            string ipText = "IP: 127.0.0.1";
            var ipSize = font.MeasureString(ipText);
            spriteBatch.DrawString(font, ipText, new Vector2(screenWidth - ipSize.X - 10, 5), whiteColor);

            // Bottom Bar (Taskleiste)
            spriteBatch.Draw(pixel, new Rectangle(0, screenHeight - barHeight, screenWidth, barHeight), redColor);
            
            // Command in der Mitte
            var commandSize = font.MeasureString(currentCommand);
            float commandX = (screenWidth - commandSize.X) / 2;
            spriteBatch.DrawString(font, currentCommand, new Vector2(commandX, screenHeight - barHeight + 5), whiteColor);
        }

        public int GetTopBarHeight() => 25;
        public int GetBottomBarHeight() => 25;
    }
}
