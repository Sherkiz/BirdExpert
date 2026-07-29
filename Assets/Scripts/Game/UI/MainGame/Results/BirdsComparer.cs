using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BirdExpert { 
    public class BirdsComparer : UIArea
    {
        [SerializeField] Image goodBirdDisplayer;
        [SerializeField] Image badBirdDisplayer;
        [SerializeField] TextMeshProUGUI goodBirdText;
        [SerializeField] TextMeshProUGUI badBirdText;
        [SerializeField] Button closeButton;

        public override void Initialize(bool active)
        {
            base.Initialize(active);
            closeButton.onClick.AddListener(CloseArea);
            closeButton.SetCodeText("back-to-result");
        }
        public void OpenArea(BirdInfo goodBird, BirdInfo badBird, Sex sex = Sex.All)
        {
            string sexText = string.Empty;
            if (sex == Sex.Male) sexText = " (" + Language.GetLang("male") + ")";
            else if (sex == Sex.Female) sexText = " (" + Language.GetLang("female") + ")";
            goodBirdDisplayer.sprite = goodBird.GetRandomImage(GameSettings.ImageSettings.Base, sex).image;
            Sex badBirdSex = sex;
            if (!badBird.hasSexualDimorphism) badBirdSex = Sex.All;
            badBirdDisplayer.sprite = badBird.GetRandomImage(GameSettings.ImageSettings.Base, badBirdSex).image;
            goodBirdText.SetText(goodBird.GetName(GameManager.Instance.gameMode.lang) + sexText);
            string badBirdSexText = badBird.hasSexualDimorphism ? sexText : " (" + Language.GetLang("no-sexual-dimorphism") + ")";
            badBirdText.SetText(badBird.GetName(GameManager.Instance.gameMode.lang) + badBirdSexText);
            OpenArea();
        }
        public void OpenArea(QuizzAnswer answer) => OpenArea(answer.expectedBird, answer.givenAnswer, answer.birdImage.sex);
    }
}
