using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TimeConversion : MonoBehaviour
{
    [SerializeField]
    TMP_InputField timeInput;
    [SerializeField]
    TMP_InputField resultInput;


    public void ConvertTime()
    {
        string time = timeInput.text;

        string result = "";

        bool validate = ValidateTimeFormat(time);

        if (!validate)
            result = "Format Waktu Salah";
        else
        {
            int h1 = (int)time[1] - '0';
            int h2 = (int)time[0] - '0';
            int hh = (h2 * 10 + h1 % 10);


            if (hh == 12)
            {
                result = (time[8] == 'A' ? "00" : "12");
            }
            else
            {
                if (time[8] == 'P')
                    result = (hh + 12).ToString();
                else
                    result = time.Substring(0, 2);
            }

            for (int i = 2; i <= 7; i++)
            {
                result += time[i];
            }
        }

        resultInput.text = result;
    }

    bool ValidateTimeFormat(string time)
    {
        int hour = 0;
        int minute = 0;
        int second = 0;

        if (time.Length != 10)
            return false;

        if (int.TryParse(time.Substring(0, 2), out hour))
        {
            if (hour > 12 || hour < 1)
                return false;
        }

        if (int.TryParse(time.Substring(3, 2), out minute) && int.TryParse(time.Substring(6, 2), out second)) {
            if ((minute >= 60 || minute < 0) || (second >= 60 || second < 0))
                return false;
        }


        if (time.Substring(8, 2) != "AM" && time.Substring(8, 2) != "PM")
            return false;

        return true;
    }
}
