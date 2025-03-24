# CreditsGameManager!
En este paquete contienes lo necesario para tener un Manager de créditos básico con el que se puede interactuar al contacto del jugador

## Archivos

En este paquete debería de encontrar lo siguiente:

 - Prefabs de un CreditsGameManager, Credits y CreditsDispensers con sus Scripts correspondientes
 - Script del CreditsGameManager, Script para mostrar los créditos, y otro para cambiar el color del texto

### ¿Cómo funciona?
Simplemente lo que hace es crear un GameObject el cual contiene dentro un TextMeshPro con un texto dado del manager. Estos textos se pueden añadir directamente desde la interfaz de Unity.

Luego dentro de los objetos de los contribuidores que crees, comprueba que la colisión contenga un tag con un nombre que hayas dado (por defecto Player), y si se da el caso dispara un evento de Unity a tu elección. En los scripts deberías de tener uno para simplemente cambiar el color del texto

#### Minitutorial

 1. Extrae el paquete de unity en tu proyecto
 2. Una vez se haya extraido, inserta los prefabs en tu area de juego correspondiente
 3. Crea el area donde vayas a crear los creditos. Es recomendable que te hagas objetos vacios y los instancies desde ahi
 4. Rellena los datos
