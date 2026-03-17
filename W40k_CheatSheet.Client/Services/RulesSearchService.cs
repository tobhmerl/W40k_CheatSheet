using System.Net.Http.Json;
using System.Text.Json.Serialization;

namespace W40k_CheatSheet.Client.Services;

public record RulesChunk(
    [property: JsonPropertyName("text")] string Text);

public record RulesSearchResult(string Text, double Score);

public class RulesSearchService(HttpClient http)
{
    private List<string> _chunks = [];
    private List<Dictionary<string, int>> _termFreqs = [];
    private Dictionary<string, int> _docFreqs = new();
    private double _avgDocLen;
    private bool _indexed;

    private const double K1 = 1.5;
    private const double B  = 0.75;

    public bool IsLoaded => _indexed;
    public bool HasChunks => _chunks.Count > 0;

    public async Task<bool> EnsureLoadedAsync()
    {
        if (_indexed) return HasChunks;
        try
        {
            var chunks = await http.GetFromJsonAsync<List<RulesChunk>>("data/rules-chunks.json");
            if (chunks is { Count: > 0 })
                BuildIndex(chunks.Select(c => c.Text).ToList());
        }
        catch { /* file not found or parse error — search disabled */ }
        _indexed = true;
        return HasChunks;
    }

    private void BuildIndex(List<string> docs)
    {
        _chunks    = docs;
        _termFreqs = docs.Select(d =>
            Tokenize(d).GroupBy(t => t).ToDictionary(g => g.Key, g => g.Count())
        ).ToList();
        _avgDocLen = _termFreqs.Average(tf => (double)tf.Values.Sum());
        _docFreqs  = new Dictionary<string, int>(StringComparer.Ordinal);
        foreach (var tf in _termFreqs)
            foreach (var term in tf.Keys)
                _docFreqs[term] = _docFreqs.GetValueOrDefault(term) + 1;
    }

    public List<RulesSearchResult> Search(string query, int topK = 5)
    {
        if (!_indexed || _chunks.Count == 0) return [];
        var queryTerms = Tokenize(query);
        if (queryTerms.Count == 0) return [];

        int n = _chunks.Count;
        var scores = new double[n];

        foreach (var term in queryTerms.Distinct(StringComparer.Ordinal))
        {
            int df = _docFreqs.GetValueOrDefault(term, 0);
            if (df == 0) continue;
            double idf = Math.Log((n - df + 0.5) / (df + 0.5) + 1.0);

            for (int i = 0; i < n; i++)
            {
                int tf = _termFreqs[i].GetValueOrDefault(term, 0);
                if (tf == 0) continue;
                int docLen  = _termFreqs[i].Values.Sum();
                double tfNorm = tf * (K1 + 1.0) / (tf + K1 * (1.0 - B + B * docLen / _avgDocLen));
                scores[i] += idf * tfNorm;
            }
        }

        return scores
            .Select((score, i) => new RulesSearchResult(_chunks[i], score))
            .Where(r => r.Score > 0)
            .OrderByDescending(r => r.Score)
            .Take(topK)
            .ToList();
    }

    private static List<string> Tokenize(string text) =>
        text.ToLowerInvariant()
            .Split([' ', '\t', '\n', '\r', '.', ',', ';', ':', '!', '?',
                    '(', ')', '[', ']', '-', '/', '"', '\'', '–', '—'],
                   StringSplitOptions.RemoveEmptyEntries)
            .Where(t => t.Length > 2 && !StopWords.Contains(t))
            .ToList();

    private static readonly HashSet<string> StopWords =
        new(StringComparer.OrdinalIgnoreCase)
        {
            "the", "and", "for", "are", "was", "not", "you", "this", "that",
            "with", "they", "from", "can", "has", "had", "but", "its", "your",
            "may", "any", "all", "one", "have", "been", "must", "each", "more",
            "than", "when", "will", "such", "which", "also", "into", "onto",
            "their", "there", "then", "these", "those", "within", "during",
        };
}
