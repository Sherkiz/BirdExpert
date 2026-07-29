using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace BirdExpert
{
    public class InputFieldManager : MonoBehaviour
    {
        [SerializeField] private TMP_InputField inputField;
        [SerializeField] private RectTransform resultsParent;
        [SerializeField] private AutoFillText autoFillText;
        [SerializeField] private GameObject validationButton;
        [SerializeField] private RectTransform scrollViewContainer;
        [SerializeField] private ScrollRect scrollRect;
        private QuizzArea quizzArea;
        private string selectedName;
        private bool answerCompleted;
        private int selectedTextBoxIndex;
        private List<AutoFillText> textBoxesList;
        private List<string> birdNames;
        public bool Interactable { set { if (inputField != null) inputField.interactable = value; } }
        private Vector2 contentSize=> scrollRect.content.rect.size;
        private Vector2 contentScale => scrollRect.content.localScale;
        private Vector2 viewportSize => scrollRect.viewport.rect.size;

        private void Awake()
        {
            inputField.onValueChanged.AddListener(OnInputValueChanged);
            answerCompleted = false;
            selectedTextBoxIndex = 0;
            textBoxesList = new();
            scrollViewContainer.gameObject.SetActive(false);
        }
        public void Initialize(QuizzArea quizzArea)
        {
            this.quizzArea = quizzArea;
            Interactable = true;
            birdNames = BirdsManager.GetAllNamesInLang(quizzArea.gameMode.lang);
            InitializeValidationButton();
        }

        private void Update()
        {
            GetInputs();
        }

        private void GetInputs()
        {
            if (Keyboard.current.enterKey.wasPressedThisFrame)
            {
                OnEnterPressed();
            }

            if (Keyboard.current.downArrowKey.wasPressedThisFrame) OnArrowPressed(false); 
            if (Keyboard.current.upArrowKey.wasPressedThisFrame) OnArrowPressed(true);
        }

        private void OnInputValueChanged(string newText)
        {
            ClearResults();
            FillResults(GetResults(newText));
        }

        private void ClearResults()
        {
            selectedName = null;
            selectedTextBoxIndex = 0;
            foreach (var textBox in textBoxesList) textBox.OnTextClicked = null;
            textBoxesList.Clear();
            for (int childIndex = resultsParent.childCount - 1; childIndex >= 0; --childIndex)
            {
                Transform child = resultsParent.GetChild(childIndex);
                child.SetParent(null);
                Destroy(child.gameObject);
            }
        }

        private void FillResults(List<string> results)
        {
            int resultIndex = 0;
            if (results.Count > 0)
            {
                scrollViewContainer.gameObject.SetActive(true);
                while (resultIndex < results.Count)
                {
                    string birdName = results[resultIndex];
                    AutoFillText textBox = Instantiate(autoFillText, resultsParent).GetComponent<AutoFillText>();
                    textBox.BirdName = birdName;
                    textBox.OnTextClicked += () => inputField.text = textBox.BirdName;
                    textBoxesList.Add(textBox);
                    if (resultIndex == 0)
                    {
                        SelectTextBox(textBox);
                        selectedTextBoxIndex = 0;
                    }
                    resultIndex++;
                }
            }
            else
            {
                scrollViewContainer.gameObject.SetActive(false);
            }
        }


        private List<string> GetResults(string input)
        {
            OnAnswerFilled(input);
            if (!(input == null || input == "" || input == " ")) return birdNames.FindAll((str) => str.ToLower().Contains(input.ToLower()));
            return new List<string>();
        }

        private bool DoesInputExist(string input)
        {
            return birdNames.Contains(input);
        }

        private void OnAnswerFilled(string input)
        {
            bool exists = DoesInputExist(input);
            validationButton.SetActive(exists);
            answerCompleted = exists;
        }

        private void InitializeValidationButton()
        {
            validationButton.GetComponent<Button>().onClick.AddListener(ValidateAnswer);
            validationButton.SetActive(false);
        }

        private void ValidateAnswer()
        {
            quizzArea.ProcessAnswer(inputField.text);
            inputField.text = "";
        }

        private void SelectTextBox(AutoFillText textBox, bool select = true)
        {
            textBox.SelectTextBox(select);
            if (select)
            {
                UpdateViewportContentPosition(textBox);
                selectedName = textBox.BirdName;
            }
        }

        private void OnEnterPressed()
        {
            if (answerCompleted)
            {
                ValidateAnswer();
            }
            else
            {
                if (selectedName != null)
                {
                    Debug.Log("Selecting " + selectedName);
                    inputField.text = selectedName;
                }
                else
                {
                    Debug.Log("No selected name");
                }
            }
        }

        private void OnArrowPressed(bool up)
        {
            if (textBoxesList.Count > 0)
            {
                if (selectedTextBoxIndex != -1)
                {
                    SelectTextBox(textBoxesList[selectedTextBoxIndex], false);
                    if (up && selectedTextBoxIndex == 0) { selectedTextBoxIndex = textBoxesList.Count - 1; }
                    else
                    {
                        selectedTextBoxIndex += up ? -1 : 1;
                        selectedTextBoxIndex %= resultsParent.childCount;
                    }
                    SelectTextBox(textBoxesList[selectedTextBoxIndex]);
                }
            }
        }
        private void UpdateViewportContentPosition(AutoFillText autoFillText)
        {
            Vector2 scrollPosition = scrollRect.normalizedPosition;
            float textHeight = Mathf.Abs(scrollRect.content.InverseTransformPoint(autoFillText.transform.TransformPoint(autoFillText.Rect.rect.center)).y);
            if (contentSize.y > viewportSize.y)
            {
                scrollPosition.y = 1 - Mathf.Clamp01((textHeight - viewportSize.y * 0.5f) / (contentSize.y - viewportSize.y));
            }
            scrollRect.normalizedPosition = scrollPosition;
        }
        public void ResetInputField()
        {
            inputField.text = "";
        }
    }
}
