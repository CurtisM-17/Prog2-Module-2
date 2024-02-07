using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        collision.gameObject.SendMessage("TakeDamage", 5, SendMessageOptions.DontRequireReceiver);
    }
}
