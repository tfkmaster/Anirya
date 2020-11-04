using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class DialogueManager : MonoBehaviour
{
    public Text nametext;
    public Text dialogText;

    public bool dialogHasStart = false;
    private Queue<string> sentences;

    private GameObject player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        sentences = new Queue<string>();
    }

    void Update()
    {
        if (dialogHasStart && Input.GetKeyDown(KeyCode.E)) DisplayNextSentence();
    }

    public void StartDialogue(Dialogue dialogue)
    {
        player.GetComponent<CharacterMovement>().Interacting = true;
        player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        player.GetComponent<Animator>().SetBool("interacting", true);
        nametext.text = dialogue.name;

        sentences.Clear();
        foreach(string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if(sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        string sentence = sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
    }

    // coroutine to display each letter of the sentence, individually in the dialog box
    IEnumerator TypeSentence(string sentence)
    {
        dialogText.text = "";
        foreach(char letter in sentence.ToCharArray())
        {
            dialogText.text += letter;
            yield return new WaitForSeconds(.1f);
            // alternative would be [yield return null]
        }
    }

    public void EndDialogue()
    {
        player.GetComponent<CharacterMovement>().Interacting = false;
        player.GetComponent<Animator>().SetBool("interacting", false);
        Debug.Log("End of conversation");
    }
}
