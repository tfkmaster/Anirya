using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlimScriptLinker : MonoBehaviour
{
    public AlimMeetingManager AlimMeetingManager = default;

    public void ActivateDialog()
    {
        Debug.Log("MOIJZFJIOMJOIFZJIOM");
        AlimMeetingManager.UIManager.SetActiveDialog(true);
        AlimMeetingManager.UIManager.GetDialogueManager().dialogHasStart = true;
        AlimMeetingManager.UIManager.GetDialogueManager().StartDialogue(AlimMeetingManager.Dialog);
    }

    public void StunAniryaForDialoguePurpose()
    {
        AlimMeetingManager.UIManager.GetDialogueManager().StunAniryaForDialoguePurpose();
    }
}
