using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ScreenshotTaker : MonoBehaviour
{
    
    [SerializeField] private string path = "";
    private int m_Count = 0;

    private bool canTake = true;
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (!canTake)
                return;

            canTake = false;
            StartCoroutine(ScreenshotCO());
        }
    }

    private IEnumerator ScreenshotCO()
    {
        Debug.Log("Go to take screenshot");
        yield return new WaitForEndOfFrame();
        yield return new WaitForSeconds(0.1f);
        
        var screenshot = ScreenCapture.CaptureScreenshotAsTexture();
        var p = path + "/screenshot_" + m_Count.ToString("00") + ".jpg";
        File.WriteAllBytes(p, screenshot.EncodeToJPG());

        canTake = true;
        m_Count++;
        
        Debug.Log("Screenshot taken");
        
        yield break;
    }
}
