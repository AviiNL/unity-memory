using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField]
    private GameObject tile;


    private Tile firstPick;
    private Tile secondPick;

    public List<Color> Colors; // now ofcourse you should have a predefined set of say, 100 colors here

    private bool canPick = true;

    private List<Tile> tiles = new List<Tile>();

    private bool waiting = false;


    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);

        CreateBoard(6, 4, 1.3f);
    }

    // Update is called once per frame
    void Update()
    {
        if (instance == null)
            instance = this;

        if (instance != this)
        {
            Destroy(instance.gameObject);
            instance = this;
        }

        if (firstPick != null && secondPick != null)
        {
            if (firstPick.PairID != secondPick.PairID)
            {
                StartCoroutine(WaitAndFadeOut(firstPick, secondPick));
            }
            else
            {
                StartCoroutine(WaitAndDestroy(firstPick, secondPick));
            }

            firstPick = null;
            secondPick = null;
        }
    }

    IEnumerator WaitAndFadeOut(Tile t1, Tile t2)
    {
        waiting = true;
        yield return new WaitForSeconds(1f);
        waiting = false;

        t1.FadeOut();
        t2.FadeOut();

        yield return null;
    }

    IEnumerator WaitAndDestroy(Tile t1, Tile t2)
    {
        waiting = true;
        yield return new WaitForSeconds(1f);
        waiting = false;

        t1.Destroy();
        t2.Destroy();

        yield return null;
    }

    public void CreateBoard(int cols, int rows, float spacing)
    {
        if ((cols * rows) % 2 != 0)
        {
            throw new Exception("Total amount of tiles is not divisable by 2");
        }

        var totalPairs = (cols * rows) / 2;
        var pairs = new List<int>();

        for (int a = 0; a < 2; a++)
            for (var i = 0; i < totalPairs; i++)
                pairs.Add(i);

        pairs = pairs.OrderBy(a => Guid.NewGuid()).ToList();

        int fuckyoucounter = 0;
        for (int z = 0; z < rows; z++)
        {
            for (int x = 0; x < cols; x++)
            {
                var obj = Instantiate(tile);
                var t = obj.GetComponent<Tile>();
                t.OnFadeComplete.AddListener(checkIfStillLocked);
                obj.transform.position = new Vector3(obj.transform.localScale.x * (x * spacing) + 1, 0, obj.transform.localScale.z * (z * spacing));
                t.PairID = pairs[fuckyoucounter];
                tiles.Add(t);
                fuckyoucounter++;
            }
        }

        // TODO: Calculate camera position
    }

    public void checkIfStillLocked()
    {
        var done = true;

        if (waiting)
        {
            done = false;
        }
        else
        {
            foreach (var t in tiles)
            {
                if (t.isFading)
                {
                    done = false; // there is one not completed yet
                    break;        // no need to continue checking
                }
            }
        }

        if (done)
            canPick = true;
    }



    public void OnTileClicked(Tile tile)
    {
        if (!canPick) return;
        canPick = false;

        print($"Clicked on {tile.PairID}");

        if (!firstPick)
        {
            firstPick = tile;
            tile.FadeIn();
        }

        else if (firstPick && !secondPick)
        {
            if (firstPick == tile)
                return; // smartass...

            secondPick = tile;
            tile.FadeIn();
        }
    }

}
