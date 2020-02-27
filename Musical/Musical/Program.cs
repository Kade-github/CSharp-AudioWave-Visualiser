using NAudio.Wave;
using System;
using System.Threading;

namespace Musical
{
    internal class Program
    {
        public static double lastPeak = 0;

        public static void Main(string[] args)
        {
            WasapiLoopbackCapture CaptureInstance = new WasapiLoopbackCapture();
            CaptureInstance.DataAvailable += (s, a) =>
            {
                float max = 0;
                var buffer = new WaveBuffer(a.Buffer);
                // interpret as 32 bit floating point audio
                for (int index = 0; index < a.BytesRecorded / 4; index++)
                {
                    var sample = buffer.FloatBuffer[index];

                    // absolute value 
                    if (sample < 0) sample = -sample;
                    // is this the max value?
                    if (sample > max) max = sample;
                }

                if (max.Equals(0))
                {
                    Console.Title = "Waiting for music";
                    Console.Clear();
                    lastPeak = 0;
                }
                else
                {
                    string ss = CalcTheAnimation(max * 100);
                    Console.SetCursorPosition((Console.WindowWidth - ss.Length) / 2, Console.CursorTop);
                    Console.WriteLine(ss);
                    Console.Title = "Current Peak: " + lastPeak;
                }
            };
            
            CaptureInstance.StartRecording();
            
            Thread.Sleep(-1);
        }

        public static string CalcTheAnimation(float peak)
        {
            string p = "                ";
            string bars = "";
            for (int i = 0; i < peak; i++)
            {
                if (i % 5 == 0)
                    bars += "|";
            }
            p = bars + bars;
            lastPeak = Math.Floor(peak);
            return p;
        }
    }
}