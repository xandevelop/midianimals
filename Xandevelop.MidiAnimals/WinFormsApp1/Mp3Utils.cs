using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Xandevelop.MidiAnimals
{
    internal class SoundEffect
    {

        // see https://www.codeproject.com/Articles/14709/Playing-MP3s-using-MCI

        IWinMM Mci { get; set; }


        private string Alias { get; }


        public SoundEffect(string uniqueName, string path, IWinMM dllWrapper = null)
        {
            Alias = uniqueName;

            if (dllWrapper == null) Mci = new WinMM();
            else Mci = dllWrapper;

            Mci.SendString($"open \"{path}\" type mpegvideo alias {Alias}");


        }

        ~SoundEffect()
        {
            StopPlay();
            Mci.SendString($"close {Alias}");
        }

        public void StartPlay()
        {
            Mci.SendString($"play {Alias}");
        }
        public void StopPlay()
        {
            Mci.SendString($"seek {Alias} to start");
            Mci.SendString($"stop {Alias}");
        }
    }
}
