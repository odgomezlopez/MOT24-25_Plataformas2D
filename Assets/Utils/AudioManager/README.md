# AudioManager - Guía Breve

Este **AudioManager** es un sistema centralizado para manejar la reproducción de audio en Unity, separando el sonido por categorías (Background, Music, Dialogue, SFX, UI). Es una primera versión, por lo que si encuentras cualquier bug o que falta alguna funcionalidad importante comentamelo.

Este AudioManager ofrece:
- **Control centralizado y sencillo**. Se recomienda no utilizar AudioSources fuera de este Manager para un comportamiento correcto.
- **Separación de audio por categorías**: Background, Music, Dialogue, SFX y UI.
- ** Gestión de volumen con AudioMixer** y persistencia con `PlayerPrefs`

- **Audios con aletoriedad** con el Scriptable Object SmartAudioSO.
- **Diccionario de audio**. Puede utilizarse tanto como diccionario local de un personaje, como global de la escena.

---

## Puesta en marcha (rapida)
1. Arrastra el prefab AudioManager a la escena.
2. Ajustar volument en el ScriptableObject de VolumeSetting

## Puesta en marcha (detallada)

1. **Asigna el `VolumeSettings`**  
   - Crea un asset de tipo `VolumeSettings` (Assets → Create → Settings → VolumeSettings).  
   - Arrástralo al campo `VolumeSettings` del `AudioManager` en la escena.

2. **Ajusta volúmenes** en el `VolumeSettings`  
   - Cada `AudioCategory` (master, music, etc.) tiene su propio slider.
   - Los cambios se guardan en `PlayerPrefs`.
3. **Ajustar parámetros de cada canal de audio**.
    -`AudioType`. Si la canal tiene una sola fuente o puede tener múltiples.
    -`AudioMode`. Si el sonido es 2D o 3D.
    -Loop. Si se quiere que el sonido se reproduzca en bucle.
    -El nivel máximo de decibelios del canal.

## Ejemplos de Uso

### 1. Reproducir SFX 3D en una posición (estático, no requiere que el AudioManager esté instanciado en la escena)

Crea un objeto temporal con un `AudioSource`, ideal para explosiones, pasos, etc.

~~~~csharp
AudioManager.PlaySoundAtPoint(mySFXClip, transform.position, volume: 1f, pitch: 1f);
~~~~

### 2. Reproducir un clip directamente

Las principales categorias son: AudioCategory.Background, AudioCategory.Music, AudioCategory.Dialogue, AudioCategory.SFX, AudioCategory.UI. Por ejemplo, para reproducir una música de fondo:

~~~~csharp
AudioManager.Instance.Play(AudioCategory.Music, myAudioClip);
~~~~

Si se quiere que tenga FadeIn sería:

~~~~csharp
AudioManager.Instance.Play(AudioCategory.Music, myAudioClip, fadeTime: 5f);
~~~~

Y si ya hay otra música y se quiere hacer un crossfade basta con:

~~~~csharp
AudioManager.Instance.Play(AudioCategory.Music, myAudioClip, fadeOutTime: 5f, fadeInTime: 5f);
~~~~

Este método devuelve por parámetro el `AudioSource` en el que está sonando el audio. Pudiendo guardarla como `playinAtSource` y modificarl si se desea. 

Adicionalmente también hay método de `Pause(audioCategory)`, `Resume(audioCategory)` y `Stop(audioCategory)` por categoria. O si se quieren parar todas las fuentes se cuenta con los métodos `PauseAllAudio()`, `ResumeAllAudio()`, `StopAllAudio()`

### 3. Reproducir un `AudioClipSO`

Si se quiere aleatorizar un sonido (entre varios clips, el volumen o el pitch) se puede hacer utilizando el ScriptableObject `AudioClipSO`.

Cada sonido debe indicar a que categoria pertenece. Así, para hacer reproducir este audio basta con sus valores aleatorizados basta con:

~~~~csharp
myClipSO.Play();
~~~~

Puedes crear un recurso `AudioClipSO` desde: `Audio SO/Audio Clip`

PENDIENTE. Metodo de pausar un audio desde el mismo `AudioClipSO`

### 4. Reproducir `AudioClipSO` por clave usando el diccionario global

Este sistema permite trabajar con diccionarios de `AudioClipSO`, guardados como ScriptableObjects.

Si son sonidos globales del juego, estos diccionariospueden ser añadidos al Singleton `GlobalAudioDicts` y usados de forma simple:

~~~~csharp
GlobalAudioDicts.Play("npcLine01");
~~~~

Puedes crear un recurso `AudioDictSO` desde: `Audio SO/Audio Dict`

## Ejemplos Interesantes y Casos de Uso Avanzados

### 5. AudioDictionary local para un enemigo o personaje

Cada enemigo/objeto puede tener su propio conjunto de sonidos sin mezclar claves en un diccionario global.

~~~~csharp
public class EnemyStats : MonoBehaviour
{
    [SerializeField] private AudioDictionary localAudioDict;

    public void PlayAttackSound()
    {
        localAudioDict.Play("Attack");
    }

    public void PlayDeathSound()
    {
        localAudioDict.Play("Death");
    }
}
~~~~

Basta con asignar a cada prefab (enemigo, jefe, etc.) su diccionario personalizado y llamar al método Play con la clave del `AudioClipSO` correspondiente.

### 6. AudioDictionary para un jugador con pisadas

Puedes tener “Footstep_Grass”, “Footstep_Metal”, etc.

~~~~csharp
void PlayFootstep()
{
    string surfaceKey = GetSurfaceKey(); // Determinada por la superficie actual
    walkingSurfaceDict.Play(surfaceKey);
}
~~~~

## Mejoras pendientes
- Revisar la pausa de sonidos multi-source.