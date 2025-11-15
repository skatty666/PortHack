using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Hacknet.Effects
{
    public static class Cube3DDebug
    {
        private static BasicEffect basicEffect;
        private static GraphicsDevice graphicsDevice;
        
        // WÜRFEL VERTICES (gleichmäßig in alle Richtungen)
        private static Vector3[] cubeVertices = new Vector3[]
        {
            new Vector3(-1, -1, -1),  // 0
            new Vector3(1, -1, -1),   // 1
            new Vector3(1, 1, -1),    // 2
            new Vector3(-1, 1, -1),   // 3
            new Vector3(-1, -1, 1),   // 4
            new Vector3(1, -1, 1),    // 5
            new Vector3(1, 1, 1),     // 6
            new Vector3(-1, 1, 1)     // 7
        };
        
        private static int[] cubeIndices = new int[]
        {
            0, 1, 1, 2, 2, 3, 3, 0,
            4, 5, 5, 6, 6, 7, 7, 4,
            0, 4, 1, 5, 2, 6, 3, 7
        };
        
        // ALLE 6 DIAGONALEN - KORRIGIERT!
        private static int[] diagonalIndices = new int[]
        {
            1, 3,  // Vordere Fläche: rechts-unten → links-oben
            5, 7,  // Hintere Fläche: rechts-unten → links-oben
            0, 7,  // Linke Fläche
            1, 6,  // Rechte Fläche
            3, 6,  // Obere Fläche
            0, 5   // Untere Fläche
        };

        public static void Initialize(GraphicsDevice device)
        {
            graphicsDevice = device;
            basicEffect = new BasicEffect(device)
            {
                VertexColorEnabled = true,
                Projection = Matrix.CreatePerspectiveFieldOfView(
                    MathHelper.ToRadians(45),
                    device.Viewport.AspectRatio,
                    0.1f,
                    100f)
            };
            
            UpdateCamera(2f, 18f, 16f, 5f, 35f, 0f);
        }

        public static void UpdateCamera(float rechts, float oben, float distanz, float pan, float tilt, float roll)
        {
            if (basicEffect == null) return;

            // Sphärische Koordinaten für 360° Rotation um das Objekt
            float panRad = MathHelper.ToRadians(pan);
            float tiltRad = MathHelper.ToRadians(tilt);
            
            // Berechne Kamera-Position in sphärischen Koordinaten
            float x = distanz * (float)Math.Cos(tiltRad) * (float)Math.Sin(panRad);
            float y = distanz * (float)Math.Sin(tiltRad);
            float z = distanz * (float)Math.Cos(tiltRad) * (float)Math.Cos(panRad);
            
            // Kamera rotiert um den Würfel bei (0,0,0)
            Vector3 target = Vector3.Zero;
            Vector3 cameraPosition = new Vector3(x, y, z);
            
            Vector3 up = Vector3.Up;
            
            // Roll-Rotation für die Up-Richtung
            if (roll != 0f)
            {
                Matrix rollMatrix = Matrix.CreateRotationZ(MathHelper.ToRadians(roll));
                up = Vector3.Transform(up, rollMatrix);
            }
            
            basicEffect.View = Matrix.CreateLookAt(cameraPosition, target, up);
        }

        public static void RenderCubeWithDiagonals(Vector3 position, float scale, Vector3 rotation, Color color)
        {
            if (basicEffect == null) return;

            Matrix world = Matrix.CreateScale(scale) *
                          Matrix.CreateRotationX(rotation.X) *
                          Matrix.CreateRotationY(rotation.Y) *
                          Matrix.CreateRotationZ(rotation.Z) *
                          Matrix.CreateTranslation(position);

            basicEffect.World = world;

            VertexPositionColor[] vertices = new VertexPositionColor[cubeVertices.Length];
            for (int i = 0; i < cubeVertices.Length; i++)
            {
                vertices[i] = new VertexPositionColor(cubeVertices[i], color);
            }

            foreach (EffectPass pass in basicEffect.CurrentTechnique.Passes)
            {
                pass.Apply();
                
                graphicsDevice.DrawUserIndexedPrimitives(
                    PrimitiveType.LineList,
                    vertices,
                    0,
                    vertices.Length,
                    cubeIndices,
                    0,
                    cubeIndices.Length / 2
                );
                
                graphicsDevice.DrawUserIndexedPrimitives(
                    PrimitiveType.LineList,
                    vertices,
                    0,
                    vertices.Length,
                    diagonalIndices,
                    0,
                    diagonalIndices.Length / 2
                );
            }
        }
    }
}