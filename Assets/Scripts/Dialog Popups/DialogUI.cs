using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace EasyUI.Dialogs {

    public class Dialog {
        public string Title = "Title";
        public string Message = "Message goes here.";
    }

    public class DialogUI : MonoBehaviour
    {
        [SerializeField] GameObject canvas;
        [SerializeField] Text titleUIText;
        [SerializeField] Text messageUIText;
        [SerializeField] Button closeUIButton;
        
        Queue<Dialog> dialogsQueue = new Queue<Dialog>();
        Dialog dialog = new Dialog();

        public bool IsActive = false;

        public static DialogUI Instance;

        void Awake() {
            Instance = this;

            closeUIButton.onClick.RemoveAllListeners();
            closeUIButton.onClick.AddListener(Hide);
        }

        public DialogUI SetTitle (string title) {
            dialog.Title = title;
            return Instance;
        }

          public DialogUI SetMessage (string message) {
            dialog.Message = message;
            return Instance;
        }


        public void Show() {
            dialogsQueue.Enqueue(dialog);
            dialog = new Dialog();

            if(!IsActive)
                ShowNextDialog();
            
        }

        void ShowNextDialog() {
            Dialog tempDialog = dialogsQueue.Dequeue();
            titleUIText.text = tempDialog.Title;
            messageUIText.text = tempDialog.Message;

            IsActive = true;
            canvas.SetActive(true);
        }

         public void Hide() {
            canvas.SetActive(false);
            IsActive = false;

            
            if (dialogsQueue.Count != 0)
                ShowNextDialog();
        }
    }

}