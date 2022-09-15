using System;
using System.Collections.Generic;
using System.Text;

namespace Xandevelop.MidiAnimals
{
    

    public delegate void MidiKeyEventHandler(int MidiNoteCode);

    public class MidiInHandler
    {
        public event MidiKeyEventHandler OnKeyDown;
        public event MidiKeyEventHandler OnKeyUp;

        private IWinMM WinMM { get; }

        public MidiInHandler(IWinMM winMM = null)
        {
            if (winMM == null) WinMM = new WinMM();
            else WinMM = winMM;
        }

        private MidiHandle Handle { get; set; }
        public void OpenStart()
        {
            var Handle = WinMM.MidiInOpen(0, myMidiHandler, 0);
            WinMM.MidiInStart(Handle);
        }
        public void StopClose()
        {
            WinMM.MidiInStop(Handle);
            WinMM.MidiInClose(Handle);
        }

        private const int MIM_OPEN = 0x3C1; // device opened
        private const int MIM_CLOSE = 0x3C2; // device closed
        private const int MIM_DATA = 0x3C3; // data received

        private const int AmbiantSystemMessage = 0xFE;

        private void myMidiHandler(int handle, int msg, int instance, int param1, int param2)
        {
            if (msg == MIM_OPEN)
            {
                // device opened ok
            }
            else if (msg == MIM_CLOSE)
            {
                // device closed
            }
            else if (msg == MIM_DATA)
            {
                if (param1 == AmbiantSystemMessage)
                {
                    return;
                }
                else
                {
                    string msgHex = param1.ToString("X"); // hex representation of num
                    if (msgHex.Length != 6) return; // cannot be note on/off

                    var noteCodeHex = msgHex.Substring(2, 2);
                    string msgTypeHex = msgHex.Substring(4, 2); // on/off/pitch wheel/etc

                    int noteVal = Int16.Parse(noteCodeHex, System.Globalization.NumberStyles.HexNumber);

                    if (msgTypeHex.CompareTo("90") == 0)
                    {
                        OnKeyDown?.Invoke(noteVal);
                    }
                    if (msgTypeHex == "80") OnKeyUp?.Invoke(noteVal);
                }
            }
        }
    }
}
