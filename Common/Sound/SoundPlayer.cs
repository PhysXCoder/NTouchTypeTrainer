using NTouchTypeTrainer.Interfaces.Common.Sound;
using System;
using System.IO;
using System.Windows;

namespace NTouchTypeTrainer.Common.Sound
{
    public class SoundPlayer : ISoundPlayer
    {
        public void Play(FileInfo waveFile)
        {
            using (var player = new System.Media.SoundPlayer(waveFile.FullName))
            {
                player.Play();
            }
        }

        public void Play(Stream waveStream)
        {
            using (var player = new System.Media.SoundPlayer(waveStream))
            {
                player.Play();
            }
        }

        public void Play(Uri waveResource)
        {
            var waveStreamInfo = Application.GetResourceStream(waveResource);
            if (waveStreamInfo == null)
            {
                throw new ApplicationException($"Couldn't load wave resource {waveResource}");
            }

            using (var waveStream = waveStreamInfo.Stream)
            {
                Play(waveStream);
            }
        }
    }
}