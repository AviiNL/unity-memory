using System.Collections.Generic;
using UnityEngine;


class TileColors
{
    public static IReadOnlyList<Color> Colors { get; private set; }

    static TileColors () {
        // static constructor
        List<Color> colors = new List<Color>();
        var rnd = new System.Random(); // and not make random colors here

        for (var i = 0; i < 100; i++) // make 100 colors, now.. if your board exeeds 100 pairs, you're fucked... so 20x20 board is max
            colors.Add(new Color((float)rnd.Next(0, 100) / 100,
                (float)rnd.Next(0, 100) / 100,
                (float)rnd.Next(0, 100) / 100
            ));


        Colors = colors;
    }
}
