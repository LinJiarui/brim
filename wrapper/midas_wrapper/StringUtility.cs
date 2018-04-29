using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utility
{
	public static class StringUtility
	{
		//根据str的空格，将str划分成段落，存入List中
		public static List<string> Split(string str)
		{
		    return Split(str, " ");
		}

		//根据str的sign标识，将str划分成段落，存入List中
		public static List<string> Split(string str, string sign)
		{
            //			List<string> result = new List<string>();
            //			str = str.Trim();
            //			string tempStr = str;
            //			tempStr = tempStr.Trim();
            //
            //			while (tempStr.Contains(sign)) {
            //				int index = tempStr.IndexOf(sign);
            //                tempStr = tempStr.Substring(0, index).Trim();   //添加Trim()方法by ghh
            //				result.Add(tempStr);
            //
            //				tempStr = str.Substring(index + sign.Length, str.Length - index - sign.Length);
            //				tempStr = tempStr.Trim();
            //				str = tempStr;
            //			}
            //
            //			if (tempStr != "") {
            //				result.Add(tempStr);
            //			}
            //
            //			return result;
            List<int> indexes = new List<int>();
            int betweenQuotes = -1;
            char curChar;

            indexes.Add(-1);
            for (int i = 0; i < str.Length; i++)
            {
                curChar = str[i];
                if (curChar == '\"')
                {
                    betweenQuotes *= -1;
                }
                else if (betweenQuotes == -1)
                {
                    if (curChar == sign[0])
                    {
                        indexes.Add(i);
                    }
                }
            }
            indexes.Add(str.Length);

            var parameters = new List<string>();
            for (int i = 0; i < indexes.Count - 1; i++)
            {
                var p = str.Substring(indexes[i] + 1, indexes[i + 1] - indexes[i] - 1).Trim();
                if (!string.IsNullOrWhiteSpace(p))
                {
                    parameters.Add(p);
                }
            }

            return parameters;
        }

		//根据str的空格，将str划分成段落，存入List中
		public static string TrimPare(string str, string sign)
		{
			string result;
			result = str.TrimStart(sign.ToCharArray());
			result = result.TrimEnd(sign.ToCharArray());
			return result;
		}
        //by zxy
        //根据str的sign标识，只保留sign标识之后的字符串存入List中
        public static string TrimFol(string str, string sign)
        {
            string result;
            int index = str.IndexOf(sign);
            result = str.Substring(index + sign.Length, str.Length - index - sign.Length);
            return result;
        }

        //by zxy
        //根据str的sign标识，只保留sign标识之前的字符串存入List中
        public static string TrimLas(string str, string sign)
        {
            string result;
            int index = str.IndexOf(sign);
            result = str.Substring(0, index);
            return result;
        }

	}
}
