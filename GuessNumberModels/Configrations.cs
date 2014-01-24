using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;
using System.Xml.Serialization;
using System.IO;
using System.IO.IsolatedStorage;
using System.Diagnostics;
using System.Xml;

namespace GuessNumberModels
{
    public class Setting : ViewModelBase
    {
        private int guessTimes = 0;

        [XmlElement]
        public int GuessTimes
        {
            get
            {
                return guessTimes;
            }
            set
            {
                guessTimes = value;
                RaisePropertyChanged(() => GuessTimes);
            }
        }

        private int useTime = 0;

        [XmlElement]
        public int UseTime
        {
            get
            {
                return useTime;
            }
            set
            {
                useTime = value;
                RaisePropertyChanged(() => UseTime);
            }
        }
    }

    public class Configrations
    {
        public const int Easy_Times = 10;
        public const int Medium_Times = 8;
        public const int Hard_Times = 6;

        public const int Easy_TimeSpan = 0;
        public const int Medium_TimeSpan = 5;
        public const int Hard_TimeSpan = 2;

        public Configrations()
        {
            Easy = new Setting() { GuessTimes = Easy_Times, UseTime = Easy_TimeSpan };
            Medium = new Setting() { GuessTimes = Medium_Times, UseTime = Medium_TimeSpan };
            Hard = new Setting() { GuessTimes = Hard_Times, UseTime = Hard_TimeSpan };
        }

        [XmlElement]
        public Setting Easy { get; set; }

        [XmlElement]
        public Setting Medium { get; set; }

        [XmlElement]
        public Setting Hard { get; set; }

        public void Save()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Configrations));
            using (Stream stream = new MemoryStream())
            {
                serializer.Serialize(stream, this);
                stream.Position = 0;

                using (IsolatedStorageFile isf = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    using (IsolatedStorageFileStream fs = new IsolatedStorageFileStream("a.txt", FileMode.Create, isf))
                    {
                        byte[] buffer = new byte[stream.Length];
                        stream.Read(buffer, 0, buffer.Length);
                        fs.Write(buffer, 0, buffer.Length);
                    }
                }
            }            
        }

        protected void Clone(object inSource, object outSource)
        {
            PropertyInfo[] properties = inSource.GetType().GetProperties();
            foreach (PropertyInfo pi in properties)
            {
                PropertyInfo osp = outSource.GetType().GetProperty(pi.Name);

                if (osp.PropertyType.IsPrimitive)
                {
                    pi.SetValue(inSource, osp.GetValue(outSource));
                }
                else
                {
                    object isource = pi.GetValue(inSource);
                    object osource = osp.GetValue(outSource);
                    Clone(isource, osource);
                }
            }
        }

        public void Load()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Configrations));
            using (IsolatedStorageFile isf = IsolatedStorageFile.GetUserStoreForApplication())
            {
                using (IsolatedStorageFileStream fs = new IsolatedStorageFileStream("a.txt", FileMode.Open, isf))
                {
                    byte[] buffer = new byte[fs.Length];
                    fs.Read(buffer, 0, buffer.Length);
                    using (MemoryStream ms = new MemoryStream(buffer))
                    {
                        Configrations config = serializer.Deserialize(ms) as Configrations;
                        if (null != config)
                        {
                            Clone(this, config);
                        }
                    }
                }
            }
        }
    }
}
