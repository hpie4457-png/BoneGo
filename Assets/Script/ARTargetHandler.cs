using UnityEngine;
using Vuforia;

public class ARTargetHandler : MonoBehaviour
{
    public BoneViewer boneViewer;
    public string targetName;

    private ObserverBehaviour observer;

    void Start()
    {
        observer = GetComponent<ObserverBehaviour>();

        if (observer != null)
        {
            observer.OnTargetStatusChanged += OnTargetStatusChanged;
        }
    }

    private void OnTargetStatusChanged(ObserverBehaviour behaviour, TargetStatus status)
    {
        if (status.Status == Status.TRACKED || status.Status == Status.EXTENDED_TRACKED)
        {
            OnTargetDetected();
        }
        else
        {
            OnTargetLost();
        }
    }

    public void OnTargetDetected()
    {
        boneViewer.ShowBoneById(targetName);
        Debug.Log("Target Name : " + targetName);
    }

    public void OnTargetLost()
    {
        boneViewer.OnTargetLost(targetName);
        Debug.Log("Target Lost : " + targetName);
    }
}