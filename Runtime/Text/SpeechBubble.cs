using System;
using TMPro;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Events;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace OSK
{
    [DefaultExecutionOrder(-101)]
    public class SpeechBubble : MonoBehaviour
    {
        public UnityEvent onCompleted;
        public Action onVocal, onWord;

        [SerializeField] private Graphic textArea;
        [SerializeField] private float delayBetweenLetters = 0.02f;
        [SerializeField] private float delayBetweenWords = 0.05f;

        [SerializeField] private bool speechToEnable;
        [SerializeField] private float delaySpeechToEnable;

        [SerializeField] private string message;
        [SerializeField] private bool getMessageToText;

        [SerializeField] private KeyCode keySkip = KeyCode.Mouse0;

        private IEnumerator showHandle;
        private readonly string[] vocals = { "a", "e", "i", "o", "u", "y" };
        private bool done = true;
        private bool showing;
        private Queue<string> queue;

        private void Awake()
        {
            queue = new Queue<string>();
            if (getMessageToText)
                message = textArea is Text ? ((Text)textArea).text : ((TMP_Text)textArea).text;
        }

        private void OnEnable()
        {
            if (speechToEnable)
            {
                if (textArea is Text)
                {
                    var t = (Text)textArea;
                    t.text = "";
                }
                else if (textArea is TMP_Text)
                {
                    var t = (TMP_Text)textArea;
                    t.text = "";
                }

                StartCoroutine(DelayShow());

                IEnumerator DelayShow()
                {
                    yield return new WaitForSeconds(delaySpeechToEnable);
                    Show(message, true);
                }
            }
        }

        public void Show(string text, bool force = false)
        {
            if (force)
            {
                queue.Clear();
                ShowNext(text);
                return;
            }

            if (!done && showing || queue.Any())
            {
                queue.Enqueue(text);
                return;
            }

            ShowNext(text);
        }

        private void ShowNext(string text)
        {
            done = false;
            CancelPrevious();
            message = text;
            showHandle = RevealText();
            StartCoroutine(showHandle);

            if (showing) return;

            PlaySound();
            showing = true;
        }

        private void PlaySound()
        {
            // if (toggleSound)
            // {
            //     playSound
            // }
        }

        private void Update()
        {
            if (!Input.GetKeyDown(keySkip))
                return;
            SkipOrHide();
        }

        private void SkipOrHide()
        {
            if (done)
            {
                HideOrShowNext();
                return;
            }

            Skip();
        }

        private void HideOrShowNext()
        {
            if (queue.Any())
            {
                ShowNext(queue.Dequeue());
                return;
            }

            Hide();
            PlaySound();
        }

        public void Hide()
        {
            showing = false;
        }

        private void CancelPrevious()
        {
            if (showHandle != null)
            {
                StopCoroutine(showHandle);
            }
        }

        private string StripHtmlTags(string input)
        {
            // Dùng biểu thức chính quy để loại bỏ mọi thẻ HTML khỏi chuỗi.
            return Regex.Replace(input, "<.*?>", string.Empty);
        }

        private IEnumerator RevealText()
        {
            int visibleCharCount = 0;
            string fullMessage = message;
            string visibleMessage = "";

            //  use regex to find all html tags
            var matches = Regex.Matches(fullMessage, "<.*?>");
            List<(int index, string tag)> htmlTags = new List<(int, string)>();

            foreach (Match match in matches)
            {
                htmlTags.Add((match.Index, match.Value));
            }

            // delete all html tags
            string strippedMessage = Regex.Replace(fullMessage, "<.*?>", string.Empty);

            while (visibleCharCount <= strippedMessage.Length)
            {
                // build visible message
                visibleMessage = BuildVisibleMessage(strippedMessage, htmlTags, visibleCharCount);

                if (textArea is Text)
                {
                    var t = (Text)textArea;
                    t.text = visibleMessage;
                }

                if (textArea is TMP_Text)
                {
                    var t = (TMP_Text)textArea;
                    t.text = visibleMessage;
                }

                if (visibleCharCount < strippedMessage.Length)
                {
                    string current = strippedMessage[visibleCharCount].ToString();
                    CheckForVocal(current);
                    CheckForWord(visibleCharCount, current);
                }

                float delay = visibleCharCount < strippedMessage.Length && strippedMessage[visibleCharCount] == ' '
                    ? delayBetweenWords
                    : delayBetweenLetters;

                visibleCharCount++;
                yield return new WaitForSeconds(delay);
            }

            RevealDone();
        }

        /// <summary>
        /// build visible message
        /// </summary>
        private string BuildVisibleMessage(string strippedMessage, List<(int index, string tag)> htmlTags,
            int visibleCharCount)
        {
            string visibleText = strippedMessage.Substring(0, visibleCharCount);
            string result = visibleText;

            //  insert html tags
            foreach (var tag in htmlTags)
            {
                if (tag.index <= result.Length)
                {
                    result = result.Insert(tag.index, tag.tag);
                }
            }

            return result;
        }

        private void CheckForWord(int pos, string current)
        {
            if (pos == 0 || current == " ")
            {
                onWord?.Invoke();
            }
        }

        private void RevealDone()
        {
            done = true;
            onCompleted?.Invoke();
        }

        private void CheckForVocal(string current)
        {
            if (vocals.Contains(current))
            {
                onVocal?.Invoke();
            }
        }

        private void Skip()
        {
            CancelPrevious();

            if (textArea is Text)
            {
                var t = (Text)textArea;
                t.text =  message;
            }
            else if (textArea is TMP_Text)
            {
                var t = (TMP_Text)textArea;
                t.text = message;
            }

            RevealDone();
        }
    }
}