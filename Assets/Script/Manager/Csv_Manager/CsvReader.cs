using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[SerializeField]
public class CsvReader : MonoBehaviour {

    public string filename;
    protected string[,] all_words;


    public void Read_csv()
    {
        Read();
    }
    public virtual void Read_csv(string _filename)
    {
        filename = _filename;
        Read();
    }

    public virtual void Read()
    {
        StreamReader sr;
        sr = new StreamReader("Assets/Resources/"+ filename + ".csv",System.Text.Encoding.Default);
        string all_text = sr.ReadToEnd();
        string[] lines = all_text.Split('\n');
        string top_line = lines[0];
        string[] top_line_words = top_line.Split(',');
        all_words = new string[top_line_words.Length,lines.Length];
        //Debug.Log(all_words.GetLength(0) + "   " + all_words.GetLength(1));

        for (int column = 0; column < all_words.GetLength(1); column++)
        {
            string[] words = lines[column].Split(',');
            for (int row = 0; row < all_words.GetLength(0); row++)
            {
                try
                {
                    if (words[row] == "") break;
                }
                catch
                {
                    Debug.LogError("index error : " + row);
                }
                all_words[ row, column] = words[row];
            }
        }
        /* 오버라이딩할때 쓸 것.
        for (int column = 1; column < all_words.GetLength(1); column++)
        {
            for (int row = 0; row < all_words.GetLength(0); row++)
            {
                if (all_words[row, column] == "" | all_words[row, column] == null)
                {
                    //Debug.Log("end");
                    break;
                }
                //Debug.Log(all_words[row, column]);
            }
        }
        */
        sr.Close();
    }

    public virtual void Write()
    {
    }

    public T Pop<T>(List<T> list)
    {
        T value = list[0];
        list.RemoveAt(0);

        return value;
    }
}
