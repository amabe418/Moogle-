using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Text.RegularExpressions;
using System.Diagnostics;

namespace MoogleEngine;


public static class TFIDF
{
    public static Dictionary<string, Dictionary<string, float>> documentsTf = tfFull(Processor.titleAndContent);
    static Dictionary<string, Dictionary<string, float>> tfFull(Dictionary<string, string[]> titleAndContent)
    {
        Dictionary<string, Dictionary<string, float>> documentsTf = new Dictionary<string, Dictionary<string, float>>();
        foreach (var item in titleAndContent)
        {
            documentsTf.Add(item.Key, TF(item.Value));
        }
        return documentsTf;
    }

    public static Dictionary<string, float> documentsIdf = IDF(documentsTf);

    public static Dictionary<string, Dictionary<string, float>> documentsTfIdf = TFxIDF(documentsTf, documentsIdf);


    public static Dictionary<string, float> TF(string[] Words)
    {  /* 
           Este metodo hace el tf de cada palabra del documento.
           El tf de una palabra es la primera parte del TF-IDF, 
           que es para saber que tan relevante es una palabra.
           La formula del TF es : frecuencia de la palabra entre 
           el valor de la palabra con mayor frecuencia, la frecuencia 
           es la cantidad de veces que se repite la palabra en el documento.
     */
        Dictionary<string, float> WordsTf = new Dictionary<string, float>();
        float Highest_Frequency = 0;
        for (int a = 0; a < Words.Length; a++)
        {
            if (!WordsTf.ContainsKey(Words[a]))
            {
                WordsTf.Add(Words[a], 1);
            }
            else
            {
                WordsTf[Words[a]]++;
            }
            Highest_Frequency = Math.Max(Highest_Frequency, WordsTf[Words[a]]);
        }

        foreach (var keyValue in WordsTf)
        {
            WordsTf[keyValue.Key] = WordsTf[keyValue.Key] / Highest_Frequency;
        }

        return WordsTf;
    }


    public static Dictionary<string, float> IDF(Dictionary<string, Dictionary<string, float>> Documents)
    {   /* 
            Este metodo es para obtener y guardar en un diccionario la frecuencia inversa
            de cada palabra que existe en al menos un documento de la base de datos, el objetivo
            es multiplicar este valor por el de la frecuencia de la palabra para obtener su peso y
            ver que tan semejante es con la query. 
            formula: logaritmo en base 10 del resultador de dividir la cantidad de documentos
                     entre la cantidad de documentos en los que aparece la palabra.
        */
        int docsAmmount = Documents.Count;

        Dictionary<string, float> WordsIdf = new Dictionary<string, float>();

        foreach (var item in Documents)
        {
            foreach (var kvp in item.Value)
            {
                if (!WordsIdf.ContainsKey(kvp.Key))
                {
                    WordsIdf.Add(kvp.Key, 1);
                }
                else
                {
                    WordsIdf[kvp.Key]++;
                }
            }
        }

        foreach (var item in WordsIdf)
        {
            WordsIdf[item.Key] = (float)(Math.Log10(docsAmmount / WordsIdf[item.Key]));
        }
        return WordsIdf;
    }




    public static Dictionary<string, Dictionary<string, float>> TFxIDF(Dictionary<string, Dictionary<string, float>> TF, Dictionary<string, float> IDF)
    {  /* 
          este metodo es para encontrar el peso de cada palabra
     */
        Dictionary<string, Dictionary<string, float>> tfIdfDocuments = new Dictionary<string, Dictionary<string, float>>();
        foreach (var keyValuePair in TF)
        {  //Aqui estoy dandole el titulo del documento al nuevo diccionario
            tfIdfDocuments.Add(keyValuePair.Key, new Dictionary<string, float>());
        }

        for (int i = 0; i < tfIdfDocuments.Count; i++)
        {
            foreach (var item in TF.ElementAt(i).Value)
            {
                tfIdfDocuments.ElementAt(i).Value.Add(item.Key, item.Value * IDF[item.Key]);
            }
        }



        return tfIdfDocuments;
    }

    /* 
      El score es el valor de semejanza que hay entre el documento y la query
   */
    public static Dictionary<string, float> Score(Dictionary<string, float> query, Dictionary<string, Dictionary<string, float>> Documents)
    {
        Dictionary<string, float> score = new Dictionary<string, float>();
        float queryModule = 0;
        foreach (var item in query)
        {
            queryModule = queryModule + (float)(Math.Pow(item.Value, 2));
        }
        foreach (var keyValuePair in Documents)
        {
            float vectorsMultiplied = 0;
            for (int a = 0; a < query.Count; a++)
            {
                if (keyValuePair.Value.ContainsKey(query.ElementAt(a).Key))
                {
                    vectorsMultiplied = vectorsMultiplied + (query.ElementAt(a).Value * keyValuePair.Value[query.ElementAt(a).Key]);
                }
            }
            float documentModule = 0;
            foreach (var item in keyValuePair.Value)
            {
                documentModule = documentModule + (float)(Math.Pow(item.Value, 2));
            }

            float scoreValue = vectorsMultiplied / (float)((Math.Sqrt(documentModule)) * (Math.Sqrt(queryModule)));


            score.Add(keyValuePair.Key, scoreValue);
        }
        score = score.OrderBy(x => x.Value).ToDictionary(x => x.Key, x => x.Value);

        return score;
    }
    /* 
            El snippet es una porcion del documento en el que aparece al menos una palabra de la query
         */
    public static string Snippet(string Documento, Dictionary<string, float> Query)
    {
        List<string> finalResult = Documento.Split(".").ToList();
        int counter = 0;
        List<string> workIn = new List<string>();

        foreach (var item in finalResult)
        {
            workIn.Add(Regex.Replace(item.ToLower().Normalize(NormalizationForm.FormD), @"[^\da-z ]", ""));

        }
        foreach (var item in Query)
        {
            for (int i = 0; i < workIn.Count; i++)
            {
                if (workIn.ElementAt(i).Contains(item.Key))
                {
                    counter = i;
                    break;
                }
            }
        }
        string Result;
        if ((counter >= 0) && (counter < finalResult.Count - 1))
        {
            Result = finalResult.ElementAt(counter) + finalResult.ElementAt(counter + 1);
        }
        else if ((counter == finalResult.Count - 1) && (finalResult.Count > 1))
        {
            Result = finalResult.ElementAt(counter - 1) + finalResult.ElementAt(counter);
        }
        else
        {
            Result = finalResult.ElementAt(counter);
        }

        return Result;
    }




}






