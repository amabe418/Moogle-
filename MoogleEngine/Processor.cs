using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Text.RegularExpressions;
using System.Diagnostics;


namespace MoogleEngine;

public class Processor
{
    static string ContentPath = Path.Join("..", "Content");

    static string[] FilesPath = Directory.GetFiles(ContentPath, "*.txt");
    static string[] FilesContent = FullFilesContent(FilesPath);
    private static string[] FullFilesContent(string[] FilesPath)
    {
        string[] FilesContent = new string[FilesPath.Length];
        for (int i = 0; i < FilesContent.Length; i++)
        {
            FilesContent[i] = File.ReadAllText(FilesPath[i]);  //llene el array con los documentos.
        }
        return FilesContent;
    }
    public static Dictionary<string, string[]> titleAndContent = Converted(FilesPath, FilesContent);



    public static string[] Normalizer(string Entrance)
    {   /* 
            Este metodo limpia el string que contiene 
            al documento y lo convierte en un array donde se almacenan las palabras.
            -LLevo todo a letras minusculas
            -Quito caracteres raros
            -Reemplazo las letras con signos por letras normales
            */

        return Regex.Replace(Entrance.ToLower().Normalize(NormalizationForm.FormD), @"[^\da-z ]", "").Split(' ', StringSplitOptions.RemoveEmptyEntries);

    }

    private static Dictionary<string, string[]> Converted(string[] Path, string[] Content)
    {
        /* Este metodo devuelve un diccionario 
        con el titulo de los documentos como key y un array con las palabras de 
        cada documento como value */
        Dictionary<string, string[]> Documents = new Dictionary<string, string[]>();
        for (int i = 0; i < Path.Length; i++)
        {
            Documents.Add(FilesPath[i].Substring(FilesPath[i].IndexOf("Content") + 8), Normalizer(FilesContent[i]));
        }
        return Documents;
    }

    private static Dictionary<string, string> dictForSnippet(Dictionary<string, string[]> Title, string[] Content)
    {
        Dictionary<string, string> result = new Dictionary<string, string>();
        for (int i = 0; i < Content.Length; i++)
        {
            result.Add(Title.ElementAt(i).Key, Content[i]);
        }
        return result;
    }

public static Dictionary<string, string> DictForSnippet = dictForSnippet(titleAndContent,FilesContent);


}