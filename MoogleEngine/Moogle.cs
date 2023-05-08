namespace MoogleEngine;


public static class Moogle
{


    public static SearchResult Query(string query)
    {
        Dictionary<string, float> queryTf = QueryClass.QueryTF(Processor.Normalizer(query));

        Dictionary<string, float> queryTfIdf = QueryClass.QueryTFIDF(queryTf, TFIDF.documentsIdf);

        Dictionary<string, float> Score = TFIDF.Score(queryTfIdf, TFIDF.documentsTfIdf);

        SearchItem[] items = new SearchItem[3] {
            new SearchItem(Score.ElementAt(Score.Count-1).Key, TFIDF.Snippet(Processor.DictForSnippet[Score.ElementAt(Score.Count-1).Key],queryTfIdf), Score.ElementAt(Score.Count-1).Value),
            new SearchItem(Score.ElementAt(Score.Count-2).Key,TFIDF.Snippet(Processor.DictForSnippet[Score.ElementAt(Score.Count-2).Key],queryTfIdf), Score.ElementAt(Score.Count-2).Value),
            new SearchItem(Score.ElementAt(Score.Count-3).Key, TFIDF.Snippet(Processor.DictForSnippet[Score.ElementAt(Score.Count-3).Key],queryTfIdf), Score.ElementAt(Score.Count-3).Value),
        };

        return new SearchResult(items, query);
    }
}
