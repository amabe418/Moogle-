


namespace MoogleEngine;

public class QueryClass
{
    /* En esta clase se encuentran los metodos necesarios para tener el peso
    de las palabras de la query para poder encontrar la similitud entre esta
    y los documentos*/

    public static Dictionary<string, float> QueryTF(string[] query)
    {   /* 
           metodo para encontrar la frecuencia de cada termino.   
        */
        float a = 0.5F;
        float maxFrequency = 0;
        Dictionary<string, float> tf = new Dictionary<string, float>();

        for (int i = 0; i < query.Length; i++)
        {
            if (!tf.ContainsKey(query[i]))
            {
                tf.Add(query[i], 1);
            }
            else
            {
                tf[query[i]]++;
            }
            maxFrequency = Math.Max(maxFrequency, tf[query[i]]);
        }

        foreach (var item in tf)
        {
            tf[item.Key] = a + ((1 - a) * (tf[item.Key] / maxFrequency));
        }

        return tf;

    }


    public static Dictionary<string, float> QueryTFIDF(Dictionary<string, float> tf, Dictionary<string, float> idf)
    {   /* Este metodo es para calcular el TF*IDF de cada palabra de la query
            usando el universo de palabras (documentsIdf) para su frecuencia inversa */
        Dictionary<string, float> tfIdfDocument = new Dictionary<string, float>();
        foreach (var item in tf)
        {
            if (!idf.ContainsKey(item.Key))
            {
                idf.Add(item.Key, 1);
            }
        }
        foreach (var keyValuePair in tf)
        {
            tfIdfDocument.Add(keyValuePair.Key, tf[keyValuePair.Key] * idf[keyValuePair.Key]);
        }
        return tfIdfDocument;
    }


   
   

}

