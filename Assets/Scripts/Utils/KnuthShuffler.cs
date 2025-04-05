using System;
using System.Collections.Generic;

public class KnuthShuffler
{
    private Random random;

    public KnuthShuffler()
    {
        random = new Random();
    }

    public KnuthShuffler(int seed)
    {
        random = new Random(seed);
    }

    public IList<T> Shuffled<T>(IList<T> list)
    {
        if (list is null)
        {
            throw new ArgumentNullException("List is null.");
        }
        var clone = new List<T>(list);
        KnuthShuffle(clone);
        return clone;
    }

    public void Shuffle<T>(IList<T> list)
    {
        if (list is null)
        {
            throw new ArgumentNullException("List is null.");
        }
        KnuthShuffle(list);
    }

    private void KnuthShuffle<T>(IList<T> list)
    {
        for (var i = 0; i < list.Count - 1; ++i)
        {
            var j = random.Next(i, list.Count);
            (list[i], list[j]) = (list[j], list[i]);
        }
    }
}
