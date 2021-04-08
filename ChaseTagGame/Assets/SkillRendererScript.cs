using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace GameLibrary
{
    public class SkillRendererScript : MonoBehaviour
    {
        public Skill Skill;
        [SerializeField] private TextMeshProUGUI skillNameLabel;
        [SerializeField] private Image skillArtwork;
        [SerializeField] private TextMeshProUGUI skillPriceLabel;


        private void Start()
        {
            skillNameLabel.text = Skill.name;
            skillArtwork.sprite = Skill.artwork;
            skillPriceLabel.text = $"Buy - {Skill.price}";
        }

        public void BuySkillButtonClicked()
        {
            var e = new OnSkillBoughtEvent(Skill, gameObject);
            EventManager.Instance.Dispatch(e);
        }
    }
}