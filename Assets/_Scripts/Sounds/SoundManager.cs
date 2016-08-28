using System.Collections.Generic;

public class SoundManager : Singleton<SoundManager>
{
    private bool _soundEnable = true;

    public bool SoundEnable
    {
        get
        {
            return _soundEnable;
        }
        set
        {
            _soundEnable = value;

            if (_soundEnable)
            {
                ResumeAll();
            }
            else
            {
                PauseAll();
            }
        }
    }

    public List<AudioSourceAudioIdTuple> Sounds;

    public void PlayAudio(params AudioId[] audioId)
    {
        if (Sounds == null)
            return;

        if (!SoundEnable)
            return;

        for (int i = 0; i < audioId.Length; i++)
        {
            var sound = Sounds.Find(s => s.AudioId == audioId[i]);
            if (!sound.AudioSource.isPlaying)
                sound.AudioSource.Play();
        }
    }

    public void Stop(params AudioId[] audioIds)
    {
        if (Sounds == null)
            return;

        for (int i = 0; i < audioIds.Length; i++)
        {
            var sound = Sounds.Find(s => s.AudioId == audioIds[i]);
            sound.AudioSource.Stop();
        }
    }

    public void StopAll()
    {
        if (Sounds == null)
            return;

        for (int i = 0; i < Sounds.Count; i++)
        {
            Sounds[i].AudioSource.Stop();
        }
    }

    public void PauseAll()
    {
        if (Sounds == null)
            return;

        for (int i = 0; i < Sounds.Count; i++)
        {
            Sounds[i].AudioSource.Pause();
        }
    }

    public void ResumeAll()
    {
        if (Sounds == null)
            return;

        for (int i = 0; i < Sounds.Count; i++)
        {
            Sounds[i].AudioSource.UnPause();
        }
    }

    public void Pause(params AudioId[] audioIds)
    {
        if (Sounds == null)
            return;

        for (int i = 0; i < audioIds.Length; i++)
        {
            var sound = Sounds.Find(s => s.AudioId == audioIds[i]);
            sound.AudioSource.Pause();
        }
    }

    public void Resume(params AudioId[] audioIds)
    {
        if (Sounds == null)
            return;

        for (int i = 0; i < audioIds.Length; i++)
        {
            var sound = Sounds.Find(s => s.AudioId == audioIds[i]);
            sound.AudioSource.Stop();
        }
    }
}
