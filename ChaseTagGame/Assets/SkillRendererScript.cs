using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

namespace GameLibrary
{
    public class SkillRendererScript : MonoBehaviour
    {
        public Skill Skill;
        [SerializeField] private TextMeshProUGUI skillNameLabel;
        [SerializeField] private Image skillArtwork;
        [SerializeField] private TextMeshProUGUI skillPriceLabel;
        [SerializeField] private Button skillBuyButton;


        private void Start()
        {
            skillNameLabel.text = Skill.name;
            skillArtwork.sprite = Skill.artwork;
            skillPriceLabel.text = $"Buy - {Skill.price}";
            skillBuyButton.interactable = GameManager.WaveManager.Currency >= Skill.price;
            EventManager.Instance.AddListener<OnSkillBoughtEvent>(OnSkillBought);
        }

        private void OnDestroy()
        {
            EventManager.Instance.RemoveListener<OnSkillBoughtEvent>(OnSkillBought);
        }

        private void OnSkillBought(OnSkillBoughtEvent e)
        {
            if (e.Skill == Skill)
                return;

            skillBuyButton.interactable = e.NewCurrency >= Skill.price;
        }

        public void BuySkillButtonClicked()
        {
            var newCurrency = GameManager.WaveManager.Currency - Skill.price;
            var e = new OnSkillBoughtEvent(Skill, gameObject, newCurrency);
            EventManager.Instance.Dispatch(e);
        }
    }
}