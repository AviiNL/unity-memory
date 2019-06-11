using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Tile : MonoBehaviour
{
    public UnityEvent OnFadeComplete = new UnityEvent();

    [SerializeField]
    private int pairID;

    private Color color;

    private bool fadeToColor = false;

    private new Renderer renderer;

    private float time = 0;
    private float destroyTime = 0;

    public bool isFading = false;

    private Color colorToFadeTo = Color.white;

    private bool isDestroying = false;


    public int PairID {
        get {
            return pairID;
        }
        set {
            pairID = value;
            color = TileColors.Colors[value]; // now when you set the pairID, it'll automatically assign the color
        }
    }

    public Color Color { // and this becomes readonly
        get {
            return color;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        renderer = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        ColorHandler();
        DestructionHandler();
    }

    public void FadeIn()
    {
        colorToFadeTo = this.Color;
        time = 0;
    }

    public void FadeOut()
    {
        colorToFadeTo = Color.white;
        time = 0;
    }

    public void Destroy()
    {
        destroyTime = 0;
        isDestroying = true;
    }


    private void DestructionHandler()
    {
        if (isDestroying)
        {
            destroyTime += Time.deltaTime / 1f;
        }

        transform.localScale = Vector3.Lerp(transform.localScale, Vector3.zero, destroyTime);

        if (transform.localScale.normalized.magnitude <= 0)
        {
            isFading = false;
            isDestroying = false;
            OnFadeComplete?.Invoke();
            Destroy(gameObject);
        }
    }

    private void ColorHandler()
    {
        if (time < 1f)
        {
            time += Time.deltaTime / 1f;
        }

        renderer.material.color = Color.Lerp(renderer.material.color, colorToFadeTo, time);

        // we're either white or the color we want to become, so unlock
        if (renderer.material.color == Color.white || renderer.material.color == this.Color)
        {
            if (isFading)
            {
                isFading = false;
                OnFadeComplete?.Invoke();
            }
        }
        else
            isFading = true;
    }

    private void OnMouseDown()
    {
        GameManager.instance.OnTileClicked(this);
    }
}
