using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Utility;

namespace Porter.Midas.Entities
{
    public class MidasStoryEntity
    {
        private string _storyName;
        private double _level;
        private bool _bfldiap;
        private double _windwx;
        private double _windwy;
        private double _windcx;
        private double _windcy;
        private double _eccx;
        private double _eccy;
        private double _ieccx;
        private double _ieccy;
        private double _tafx;
        private double _tafy;

        public string StoryName { get { return _storyName; } set { _storyName = value; } }
        public double Level { get { return _level; } set { _level = value; } }
        public bool Bfldiap { get { return _bfldiap; } set { _bfldiap = value; } }
        public double Windwx { get { return _windwx; } set { _windwx = value; } }
        public double Windwy { get { return _windwy; } set { _windwy = value; } }
        public double Windcx { get { return _windcx; } set { _windcx = value; } }
        public double Windcy { get { return _windcy; } set { _windcy = value; } }
        public double Eccx { get { return _eccx; } set { _eccx = value; } }
        public double Eccy { get { return _eccy; } set { _eccy = value; } }
        public double Ieccx { get { return _ieccx; } set { _ieccx = value; } }
        public double Ieccy { get { return _ieccy; } set { _ieccy = value; } }
        public double Tafx { get { return _tafx; } set { _tafx = value; } }
        public double Tafy { get { return _tafy; } set { _tafy = value; } }

        public static Dictionary<string, MidasStoryEntity> ReadStrings(StreamReader sr)
        {
            Dictionary<string, MidasStoryEntity> result = new Dictionary<string, MidasStoryEntity>();
            MidasStoryEntity story;
            List<string> strList;
            string storyID;
            string str = sr.ReadLine();
            while (str[0] == ';')
            {
                str = sr.ReadLine();
            }
            while (str != "")
            {
                strList = StringUtility.Split(str, ",");
                story = new MidasStoryEntity();
                storyID = strList[0];
                story.StoryName = storyID;
                story.Level=Convert.ToDouble(strList[1]);
                story.Bfldiap=(strList[2] == "YES" ? true : false);
                story.Windwx = Convert.ToDouble(strList[3]);
                story.Windwy = Convert.ToDouble(strList[4]);
                story.Windcx = Convert.ToDouble(strList[5]);
                story.Windcy = Convert.ToDouble(strList[6]);
                story.Eccx = Convert.ToDouble(strList[7]);
                story.Eccy = Convert.ToDouble(strList[8]);
                story.Ieccx = Convert.ToDouble(strList[9]);
                story.Ieccy = Convert.ToDouble(strList[10]);
                story.Tafx = Convert.ToDouble(strList[11]);
                story.Tafy = Convert.ToDouble(strList[12]);
                result.Add(storyID, story);
                str = sr.ReadLine();
            }
            return result;
        }

    }
}
