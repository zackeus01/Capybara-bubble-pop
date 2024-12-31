using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TypewriteEffect : MonoBehaviour
{
    [SerializeField]private float TWSpeed;
    public  Coroutine Run(string textToType, TMP_Text textlable)
    {
      return  StartCoroutine(TypeText(textToType, textlable));
    }
    private IEnumerator TypeText(string textToType, TMP_Text textlable)
    {
        float t = 0;
        int charIndex = 0;
        while (charIndex < textToType.Length)
        {
            t += Time.deltaTime * TWSpeed;
            charIndex = Mathf.FloorToInt(t);
            charIndex = Mathf.Clamp(charIndex, 0, textToType.Length);
            textlable.text = textToType.Substring(0, charIndex);
            yield return null;
        }
        textlable.text = textToType;
    }
}
