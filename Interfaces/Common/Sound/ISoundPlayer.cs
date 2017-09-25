using System;
using System.IO;

namespace NTouchTypeTrainer.Interfaces.Common.Sound
{
    public interface ISoundPlayer
    {
        void Play(FileInfo waveFile);

        void Play(Stream waveStream);

        void Play(Uri waveResource);
    }
}