using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CursorManager : MonoBehaviour
{
    [SerializeField] private Texture[] animatedCursor = new Texture[2];
    [SerializeField] private RectTransform cursorRect;
    private RawImage cursorImage;

    private void Start()
    {
        Cursor.visible = false;
        cursorImage = cursorRect.GetComponent<RawImage>();
        StartCoroutine(AnimateCursor());
    }
    private void Update()
    {
        Vector3 newPos = Input.mousePosition;

        newPos.y -= cursorRect.rect.height / 2;
        newPos.x += cursorRect.rect.width / 2;

        cursorRect.position = newPos;
    }
    IEnumerator AnimateCursor()
    {
        int i = 0;
        while (true)
        {
            i++;
            cursorImage.texture = animatedCursor[i % 2];
            yield return new WaitForSeconds(0.5f);
        }

    }

}
