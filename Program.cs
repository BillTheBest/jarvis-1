using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Threading;
using System.Speech.Synthesis;


namespace Jarvis
{
    class Program
    {
        public static SpeechSynthesizer synth = new SpeechSynthesizer();


        static void Main(string[] args)
        {

            //List of messages that would be selected at random
            List<string> cpuMaxOutMessages = new List<string>();
            cpuMaxOutMessages.Add("WARNING! CPU is about to catch fire");
            cpuMaxOutMessages.Add("WARNING! Oh My God! you should not run your PC that hard");
            cpuMaxOutMessages.Add("WARNING! Stop all the downloads");
            cpuMaxOutMessages.Add("WARNING! REST IN PIECE CPU!");
            cpuMaxOutMessages.Add("RED ALERT! RED ALERT! RED ALERT! RED ALERT!");

            //The dice like DND
            Random rand = new Random();


            //This will read the user

            synth.Speak("Welcome to Jarvis version one point oh!");


            #region My Performance Counters
            //CPU Load percentage
            PerformanceCounter perfCpuCount = new PerformanceCounter("Processor Information", "% Processor time", "_Total");
            perfCpuCount.NextValue();

            //Current available memory in Megabytes
            PerformanceCounter perfMemCount = new PerformanceCounter("Memory", "Available MBytes");
            perfMemCount.NextValue();

            //Number of days, hours, since powered off (in seconds)
            PerformanceCounter perfUptimeCount = new PerformanceCounter("System", "System Up Time");
            perfUptimeCount.NextValue();
            #endregion

            //Speak the current system uptime
            TimeSpan upTimeSpan = TimeSpan.FromSeconds(perfUptimeCount.NextValue());
            string systemUptimeMessage = string.Format("Current system uptime is {0} days {1} hours {2} minutes {3} seconds",
                (int)upTimeSpan.TotalDays,
                (int)upTimeSpan.Hours,
                (int)upTimeSpan.Minutes,
                (int)upTimeSpan.Seconds
                );

            //Display the current system uptime
            Console.WriteLine("Current System Up time is {0} days {1} hours {2} minutes {3} seconds",
                (int)upTimeSpan.TotalDays,
                (int)upTimeSpan.Hours,
                (int)upTimeSpan.Minutes,
                (int)upTimeSpan.Seconds
                );

            //Tell the user what the current uptime is
            ShreyasSpeak(systemUptimeMessage, VoiceGender.Female, 3);

            int speechSpeed = 1;
            bool isChromeOpen = false;

            while (true)
            {



                //Get the current performance values
                int currentCpuPercentage = (int)perfCpuCount.NextValue();
                int currentAvailableMemory = (int)perfMemCount.NextValue();

                //Print the CPU % every 1 second   
                Console.WriteLine("Cpu Load: {0}%", currentCpuPercentage);
                Console.WriteLine("Memory  : {0}MB", currentAvailableMemory);

                

                #region Logic
                //Speak to the user only if the current CPU Usage is above 80%
                if (currentCpuPercentage > 80)
                {
                    if (currentCpuPercentage == 100)
                    {
                        //This will prevent the speech speed exceeding 5 times the normal speed
                        if (speechSpeed < 5)
                        {
                            speechSpeed++;
                        }
                        string cpuLoadVocalMessage = cpuMaxOutMessages[rand.Next(5)];
                       
                        if (isChromeOpen == false)
                        {
                            openWebsite("www.shreyasgaonkar.com");
                            isChromeOpen = true;
                        }

                        ShreyasSpeak(cpuLoadVocalMessage, VoiceGender.Male, speechSpeed);

                    }
                    else
                    {
                        string cpuLoadVocalMessage = String.Format("The current CPU load is {0} percent", currentCpuPercentage);
                        ShreyasSpeak(cpuLoadVocalMessage, VoiceGender.Female, 5);
                    }
                }


                //Speak to the user only if the available memory is less than 1GB
                if (currentAvailableMemory < 1024)
                {

                    string memAvailableMessage = String.Format("You currently have {0} megabytes of memory available", currentAvailableMemory);
                    ShreyasSpeak(memAvailableMessage, VoiceGender.Male, 10);
                }
                #endregion


                Thread.Sleep(1000);
            }//end of loop
        }


        /// <summary>
        /// Speaks with a selected voice
        /// </summary>
        /// <param name="message"></param>
        /// <param name="voiceGender"></param>
        public static void ShreyasSpeak(string message, VoiceGender voiceGender)
        {
            synth.SelectVoiceByHints(voiceGender);
            synth.Speak(message);
        }

        /// <summary>
        /// Speaks with a selected voice
        /// </summary>
        /// <param name="message"></param>
        /// <param name="voiceGender"></param>
        /// <param name="rate"></param>
        public static void ShreyasSpeak(string message, VoiceGender voiceGender, int rate)
        {
            synth.Rate = rate;
            ShreyasSpeak(message, voiceGender);
        }

        public static void openWebsite(string URL)
        {
            Process p1 = new Process();
            p1.StartInfo.FileName = "chrome.exe";
            p1.StartInfo.Arguments = URL;
            p1.StartInfo.WindowStyle = ProcessWindowStyle.Maximized;
            p1.Start();
        }

    }
}
