using TMPro;
using UnityEngine;
using UnityEngine.Localization;

namespace Core.Scripts.UI.Localization
{
    public class TextSwitcher : MonoBehaviour
    {
        #region Fields

        [SerializeField] private TMP_Text uiText;
        [SerializeField] private string[] phraseKeys;
        private int currentPhraseIndex;

        #endregion

        void Start()
        {
            UpdateText();
        }

        public void NextPhrase()
        {
            currentPhraseIndex = (currentPhraseIndex + 1) % phraseKeys.Length;
            UpdateText();
        }

        public void PreviousPhrase()
        {
            currentPhraseIndex = (currentPhraseIndex - 1 + phraseKeys.Length) % phraseKeys.Length;
            UpdateText();
        }

        private async void UpdateText()
        {
            var localizedString = new LocalizedString { TableReference = "UI_Texts", TableEntryReference = phraseKeys[currentPhraseIndex] };

            localizedString.GetLocalizedString();
        }   
    }
}
