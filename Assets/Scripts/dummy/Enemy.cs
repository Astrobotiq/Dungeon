using System;
using UnityEngine;

public class Enemy :MonoBehaviour // Sadece Rotate ve thunder skillerinde çağırabilmek için
{
    public void openThunderPng() {
        gameObject.transform.GetChild(0).gameObject.SetActive(true);
    }

    public void closeThunderPng() {
        gameObject.transform.GetChild(0).gameObject.SetActive(false);
    }
}
