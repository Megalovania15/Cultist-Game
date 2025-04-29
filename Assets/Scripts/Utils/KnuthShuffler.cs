using System;
using System.Collections.Generic;

// Provides an implementation of the Fisher-Yates or Knuth Shuffle, which is a
// fast algorithm O(n) for uniformly shuffling a list of items.
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

    // Gets a uniformly distributed shuffle of the given list. The original
    // list is unmodified.
    public IList<T> Shuffled<T>(IList<T> list)
    {
        if (list is null)
        {
            throw new ArgumentNullException(nameof(list));
        }
        var clone = new List<T>(list);
        KnuthShuffle(clone);
        return clone;
    }

    // Uniformly shuffles the given list in-place.
    public void Shuffle<T>(IList<T> list)
    {
        if (list is null)
        {
            throw new ArgumentNullException(nameof(list));
        }
        KnuthShuffle(list);
    }

    // Knuth shuffle algorithm.
    private void KnuthShuffle<T>(IList<T> list)
    {
        for (var i = 0; i < list.Count - 1; ++i)
        {
            var j = random.Next(i, list.Count);
            (list[i], list[j]) = (list[j], list[i]);
        }
    }
}
