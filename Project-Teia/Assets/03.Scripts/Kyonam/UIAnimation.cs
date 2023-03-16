using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIAnimation : MonoBehaviour {

    public IEnumerator MoveObject(GameObject target, Vector2 startPos, Vector2 toPos, float time)// start From specific position
    {
        target.transform.localPosition = startPos;

        for (int i = 0; i < 20; i++)
        {
            target.transform.localPosition = Vector2.Lerp(startPos, toPos, 0.05f * i);
            yield return new WaitForSeconds(time / 20f);
        }
        target.transform.localPosition = toPos;
    }
    public IEnumerator MoveObject(GameObject target, Vector2 toPos, float time)// start From targets position
    {
        Vector3 temp = target.transform.localPosition;

        for(int i = 0; i < 20; i++)
        {
            target.transform.localPosition = Vector2.Lerp(temp, toPos, 0.05f * i);
            yield return new WaitForSeconds(time / 20f);
        }
        target.transform.localPosition = toPos;
    }
    public IEnumerator MoveSmoothly(GameObject target, Vector3 toPos)
    {
        for(int i = 0; i < 15; i++)
        {
            target.transform.localPosition = Vector3.Lerp(target.transform.localPosition, toPos, 0.2f);
            yield return null;
        }
        target.transform.localPosition = toPos;
    }
    public IEnumerator MoveSmoothly(GameObject target, Vector2 toPos)
    {
        for (int i = 0; i < 15; i++)
        {
            target.transform.localPosition = Vector2.Lerp(target.transform.localPosition, toPos, 0.2f);
            yield return null;
        }
        target.transform.localPosition = toPos;
    }
    public IEnumerator FadeOut(Image target, float time)
    {
        float fTemp = time;
        Color temp = target.color;

        while(fTemp <= time)
        {
            fTemp -= Time.deltaTime;
            temp.a = fTemp / time;
            target.color = temp;
            yield return null;
        }

        temp.a = 0f;
        target.color = temp;
    }
    public IEnumerator FadeIn(Image target, float time)
    {
        float fTemp = 0.0f;
        Color temp = target.color;

        while (fTemp <= time)
        {
            fTemp += Time.deltaTime;
            temp.a = fTemp / time;
            target.color = temp;
            yield return null;
        }

        temp.a = 1f;
        target.color = temp;
    }
    public IEnumerator FadeOut(SpriteRenderer target, float time)
    {
        float fTemp = time;
        Color temp = target.color;

        while (fTemp <= time)
        {
            fTemp -= Time.deltaTime;
            temp.a = fTemp / time;
            target.color = temp;
            yield return null;
        }

        temp.a = 0f;
        target.color = temp;
    }
    public IEnumerator FadeIn(SpriteRenderer target, float time)
    {
        float fTemp = 0.0f;
        Color temp = target.color;

        while (fTemp <= time)
        {
            fTemp += Time.deltaTime;
            temp.a = fTemp / time;
            target.color = temp;
            yield return null;
        }

        temp.a = 0f;
        target.color = temp;
    }
}
