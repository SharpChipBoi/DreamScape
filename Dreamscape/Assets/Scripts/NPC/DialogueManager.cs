﻿using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using UnityEngine.Rendering;
using UnityEngine.Events;
using Cinemachine;

//[Serializable]
//public class QuestionEvent : UnityEvent<Question> { }
public class DialogueManager : MonoBehaviour
{

    //public QuestionEvent questionEvent;

    public bool inDialogue;

    public static DialogueManager instance;


    public ObjectDialogue conversation;
    public ObjectDialogue defaultConversation;

    public CanvasGroup canvasGroup;
    public TMP_Animated animatedText;
    public Image nameBubble;
    public TextMeshProUGUI nameTMP;
    public Image choiceBubble;
    public TextMeshProUGUI choiceTM;
    [HideInInspector]
    public NPCScript currentNpc;

    private int dialogueIndex;
    public bool canExit;
    public bool nextDialogue;

    [Space]

    [Header("Cameras")]
    public GameObject gameCam;
    public GameObject dialogueCam;

    [Space]

    public Volume dialogueDof;


    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        animatedText.onDialogueFinish.AddListener(() => FinishDialogue());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && inDialogue)
        {
            if (canExit)
            {
                CameraChange(false);
                FadeUI(false, .2f, 0);
                Sequence s = DOTween.Sequence();
                s.AppendInterval(.8f);
                s.AppendCallback(() => ResetState());
            }

            if (nextDialogue)
            {
                AdvanceLine();
            }
        }
    }
    public void ChangeConversation(ObjectDialogue nextConversation)
    {
        nextDialogue = true;
        inDialogue = false;
        conversation = nextConversation;
        AdvanceLine();
    }

    private void Initialize()
    {
        inDialogue = true;
        dialogueIndex = 0;
    }

    private void AdvanceLine()
    {
        if (conversation == null) return;
        if (!inDialogue) Initialize();

        if (dialogueIndex < conversation.conversationBlock.Count)
            animatedText.ReadText(currentNpc.dialogue.conversationBlock[dialogueIndex]);
        //else
            //AdvanceConversation();
    }
    //private void AdvanceConversation()
    //{
    //    // These are really three types of dialog tree node
    //    // and should be three different objects with a standard interface
    //    if (conversation.question != null)
    //    {
    //        Debug.Log(questionEvent == null);
    //        Debug.Log(questionEvent);
    //        questionEvent.Invoke(conversation.question);
    //    }
    //    else if (conversation.nextConversation != null)
    //        ChangeConversation(conversation.nextConversation);
    //    else
    //        FinishDialogue();
    //}
    public void FadeUI(bool show, float time, float delay)
    {
        Sequence s = DOTween.Sequence();
        s.AppendInterval(delay);
        s.Append(canvasGroup.DOFade(show ? 1 : 0, time));
        if (show)
        {
            dialogueIndex = 0;
            s.Join(canvasGroup.transform.DOScale(0, time * 2).From().SetEase(Ease.OutBack));
            s.AppendCallback(() => animatedText.ReadText(currentNpc.dialogue.conversationBlock[0]));
        }
    }

    public void SetCharNameAndColor()
    {
        nameTMP.text = currentNpc.data.npcName;
        nameTMP.color = currentNpc.data.npcNameColor;
        nameBubble.color = currentNpc.data.npcColor;

    }

    public void CameraChange(bool dialogue)
    {
        gameCam.SetActive(!dialogue);
        dialogueCam.SetActive(dialogue);

        //Depth of field modifier
        float dofWeight = 0;
        DOVirtual.Float(dialogueDof.weight, dofWeight, .8f, DialogueDOF);
    }

    public void DialogueDOF(float x)
    {
        dialogueDof.weight = x;
    }

    public void ClearText()
    {
        animatedText.text = string.Empty;
    }

    public void ResetState()
    {
        //currentNpc.Reset();
        FindObjectOfType<PlayerMovement>().active = true;
        inDialogue = false;
        canExit = false;
    }

    public void FinishDialogue()
    {
        //if (dialogueIndex < currentNpc.dialogue.conversationBlock.Count - 1 && (conversation.nextConversation != null || conversation.question != null))
        //{
        //    dialogueIndex++;
        //    nextDialogue = true;
        //}
        conversation = defaultConversation;
        inDialogue = false;
        canExit = true;
        
    }

}
