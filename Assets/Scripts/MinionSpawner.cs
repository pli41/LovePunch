﻿using UnityEngine;
using System.Collections;

public class MinionSpawner : MonoBehaviour {

	public void Spawn(GameObject minion)
    {
        Instantiate(minion, transform.position, transform.rotation);
    }
}