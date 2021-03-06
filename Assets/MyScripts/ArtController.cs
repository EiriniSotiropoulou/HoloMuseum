﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.MixedReality.Toolkit.Experimental.Utilities;

public class ArtController : MonoBehaviour
{
    /*Creates spatial anchors for artifact and updates them between sessions depending to where the designer places them*/

    WorldAnchorManager MyManager;
    //variables to recognise artifacts
    public int number; 
    public string category; 
    void Awake()
    {
        ;// MyManager = gameObject.GetComponent<WorldAnchorManager>();
    }


    public void onPlace() //when the art is placed a spatial anchor is created or updated at the store
    {
        //MyManager.AttachAnchor(gameObject);
        Debug.Log("AnchorAttached");
    }

    public void onPickUp()//when the art is beign moved a spatial anchor is deleted at the store
    {
        //MyManager.RemoveAnchor(gameObject);
        Debug.Log("AnchorRemoved");
    }



}
