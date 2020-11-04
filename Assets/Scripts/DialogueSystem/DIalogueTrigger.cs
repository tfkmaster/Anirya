using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : Interactible
{
    public Dialogue dialogue;
    public DialogueManager dM;

    void Start()
    {
        dM = FindObjectOfType<DialogueManager>();
    }

    public override void Trigger()
    {
        base.Trigger();
        if (!dM.dialogHasStart)
        {
            dM.dialogHasStart = true;
            dM.StartDialogue(dialogue);
        }
    }

}
