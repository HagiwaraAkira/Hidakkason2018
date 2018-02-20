using System.Collections.Generic;
using System.IO;
using UnityEngine;
public static class CsvLoad
{
    public const int id = 0;
    public const int level = 1;    
    public const int left = 2;    
    public const int top = 3;    
   public const int right = 4;    
   public const int bottom = 5;    
   public const int path = 6;    
      
    public static List<string[]> CsvDatas = new List<string[]>(); // CSVの中身を入れるリスト
    public static int CardNum = 0; // CSVの行数
    public static void Import(){
    var csvFile = Resources.Load("cardlist") as TextAsset; /* Resouces/CSV下のCSV読み込み */
        StringReader reader = new StringReader(csvFile.text);

        while(reader.Peek() > -1) {
            string line = reader.ReadLine();
            CsvDatas.Add(line.Split(',')); // リストに入れる
            CardNum++; // 行数加算
        }
        CsvDatas.RemoveAt(0);
    }
}
