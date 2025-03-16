using System;
using System.Collections.Generic;
using Core.Scripts.Data;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously

namespace Core.Scripts.UI.Localization
{
    [Serializable]
    public class TextSwitcher
    {
        #region Fields

        [SerializeField] private List<string> _phraseKeys;
        
        private TMP_Text _uiText;
        private int _currentPhraseIndex;

        #endregion

        public void Init(TMP_Text uiText)
        {
            _uiText = uiText;
        }

        //TODO Refactor
        public void SetLocalization(LocalizationType localizationKey)
        {
            _currentPhraseIndex = _phraseKeys.FindIndex(key => key == localizationKey.ToString());
            UpdateText().Forget();
        }

        private async UniTaskVoid UpdateText()
        {
            var localizedString = new LocalizedString { TableReference = "UI_Texts", TableEntryReference = _phraseKeys[_currentPhraseIndex] };

            _uiText.text = localizedString.GetLocalizedString();
        }   
    }
}
