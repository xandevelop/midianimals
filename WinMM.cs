using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Xandevelop.MidiAnimals
{
    // https://docs.microsoft.com/en-us/previous-versions/dd798460(v=vs.85)
    public delegate void MidiInProc(int handle, int msg, int instance, int param1, int param2);

    public class MidiHandle
    {
        public int Handle { get; set; }
    }

    public interface IWinMM
    {
        void SendString(string cmd);
        MidiHandle MidiInOpen(int deviceId, MidiInProc proc, int instance);
        int MidiInStart(MidiHandle handle);
        int MidiInStop(MidiHandle handle);
        int MidiInClose(MidiHandle handle);
    }

    public class WinMM : IWinMM
    {
        [DllImport("winmm.dll")]
        private static extern long mciSendString(string Cmd, StringBuilder StrReturn, int ReturnLength, IntPtr HwndCallback);

        [DllImport("winmm.dll")]
        private static extern int midiInOpen(ref int handle, int deviceID, MidiInProc proc, int instance, int flags);
                
        [DllImport("winmm.dll")]
        private static extern int midiInClose(int handle);

        [DllImport("winmm.dll")]
        private static extern int midiInStart(int handle);

        [DllImport("winmm.dll")]
        private static extern int midiInStop(int handle);


        public void SendString(string cmd)
        {
            mciSendString(cmd, null, 0, IntPtr.Zero);
        }

        const int CALLBACK_FUNCTION = 0x30000; // call back to provided func.
        public MidiHandle MidiInOpen(int deviceId, MidiInProc proc, int instance)
        {
            int myHandle = 0;
            midiInOpen(ref myHandle, deviceId, proc, instance, CALLBACK_FUNCTION);
            if (myHandle == 0) throw new Exception();
            return new MidiHandle { Handle = myHandle };
        }

        public int MidiInStart(MidiHandle handle)
        {
            return midiInStart(handle.Handle);
        }

        public int MidiInClose(MidiHandle handle)
        {
            return midiInClose(handle.Handle);
        }

        public int MidiInStop(MidiHandle handle)
        {
            return midiInStop(handle.Handle);
        }
    }

    
}
