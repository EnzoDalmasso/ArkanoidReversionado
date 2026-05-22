# ArkanoidReversionado

Un juego clásico estilo Arkanoid desarrollado en **Unity** y programado en **C#**. El proyecto rediseña las mecánicas tradicionales del juego arcade priorizando un código limpio, modular, desacoplado y optimizado para el rendimiento utilizando físicas 2D y arquitectura basada en datos.

---

##  Características Clave & Arquitectura

###  Sistema de Ladrillos Modular (Data-Driven)
En lugar de codificar de forma rígida (*hardcoding*) la vida o los puntos de cada ladrillo, el sistema utiliza **ScriptableObjects** (`BrickData`). Esto permite crear infinitas variantes de bloques desde el editor sin tocar una sola línea de código.
* ** Feedback Visual:** Soporta transiciones de daño dinámicas mediante arrays de sprites (`damageSprites`) y una secuencia animada de destrucción mediante corrutinas (`breakSprites`).

###  Sistema Físico Anti-Atasco (Anti-Stuck)
Uno de los problemas más comunes en los clones de Arkanoid es cuando la bola rebota de forma infinitamente horizontal o vertical. Para solucionar esto, el script `Ball.cs` incluye un algoritmo en `FixedUpdate` que detecta vectores bloqueados usando un margen de error mínimo (`stuckEpsilon`). Si la bola entra en un bucle, se le aplica un leve ángulo aleatorio para restaurar el dinamismo del gameplay sin perder su velocidad actual.

###  Control de Audio en Decibeles Realistas
La gestión del sonido de los efectos (`AudioSFXSlider.cs`) no utiliza una degradación lineal simple, la cual suele percibirse mal al oído humano. El volumen del `AudioMixer` se calcula utilizando una **escala logarítmica** para convertir el valor del Slider ($0$ a $1$) en decibeles reales ($dB$):
$$\text{volumeDB} = \log_{10}(\text{value}) \times 20$$
Además, el estado del volumen se conserva de forma persistente entre sesiones mediante `PlayerPrefs`.

###  Transiciones Fluídas y Gestión de UI
* **Transiciones con LeanTween:** Uso de animaciones por código optimizadas para hacer *Fade-In*, *Fade-Out* y fundidos locales entre paneles del menú, evitando la sobrecarga del sistema de animación tradicional de Unity.
* **Arquitectura de Eventos:** El flujo de derrota se maneja mediante eventos (`EventHandler`), desacoplando por completo la lógica de la bola (`Ball`) de la UI del juego (`JuegoMenuManager`).
* **Persistencia Global:** Un `GameManager` centralizado en formato *Singleton* que no se destruye entre escenas (`DontDestroyOnLoad`) para mantener el puntaje y las vidas de manera consistente.

---

##  Estructura Principal de Scripts

* **`Player.cs`**: Control del movimiento de la pala basado en físicas (`AddForce`) e inputs directos del eje horizontal. Incluye reset posicional dinámico.
* **`Ball.cs`**: Controla el lanzamiento inicial aleatorio, detección de colisiones de zonas de muerte (*DeadZone*), paredes, ladrillos, y el sistema *anti-stuck*.
* **`Brick.cs` & `BrickData.cs`**: Componente lógico del bloque y su contenedor de datos configurable.
* **`BrickManager.cs`**: Cuenta los elementos restantes en escena y dispara el evento de victoria cuando el contenedor se queda sin hijos.
* **`GameManager.cs`**: Persistencia de datos core (Vidas globales y puntaje total).
* **`JuegoMenuManager.cs`**: Administrador de la interfaz de usuario en el nivel (pausa, controles, victoria, derrota) y control del `Time.timeScale`.
* **`MenuManager.cs` & `MenuOpciones.cs`**: Gestión de las pantallas del menú principal. `MenuOpciones` filtra de forma automática las resoluciones nativas del monitor del usuario usando su tasa de refresco actual (`refreshRateRatio`) para evitar parpadeos o errores gráficos.
* **`TransicionEscenasUI.cs`**: Controlador basado en LeanTween para efectos visuales de disolvencia en Canvas Groups.

---

##  Tecnologías y Herramientas utilizadas
* **Motor:** Unity 2D (Versión moderna con soporte para `refreshRateRatio`).
* **Lenguaje:** C# (.NET).
* **Librerías Externas:** LeanTween (para animaciones de interfaz de usuario eficientes).
* **UI:** TextMeshPro para textos de alta definición y escalabilidad.
