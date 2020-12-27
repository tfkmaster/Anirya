using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class DialogueManager : MonoBehaviour
{
    public Text nametext;
    public Text dialogText;

    public bool dialogHasStart = false;
    public bool dialogEnded = false;
    private Queue<string> sentences;
    private Queue<string> names;

    private GameObject player;

    private string curr_sentence = "";
    private string curr_name = "";

    private bool sentence_fully_displayed = false;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        sentences = new Queue<string>();
        names = new Queue<string>();
    }

    void Update()
    {
        if(player == null) GameObject.FindGameObjectWithTag("Player");
        if (dialogHasStart && (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown("joystick button 0")))
        {
            if (sentence_fully_displayed)
            {
                DisplayNextSentence();
            }
            else
            {
                StopAllCoroutines();
                ForceTypeSentence(curr_sentence);
            }
        }
    }

    public void StunAniryaForDialoguePurpose()
    {
        player.GetComponent<CharacterMovement>().Interacting = true;
        player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        player.GetComponentInChildren<Animator>().SetBool("interacting", true);
    }

    public void StartDialogue(Dialogue dialogue)
    {
        names.Clear();
        Debug.Log(dialogue.names);
        foreach (string name in dialogue.names)
        {
            Debug.Log(name);
            names.Enqueue(name);
        }
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
        sentence_fully_displayed = false;

        curr_sentence = sentences.Dequeue();
        curr_name = names.Dequeue();
        StopAllCoroutines();
        TypeName(curr_name);

        StartCoroutine(TypeSentence(curr_sentence));
    }

    public void ForceTypeSentence(string sentence)
    {
        dialogText.text = sentence;
        sentence_fully_displayed = true;
    }

    // coroutine to display each letter of the sentence, individually in the dialog box
    IEnumerator TypeSentence(string sentence)
    {
        dialogText.text = "";
        foreach(char letter in sentence.ToCharArray())
        {
            dialogText.text += letter;
            if(dialogText.text.Length == sentence.Length)
            {
                sentence_fully_displayed = true;
            }
            yield return new WaitForSeconds(.025f);
            // alternative would be [yield return null]
        }
    }

    public void TypeName(string name)
    {
        nametext.text = name;
    }

    public void EndDialogue()
    {
        player.GetComponent<CharacterMovement>().Interacting = false;
        player.GetComponentInChildren<Animator>().SetBool("interacting", false);
        dialogEnded = true;
    }
}
