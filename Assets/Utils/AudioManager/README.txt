# AudioManager - Guía Breve

Este **AudioManager** es un sistema centralizado para manejar la reproducción de audio en Unity, separando el sonido por categorías (Background, Music, Dialogue, SFX). Ofrece:

- **Control centralizado y sencillo**  
- **Separación de audio por categorías**: Background, musica, dialogos y SFX.
- ** Gestión de volumen con AudioMixer**
- **Persistencia de ajustes** en `PlayerPrefs`
- **Soporte de aleatorización** vía `AudioClipSO`
- **AudioDict** para repositorio de sonidos globales en Audio Manager. O locales en los stats del jugador.
---

## Puesta en marcha (rapida)
1. Arrastra el prefab AudioManager a la escena.

## Puesta en marcha (detallada)

1. **Asigna el `VolumeSettings`**  
   - Crea un asset de tipo `VolumeSettings` (Assets → Create → Settings → VolumeSettings).  
   - Arrástralo al campo `VolumeSettings` del `AudioManager` en la escena.

2. **Configura tus AudioClips o `AudioClipSO`**  
   - Usa `AudioClipSO` si quieres selección y parámetros aleatorios (volumen, pitch).  
   - Usa un `AudioDictionary` (opcional) si prefieres reproducir audio mediante “keys”.

3. **Ajusta volúmenes** en el `VolumeSettings`  
   - Cada categoría (master, music, etc.) tiene su propio slider.
   - Los cambios se guardan en `PlayerPrefs`.

---

## Clases Relevantes

### `AudioManager`
- **Singleton principal** para reproducir audio. 
- Separación de audio por categorías: Background, Music, Dialogue y SFX.
- Metodos PlayAudio, StopAudio, ChangeAudio, PauseAudio, ResumeAudio
- Sobrecargas para `AudioClip`, `AudioClipSO`, `string (key)`, etc.  
- Internamente, decide si el audio va en loop (Background/Music) o en one-shot (Dialogue/SFX).  
- Posibilidad de FadeIn y FadeOut

### `AudioDefault`
- Musica por defecto de la escena

### `VolumeSettings`
- `ScriptableObject` con campos de volumen para **master**, **background**, **music**, **dialogue** y **sfx**.
- Guarda/carga automáticamente valores en `PlayerPrefs`.

### `AudioClipReference`
- Clase serializable que contiene un `AudioClip` **o** un `AudioClipSO`.
- Permite cambiar fácilmente en el inspector si usas un clip “simple” o uno con aleatorización.

### `AudioClipSO`
- `ScriptableObject` que contiene uno o varios `AudioClip` y configuraciones de randomización (volumen, pitch, variaciones).
- Al reproducir, elige un clip aleatorio y aplica los parámetros dinámicos.

### `AudioFade`
- Realiza efecto de fade al AudioSource junto al que se coloque

### `AudioFadeUtility`
- Utilidad que se encarga de gestionar los efectos de fade

### `AudioDictionary`
- Diccionario que mapea `string` → `AudioClipReference` para cada categoría (background, music, sfx, dialogue).
- Útil para proyectos grandes: puedes reproducir sonidos por nombre en lugar de pasar referencias.

### `AudioManagerConnector`
- Clase “helper” con métodos para reproducir audio sin escribir `AudioManager.Instance...`.
- Ejemplo: `connector.PlayMusic(myClip)`, `connector.PlaySFXByKey("explosion")`.
- Util para llamarla desde UnityEvents.

---

## Ejemplos de Uso

### 1. Reproducir SFX 3D en una posición

Crea un objeto temporal con un `AudioSource`, ideal para explosiones, pasos, etc.

~~~~csharp
AudioManager.PlaySoundAtPoint(mySFXClip, transform.position, volume: 1f, pitch: 1f);
~~~~

### 2. Reproducir un clip directamente

Las principales categorias son: AudioCategory.Background, AudioCategory.Music, AudioCategory.Dialogue, AudioCategory.SFX. Por ejemplo, para reproducir una música de fondo:

~~~~csharp
AudioManager.Instance.PlayAudio(AudioCategory.Music, myAudioClip);
~~~~

Si se quiere que tenga FadeIn sería:

~~~~csharp
AudioManager.Instance.PlayAudio(AudioCategory.Music, myAudioClip, fadeTime: 5f);
~~~~

Y si se quiere que el cambio de una musica a otra sea paulatino:

~~~~csharp
AudioManager.Instance.ChangeAudio(AudioCategory.Music, myAudioClip, fadeOutTime: 5f, fadeInTime: 5f);
~~~~

### 3. Reproducir un `AudioClipSO`

Cada vez que llamas a `AudioManager.Instance.PlayAudio(...)` pasando un `AudioClipSO`, se seleccionará uno de los `Clips` de manera aleatoria y se le aplicarán variaciones de volumen/pitch si los flags (`Randomize Volume` y/o `Randomize Pitch`) están activos.

~~~~csharp
AudioManager.Instance.PlayAudio(AudioCategory.SFX, myClipSO);
~~~~

Puedes crear un recurso AudioClipSO desde: AudioSO/Audio Clip

### 4. Reproducir por clave (usando `AudioDictionary`)

~~~~csharp
AudioManager.Instance.PlayAudio(AudioCategory.Dialogue, "npcLine01");
~~~~

### 5. Usar `AudioManagerConnector`

Llamadas más simples desde otros scripts, UnityEvents o botones de UI.

~~~~csharp
public AudioManagerConnector connector;

void SomeMethod()
{
    connector.PlayMusic(myMusicClipReference); 
    connector.PlaySFXByKey("laserShot");
}
~~~~

---

## Ejemplos Interesantes y Casos de Uso Avanzados

### 6. AudioDictionary local para un enemigo o personaje

Cada enemigo/objeto puede tener su propio conjunto de sonidos sin mezclar claves en un diccionario global.

~~~~csharp
public class EnemyStats : MonoBehaviour
{
    [SerializeField] private AudioDictionary localAudioDict;

    public void PlayAttackSound()
    {
        AudioManager.Instance.PlayAudio(AudioCategory.SFX, localAudioDict.GetSfxClipReference("Attack"));
    }

    public void PlayDeathSound()
    {
        AudioManager.Instance.PlayAudio(AudioCategory.SFX, localAudioDict.GetSfxClipReference("Death"));
    }
}
~~~~

Basta con asignar a cada prefab (enemigo, jefe, etc.) su diccionario personalizado, y luego llamar a `PlayAudio` con las keys correspondientes.

### 7. AudioDictionary para un jugador con pisadas

Puedes tener “Footstep_Grass”, “Footstep_Metal”, etc.

~~~~csharp
void PlayFootstep()
{
    string surfaceKey = GetSurfaceKey(); // Determina la superficie actual
    AudioManager.Instance.PlayAudio(AudioCategory.SFX, myPlayerAudioDict.GetSfxClipReference(surfaceKey));
}
~~~~


---

## Conclusión

Con este sistema:

- **Centralizas** la reproducción de audio.  
- **Personalizas** cada categoría de sonido y guardas sus volúmenes.  
- **Aleatorizas** efectos y música para mayor variedad.  
- **Usas diccionarios globales o locales** para organizar tus sonidos.  
