using UnityEngine;


public static class VolumeMathUtility
{
    public const float MIN_DECIBELS = -80f;
    public const float MAX_DECIBELS = 0f;

    /// <summary>
    /// Converts a decibel value to amplitude in [0..1].
    /// If decibels > 0, it’s clamped to amplitude = 1.
    /// If decibels is very negative, amplitude = 0.
    /// 
    ///     amplitude = 10^(decibels / 20)
    /// </summary>
    public static float DecibelToAmplitude(float decibels)
    {
        // Calculate the "raw" amplitude
        float amplitude = Mathf.Pow(10f, decibels / 20f);

        // Clamp amplitude to [0..1]
        return Mathf.Clamp01(amplitude);
    }

    /// <summary>
    /// Converts an amplitude in [0..1] to decibels.
    /// For amplitude <= 0, returns -80 dB (a practical silence floor).
    /// 
    ///     decibels = 20 * log10(amplitude)
    /// </summary>
    public static float AmplitudeToDecibel(float amplitude)
    {
        if (amplitude <= 0f)
            return -80f; // Or any practical minimum floor you prefer

        return 20f * Mathf.Log10(amplitude);
    }

    /// <summary>
    /// Maps a normalized volume [0..1] to decibels in [minDb..maxDb],
    /// by interpolating in amplitude space.
    /// 
    ///     1) Convert minDb->amplitudeMin, maxDb->amplitudeMax
    ///     2) Lerp between amplitudeMin and amplitudeMax using volume
    ///     3) Convert the result back to dB (which is then clamped in [minDb..maxDb])
    ///
    /// So if volume=0, you get minDb. If volume=1, you get maxDb.
    /// If volume=0.5, you get halfway in amplitude, which typically is ~ -6 dB if minDb=-80/maxDb=0.
    /// </summary>
    /// <param name="volume">Slider value in [0..1].</param>
    /// <param name="minDb">Minimum decibels (e.g., -80).</param>
    /// <param name="maxDb">Maximum decibels (e.g., 0 or 10).</param>
    /// <returns>Decibel value in [minDb..maxDb].</returns>
    public static float MapVolumeToDecibels(float volume, float minDb = MIN_DECIBELS, float maxDb = MAX_DECIBELS)
    {
        // 1) Convert the dB boundaries to amplitude (already clamped to [0..1])
        float amplitudeMin = DecibelToAmplitude(minDb);
        float amplitudeMax = DecibelToAmplitude(maxDb);

        // 2) Lerp amplitude linearly in [0..1]
        float amplitude = Mathf.Lerp(amplitudeMin, amplitudeMax, Mathf.Clamp01(volume));

        // 3) Convert that amplitude back to dB, then clamp to [minDb..maxDb]
        float dB = AmplitudeToDecibel(amplitude);
        return Mathf.Clamp(dB, minDb, maxDb);
    }
}