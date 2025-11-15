using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
 
namespace Hacknet.Effects
{
    public class PortHackCubeSequence
    {
        private float rotTime = 0f;
        private float elapsedTime = 0f;
 
        float startup = 0.3f;
        float spinup = 0f;
        float pause = 0f;
        float runtime = 0f;
        float vortexClose = 0f;
        float fadeout = 0f;
        float spindown = 0f;
        float idle = 0f;
 
        public bool HeartFadeSequenceComplete = false;
        public bool ShouldCentralSpinInfinitley = false;
        
        public void Reset()
        {
            spinup = startup = pause = runtime = vortexClose = fadeout = idle = spindown = rotTime = elapsedTime = 0f;
            HeartFadeSequenceComplete = false;
        }
 
        public void DrawSequence(Rectangle dest, float t, float totalTime) 
        {
            float startupTotal = 0.6f;
            float spinupTotal = 2.0f;
            float pauseTotal = 0.5f;
            float runtimeTotal = (ShouldCentralSpinInfinitley) ? float.MaxValue : 6.0f;
            float spindownTotal = float.MaxValue;
            float idleTotal = float.MaxValue;
 
            elapsedTime += t;
            float timeShift = elapsedTime;
            timeShift = Math.Max(0f, timeShift);
 
            float spindownProgress = 0f;
 
            float spinSpeed = 1f;
            float scaleMult = 1f;
            float seperationMult = 0f;
            
            if (startup < startupTotal) 
            {
                startup += t;
                spinSpeed = 0f;
                scaleMult = Utils.QuadraticOutCurve(Utils.QuadraticOutCurve(startup / startupTotal));
            }
            else if (spinup < spinupTotal) 
            {
                spinup = Math.Min(spinupTotal, spinup + t);
                spinSpeed = (spinup / spinupTotal);
            }
            else if (pause < pauseTotal)
            {
                pause += t;
                spinSpeed = 1f;
            }
            else if (runtime < runtimeTotal) 
            {
                runtime = Math.Min(runtimeTotal, runtime + t);
                rotTime += t;
                
                float runtimeProgress = runtime / runtimeTotal;
                seperationMult = Math.Min(1f, runtimeProgress * 2f);
            }
            else if (vortexClose < 2.0f)
            {
                // Vortex zieht sich zurück, alle Würfel bleiben sichtbar und drehen weiter
                vortexClose += t;
                float closeProgress = vortexClose / 2.0f;
                
                spinSpeed = 1f;  // Alle Würfel bleiben sichtbar
                scaleMult = 1f;  // Keine Größenänderung
                seperationMult = 1f - closeProgress;  // Vortex schließt sich: 1 → 0
                rotTime += t * (1f - closeProgress * 0.5f);  // Leicht langsamer werden
            }
            else if (fadeout < 2.0f)
            {
                // Würfel verschwinden von außen nach innen
                fadeout += t;
                float fadeProgress = fadeout / 2.0f;
                
                spinSpeed = 1f - fadeProgress;  // Von 1.0 zu 0.0 (außen nach innen)
                scaleMult = 1f;  // Keine Größenänderung
                seperationMult = 0f;  // Vortex ist geschlossen
                rotTime += t * 0.3f;  // Langsam weiterdrehen
                
                // Wenn Fadeout fertig → Reset für Loop
                if (fadeout >= 2.0f)
                {
                    Reset();
                }
            }
            else if (spindown < spindownTotal) 
            {
                spindown += t;
                spindownProgress = (Math.Min(1f, spindown) / 2.3f) / 1f;
                spinSpeed = 0.1f + (0.9f * ((1f - (spindownProgress))));
                if (spindownProgress >= 0.40f) spinSpeed = 0.2f;
                rotTime += t * spinSpeed;
                
                seperationMult = 1f - spindownProgress;
                scaleMult = 0.3f + (0.7f * Utils.QuadraticOutCurve(1f - spindownProgress));
            }
            else if (idle < idleTotal) 
            {
                idle += t;
                spinSpeed = 0.1f;
                rotTime += t * (spinSpeed * 2f);
                seperationMult = 0f;
                scaleMult = 0.3f;
            }
            else 
            {
                Reset();
            }
 
            int total = 40;
            for (int i = 0; i < total; i++) 
            {
                float percentThrough = (((float)i / (float)total));
                
                // Spiral-Modifier nur wenn Rotation aktiv ist
                float interiorSpinModifier = 1f;
                if (spinSpeed > 0f)
                {
                    interiorSpinModifier = 1f + (0.05f * spinSpeed * (((float)(total - i) / (float)total) * 10f));
                }

                // VON INNEN NACH AUSSEN GESTAFFELTER START!
                float startDelay = ((float)i * 0.08f);
                float lTimeShift = rotTime - startDelay;
                
                // Cube dreht nur wenn seine Zeit gekommen ist
                if (lTimeShift < 0f)
                {
                    lTimeShift = 0f;  // Wartet noch
                }
                else
                {
                    // Vortex-Separation während Runtime
                    float vortexOffset = (((float)i * 0.05f) * seperationMult);
                    lTimeShift = (lTimeShift * interiorSpinModifier) - vortexOffset;
                    lTimeShift = Math.Max(0f, lTimeShift);
                }
                
                float rotation = lTimeShift * 1.5f;
 
                float ittscaleMultQuadritizer = scaleMult;
                for (int m = 0; m < i; m++) 
                    ittscaleMultQuadritizer *= scaleMult;
                
                float ittScaleMult = scaleMult * ((float)(total - i) / (float)total);
 
                bool draw = (percentThrough <= spinSpeed);
                if (draw) 
                {
                    Cube3D.RenderWireframe(
                        new Vector3(0f, 0f, 0f), 
                        2.6f + (((float)i / 4f) * ittscaleMultQuadritizer), 
                        new Vector3(
                            MathHelper.ToRadians(0f),  // Keine X-Rotation
                            rotation,                   // Nur Y-Rotation für Animation
                            MathHelper.ToRadians(0f)   // Keine Z-Rotation
                        ), 
                        Color.White
                    );
                }
            }
        }
 
        public void DrawHeartSequence(Rectangle dest, float t, float totalTime)
        {
            // ... (bleibt gleich, kann ich auch fixen wenn du willst)
            float startupTotal = 3f;
            float spinupTotal = 10f;
            float runtimeTotal = 2f;
            float spindownTotal = 9f;
            float idleTotal = float.MaxValue;
            float timeInIdleBeforeFades = 2f;
            float totalFadeoutTime = 21f;
 
            elapsedTime += t;
            float timeShift = elapsedTime;
            timeShift = Math.Max(0f, timeShift);
 
            float spindownProgress = 0f;
 
            float spinSpeed = 1f;
            float scaleMult = 1f;
            float seperationMult = 0f;
            float fadeOutCompletePercent = 0f;
            
            if (startup < startupTotal) 
            {
                startup += t;
                scaleMult = Utils.QuadraticOutCurve(Utils.QuadraticOutCurve(startup / 3f));
            }
            else if (spinup < spinupTotal) 
            {
                spinup = Math.Min(10f, spinup + t);
                spinSpeed = (spinup / 10f);
                rotTime += t * spinSpeed;
            }
            else if (runtime < runtimeTotal) 
            {
                runtime = Math.Min(2f, runtime + t);
                rotTime += t;
                
                float runtimeProgress = runtime / runtimeTotal;
                seperationMult = Math.Min(1f, runtimeProgress * 2f);
            }
            else if (spindown < spindownTotal) 
            {
                spindown += t;
                spindownProgress = spindown / 9f;
                spinSpeed = 0.1f + (0.9f * ((1f - spindownProgress)));
                rotTime += t * spinSpeed;
 
                seperationMult = 1f - spindownProgress;
                scaleMult = 0.3f + (0.7f * Utils.QuadraticOutCurve(1f - spindownProgress));
            }
            else if (idle < idleTotal) 
            {
                idle += t;
                spinSpeed = 0.1f;
                rotTime += t * spinSpeed;
                seperationMult = 0f;
                scaleMult = 0.3f;
 
                if (idle > timeInIdleBeforeFades) 
                {
                    fadeOutCompletePercent = Utils.QuadraticOutCurve(Math.Min(1f, (idle - timeInIdleBeforeFades) / totalFadeoutTime));
                    HeartFadeSequenceComplete = (fadeOutCompletePercent >= 1f);
                }
            }
            else 
            {
                spinup = startup = runtime = idle = spindown = rotTime = 0f;
            }
 
            int total = 40;
            for (int i = 0; i < total; i++) 
            {
                float percentThrough = (((float)i / (float)total));
                
                float interiorSpinModifier = 1f;
                if (spinSpeed > 0f)
                {
                    interiorSpinModifier = 1f + (0.05f * spinSpeed * (((float)(total - i) / (float)total) * 10f));
                }

                float startDelay = ((float)i * 0.08f);
                float lTimeShift = rotTime - startDelay;
                
                if (lTimeShift < 0f)
                {
                    lTimeShift = 0f;
                }
                else
                {
                    float vortexOffset = (((float)i * 0.05f) * seperationMult);
                    lTimeShift = (lTimeShift * interiorSpinModifier) - vortexOffset;
                    lTimeShift = Math.Max(0f, lTimeShift);
                }
                
                float rotation = lTimeShift * 1.5f;
 
                float ittscaleMultQuadritizer = scaleMult;
                for (int m = 0; m < i; m++) 
                    ittscaleMultQuadritizer *= scaleMult;
                
                float ittScaleMult = scaleMult * ((float)(total - i) / (float)total);
 
                bool draw = (percentThrough <= spinSpeed);
 
                float opacity = 1f;
                Color drawColor = Color.White;
                
                if (fadeOutCompletePercent > 0f) 
                {
                    float segmentPerItem = 1f / (float)(total);
 
                    if (((float)i) * segmentPerItem < fadeOutCompletePercent) 
                    {
                        float mySegmentMax = (float)(i + 1) * segmentPerItem;
                        if (fadeOutCompletePercent > mySegmentMax) 
                        {
                            opacity = 0f;
                            drawColor = Color.Transparent;
                        } 
                        else 
                        {
                            opacity = 1f - (fadeOutCompletePercent % segmentPerItem) / segmentPerItem;
                            drawColor = Color.Lerp(Utils.AddativeRed, Color.Red, opacity) * opacity;
                        }
                    }
                }
 
                if (draw) 
                {
                    Cube3D.RenderWireframe(
                        new Vector3(0f, 0f, 0f), 
                        2.6f + (((float)i / 4f) * ittscaleMultQuadritizer), 
                        new Vector3(
                            MathHelper.ToRadians(0f),  // Keine X-Rotation
                            rotation,                   // Nur Y-Rotation für Animation
                            MathHelper.ToRadians(0f)   // Keine Z-Rotation
                        ), 
                        drawColor
                    );
                }
            }
        }
    }
}