using System.Xml.Linq;
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
        [SerializeField] Button inverseSexButton;
        private QuizzAnswer currentAnswer;
        private BirdImage badBirdImage;
        private BirdImage goodBirdOtherSexImage;
        private BirdImage badBirdOtherSexImage;
        private Sex currentDisplayedSex;
        public override void Initialize(bool active)
        {
            base.Initialize(active);
        }
        public void OpenArea(BirdInfo goodBird, BirdInfo badBird, BirdImage questionImage)
        {
            goodBirdOtherSexImage = new(null, Sex.None);
            badBirdOtherSexImage = new(null, Sex.None);
            Sex sex = questionImage.sex;
            currentDisplayedSex = sex;
            goodBirdDisplayer.sprite = questionImage.image;
            goodBirdText.SetText(goodBird.GetName(GameManager.Instance.gameMode.lang) + GetSexText(sex));
            Sex badBirdSex = badBird.hasSexualDimorphism ? Sex.All : Sex.None;
            if (goodBird.hasSexualDimorphism && badBird.hasSexualDimorphism) badBirdSex = sex;
            badBirdImage = badBird.GetRandomImage(GameSettings.ImageSettings.Base, badBirdSex);
            badBirdDisplayer.sprite = badBirdImage.image;
            badBirdSex = badBirdImage.sex;
            if (currentDisplayedSex == Sex.None) currentDisplayedSex = badBirdSex;
            badBirdText.SetText(badBird.GetName(GameManager.Instance.gameMode.lang) + GetSexText(badBirdSex));
            inverseSexButton.gameObject.SetActive(goodBird.hasSexualDimorphism || badBird.hasSexualDimorphism);
            OpenArea();
        }
        public void OpenArea(QuizzAnswer answer) 
        {
            currentAnswer = answer;
            OpenArea(answer.expectedBird, answer.givenAnswer, answer.birdImage); 
        }
        public void InverseSex()
        {
            Sex newSex = EnumUtilities.GetOppositeSex(currentDisplayedSex);
            string sexText = GetSexText(newSex);
            if (currentAnswer.birdImage.sex != Sex.None)
            {
                if (newSex == currentAnswer.birdImage.sex)
                {
                    goodBirdDisplayer.sprite = currentAnswer.birdImage.image;
                }
                else
                {
                    if (goodBirdOtherSexImage.image == null) goodBirdOtherSexImage = currentAnswer.expectedBird.GetRandomImage(GameSettings.ImageSettings.Base, newSex);
                    goodBirdDisplayer.sprite = goodBirdOtherSexImage.image;
                }
                goodBirdText.SetText(currentAnswer.expectedBird.GetName(GameManager.Instance.gameMode.lang) + sexText);
            }
            if (currentAnswer.givenAnswer.hasSexualDimorphism)
            {
                if (newSex == badBirdImage.sex)
                {
                    badBirdDisplayer.sprite = badBirdImage.image;
                }
                else
                {
                    if (badBirdOtherSexImage.image == null) badBirdOtherSexImage = currentAnswer.givenAnswer.GetRandomImage(GameSettings.ImageSettings.Base, newSex);
                    badBirdDisplayer.sprite = badBirdOtherSexImage.image;
                }
                badBirdText.SetText(currentAnswer.givenAnswer.GetName(GameManager.Instance.gameMode.lang) + sexText);
            }
            currentDisplayedSex = newSex;
        }
        private string GetSexText(Sex sex)
        {
            return sex switch
            {
                Sex.Male => " (" + Language.GetLang("male") + ")",
                Sex.Female => " (" + Language.GetLang("female") + ")",
                Sex.None => " (" + Language.GetLang("no-sexual-dimorphism") + ")",
                _ => ""
            };
        }
    }
}
