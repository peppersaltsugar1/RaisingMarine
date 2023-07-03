using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITakeDamage
{
    void TakeDamage(int damage, int player);
    void TakeDamage(int damage);
}
