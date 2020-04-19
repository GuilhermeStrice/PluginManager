using PluginManager;
using System;

namespace TestPlugin
{
    public abstract class Class1 : Plugin
    {
        public Class1()
        {
        }
    }

    public class asd : Class1
    {
        public asd()
        {
            base.OnLoaded += Asd_OnLoaded;
        }

        private void Asd_OnLoaded(PluginData data)
        {
            Console.Out.WriteLine("I'm loaded man");
        }

        public override void DoProcessing(PluginProcessData data)
        {
            //Console.WriteLine("working");
        }

        public override string Name()
        {
            return "asd";
        }

        public override Version Version()
        {
            return new Version("1.0.0.0");
        }

        public override void BuildDataFile()
        {
            // Leave this empty if you are not using a data file
        }

        public override string DataFilePath()
        {
            return null;
        }
    }
}
