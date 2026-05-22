# ArkanoidReversionado

A classic Arkanoid-style game developed in **Unity** and programmed in **C#**. The project redesigns the traditional mechanics of the arcade game, prioritizing clean, modular, decoupled, and performance-optimized code using 2D physics and a data-driven architecture.

---

##  Key Features & Architecture

### Modular Data-Driven Brick System
Instead of hardcoding the health or score value of each brick, the system utilizes **ScriptableObjects** (`BrickData`). This allows for the creation of endless block variants directly from the editor without touching a single line of code.
* **Visual Feedback:** Supports dynamic damage transitions via sprite arrays (`damageSprites`) and a destruction animation sequence handled through coroutines (`breakSprites`).

### Anti-Stuck Physics System
One of the most common issues in Arkanoid clones occurs when the ball gets trapped bouncing in an infinite horizontal or vertical loop. To solve this, the `Ball.cs` script includes an algorithm within `FixedUpdate` that detects locked vectors using a minimal error margin (`stuckEpsilon`). If the ball enters a loop, a slight random angle deflection is applied to restore gameplay dynamism without losing its current velocity.

### Realistic Decibel Audio Control
Sound effects volume management (`AudioSFXSlider.cs`) avoids simple linear degradation, which usually sounds unnatural to the human ear. The `AudioMixer` volume is calculated using a **logarithmic scale** to convert the Slider value (0 to 1) into real decibels (dB):

```csharp
float volumeDB = Mathf.Log10(value) * 20;
