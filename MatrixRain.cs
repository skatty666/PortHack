using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PortHackUI
{
    public class MatrixRain
    {
        private List<string> leftColumn = new List<string>();
        private List<string> rightColumn = new List<string>();
        private Random random = new Random();
        private SpriteFont font;
        private Texture2D pixelTexture;

        private string[] words = new string[]
        {
            "trobbiani", "orann", "eskalon", "faranga", "durahan", "cortez", "roy", "teatime",
            "spinty", "akasha", "remikje", "harkon", "sabrosa", "nevers", "hacknet", "racoon",
            "epsilon", "vrchat", "gmp1.7", "guerkchen", "matrix", "cyber", "neural", "data",
            "system", "access", "breach", "firewall", "encrypt", "decode", "binary", "protocol",
            "network", "server", "terminal", "console", "kernel", "daemon", "process", "thread",
            "memory", "buffer", "cache", "stack", "heap", "pointer", "array", "vector",
            "function", "method", "class", "object", "instance", "module", "package", "library",
            "compile", "execute", "runtime", "debug", "trace", "log", "error", "warning",
            "critical", "fatal", "exception", "handler", "callback", "event", "trigger", "signal",
            "interrupt", "syscall", "kernel", "driver", "hardware", "firmware", "bios", "boot",
            "loader", "init", "daemon", "service", "process", "thread", "task", "job",
            "queue", "stack", "heap", "pool", "cache", "buffer", "stream", "pipe",
            "socket", "port", "packet", "frame", "segment", "datagram", "protocol", "layer",
            "route", "gateway", "bridge", "switch", "router", "hub", "node", "host",
            "client", "server", "peer", "proxy", "tunnel", "vpn", "ssl", "tls",
            "cipher", "hash", "salt", "token", "key", "cert", "auth", "oauth",
            "session", "cookie", "header", "payload", "request", "response", "status", "code",
            "json", "xml", "yaml", "toml", "csv", "binary", "hex", "base64",
            "ascii", "unicode", "utf8", "encoding", "charset", "locale", "i18n", "l10n",
            "regex", "pattern", "match", "search", "replace", "filter", "map", "reduce",
            "lambda", "closure", "scope", "context", "state", "store", "cache", "persist",
            "database", "query", "index", "schema", "table", "column", "row", "record",
            "primary", "foreign", "unique", "constraint", "trigger", "view", "procedure", "function",
            "transaction", "commit", "rollback", "lock", "deadlock", "race", "mutex", "semaphore",
            "atomic", "volatile", "sync", "async", "await", "promise", "future", "callback",
            "event", "listener", "observer", "publisher", "subscriber", "channel", "queue", "topic",
            "message", "payload", "header", "metadata", "timestamp", "uuid", "guid", "hash",
            "checksum", "crc", "md5", "sha", "hmac", "signature", "verify", "validate",
            "sanitize", "escape", "encode", "decode", "compress", "decompress", "archive", "extract",
            "upload", "download", "stream", "buffer", "chunk", "batch", "bulk", "parallel",
            "concurrent", "distributed", "cluster", "shard", "replica", "backup", "restore", "migrate",
            "deploy", "release", "version", "tag", "branch", "merge", "rebase", "cherry-pick",
            "commit", "push", "pull", "fetch", "clone", "fork", "remote", "origin"
        };

        public void LoadContent(SpriteFont spriteFont)
        {
            font = spriteFont;
            
            // Fülle beide Spalten mit genug Wörtern
            for (int i = 0; i < 100; i++)
            {
                leftColumn.Add(words[random.Next(words.Length)]);
                rightColumn.Add(words[random.Next(words.Length)]);
            }
        }

        private float stepTimer = 0f;
        private float stepInterval = 0.15f;  // Alle 0.15 Sekunden eine neue Zeile
        
        public void Update(float deltaTime, int screenWidth, int screenHeight)
        {
            stepTimer += deltaTime;
            
            // Stepped scrolling - wie Enter-Taste drücken
            if (stepTimer >= stepInterval)
            {
                stepTimer = 0f;
                
                // Füge neue Wörter am ANFANG hinzu (oben)
                leftColumn.Insert(0, words[random.Next(words.Length)]);
                rightColumn.Insert(0, words[random.Next(words.Length)]);
                
                // Entferne alte Wörter am Ende (unten)
                if (leftColumn.Count > 40)
                {
                    leftColumn.RemoveAt(leftColumn.Count - 1);
                    rightColumn.RemoveAt(rightColumn.Count - 1);
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch, int screenWidth, int screenHeight)
        {
            Color redColor = new Color(255, 50, 50);
            Color blackTransparent = new Color(0, 0, 0, 128); // 50% transparent
            float lineHeight = 32f;  // 2x größer als vorher (war 18)
            int leftX = 20;
            int rightMargin = 20;
            int columnWidth = 200;

            // Erstelle Pixel-Texture falls nicht vorhanden
            if (pixelTexture == null)
            {
                pixelTexture = new Texture2D(spriteBatch.GraphicsDevice, 1, 1);
                pixelTexture.SetData(new[] { Color.White });
            }

            // Linker schwarzer Balken (halbtransparent)
            spriteBatch.Draw(pixelTexture, new Rectangle(0, 0, columnWidth, screenHeight), blackTransparent);

            // Rechter schwarzer Balken (halbtransparent)
            spriteBatch.Draw(pixelTexture, new Rectangle(screenWidth - columnWidth, 0, columnWidth, screenHeight), blackTransparent);

            if (font != null)
            {
                // Linke Spalte - von oben nach unten
                for (int i = 0; i < leftColumn.Count; i++)
                {
                    float y = i * lineHeight;
                    if (y >= 0 && y < screenHeight)
                    {
                        spriteBatch.DrawString(font, leftColumn[i], new Vector2(leftX, y), redColor);
                    }
                }

                // Rechte Spalte - rechtsbündig
                for (int i = 0; i < rightColumn.Count; i++)
                {
                    float y = i * lineHeight;
                    if (y >= 0 && y < screenHeight)
                    {
                        // Messe die Breite des Textes und positioniere rechtsbündig
                        var textSize = font.MeasureString(rightColumn[i]);
                        float rightX = screenWidth - textSize.X - rightMargin;
                        spriteBatch.DrawString(font, rightColumn[i], new Vector2(rightX, y), redColor);
                    }
                }
            }
            else
            {
                // Fallback: Rechtecke mit variabler Breite für besseren Effekt
                if (pixelTexture == null)
                {
                    pixelTexture = new Texture2D(spriteBatch.GraphicsDevice, 1, 1);
                    pixelTexture.SetData(new[] { Color.White });
                }

                for (int i = 0; i < leftColumn.Count; i++)
                {
                    float y = i * lineHeight;
                    if (y >= 0 && y < screenHeight)
                    {
                        int width = leftColumn[i].Length * 7;
                        float opacity = 0.5f + (i % 3) * 0.2f;
                        spriteBatch.Draw(pixelTexture, new Rectangle(leftX, (int)y, width, 14), redColor * opacity);
                    }
                }

                for (int i = 0; i < rightColumn.Count; i++)
                {
                    float y = i * lineHeight;
                    if (y >= 0 && y < screenHeight)
                    {
                        int width = rightColumn[i].Length * 7;
                        float opacity = 0.5f + (i % 3) * 0.2f;
                        int rightX = screenWidth - width - rightMargin;
                        spriteBatch.Draw(pixelTexture, new Rectangle(rightX, (int)y, width, 14), redColor * opacity);
                    }
                }
            }
        }
    }
}
