using UnityEngine;

public class ConsoleToGUI : MonoBehaviour
{
    string myLog = "*begin log";
    string filename = "";
    public bool doShow = true;
    public bool doFile = true;
    int kChars = 700;

    void OnEnable()
    {
        Application.logMessageReceived += Log;
    }

    void OnDisable()
    {
        Application.logMessageReceived -= Log;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            doShow = !doShow;
        }
    }

    public void Log(string logString, string stackTrace, LogType type)
    {
        // for onscreen...
        myLog = myLog + "\n" + logString + "\n" + stackTrace + "\n \n";

        /*
        if (myLog.Length > kChars)
        {
            myLog = myLog.Substring(myLog.Length - kChars);
        }
        */
        if (!doFile)
            return;
        // for the file ...
        if (filename == "")
        {
            string d = Application.persistentDataPath;
            string r = Random.Range(1000, 9999).ToString();
            filename = d + "/log-" + r + ".txt";
            myLog += filename;
        }

        try
        {
            System.IO.File.Delete(filename);
            System.IO.File.AppendAllText(filename, myLog + "\n");
        }
        catch
        {
        }
    }

    void OnGUI()
    {
        if (!doShow)
        {
            return;
        }

        GUI.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity,
            new Vector3(Screen.width / 800.0f, Screen.height / 1200.0f, 1.0f));
        GUI.TextArea(new Rect(10, 10, 370, 540), myLog);
    }
}