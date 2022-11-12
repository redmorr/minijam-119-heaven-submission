using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Health : MonoBehaviour
{
    public abstract void Damage(int damage, Vector2 pushback);
}
